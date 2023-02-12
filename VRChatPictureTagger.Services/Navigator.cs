using System;
using System.Collections.Generic;

using CommunityToolkit.Diagnostics;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Navigation;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services
{
	public class Navigator : INavigator, IDisposable
	{
		private readonly IViewFactory _viewFactory;
		private readonly ILogger<Navigator> _logger;
		private readonly IOptionsMonitor<MainSettings> _options;
		private readonly IDisposable _optionsListener;
		private readonly IOptions<WindowAndNavigationOptions> _windowOptions;
		private readonly Stack<IView> _viewStack;
		private readonly Frame _contentFrame;

		public IView CurrentView { get; private set; }
		public IViewModel CurrentViewModel { get; private set; }

		public bool BackNavigationEnabled => _options.CurrentValue.UseBackNavigation;
		public bool CanGoBack => _options.CurrentValue.UseBackNavigation ? _viewStack.Count > 0 : false;

		public Navigator(
			ILogger<Navigator> logger,
			IOptionsMonitor<MainSettings> options,
			IOptions<WindowAndNavigationOptions> windowOptions,
			IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
			_logger = logger;
			_options = options;
			_windowOptions = windowOptions;
			_viewStack = new Stack<IView>();
			_contentFrame = _windowOptions.Value.ContentFrame;

			_optionsListener = _options.OnChange(SettingsChanged);
		}

		public bool CanNavigateTo(string friendlyName)
			=> _viewFactory.IsViewRegistered(friendlyName) && _viewFactory.IsViewModelRegistered(friendlyName);

		public void NavigateTo<TViewModel>() where TViewModel : class, IViewModel
		{
			try
			{
				Page resolvedView = _viewFactory.ResolveView<TViewModel>();
				NavigateToInternal(resolvedView);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occured trying to navigate to ViewModel {TViewModel}", typeof(TViewModel));
				throw;
			}
		}
		public void NavigateTo(string friendlyName)
		{
			Guard.IsNotNullOrWhiteSpace(friendlyName);

			if (!_viewFactory.IsViewRegistered(friendlyName) || !_viewFactory.IsViewModelRegistered(friendlyName))
				ThrowHelper.ThrowArgumentException(nameof(friendlyName), $"friendlyName {friendlyName} is not registered");

			try
			{
				Page newPage = _viewFactory.ResolveView(friendlyName);
				NavigateToInternal(newPage);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occured trying to navigate to {friendlyName}", friendlyName);
				throw;
			}
		}

		public void NavigateBack()
		{
			if (!BackNavigationEnabled)
				ThrowHelper.ThrowInvalidOperationException("Navigation Stack is disabled. You have to enable the Navigation Stack in order to navigate back.");

			try
			{
				_logger.LogInformation("Attempting to navigate back");

				if (_viewStack.Count > 1)
				{
					_ = _viewStack.Pop();
					Page lastView = _viewStack.Peek() as Page;
					NavigateToInternal(lastView, true);
				}
				else
				{
					_viewStack.Clear();
					_logger.LogInformation("No Views left on the Stack. Show empty page");
					CurrentView = null;
					CurrentViewModel = null;
					_windowOptions.Value.UIDispatcher.TryEnqueue(() => { _contentFrame.Content = null; });
				}
				_logger.LogInformation("Successfully navigated back");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occured trying to navigate back");
				throw;
			}
		}

		private void NavigateToInternal(Page newPage, bool isBackNavigation = false)
		{
			Guard.IsNotNull(newPage, nameof(newPage));

			try
			{
				_logger.LogInformation("Attempting to navigate to page {newPage}", newPage);

				CurrentViewModel?.NavigatedFrom();

				CurrentView = newPage as IView;
				CurrentViewModel = newPage.DataContext as IViewModel;

				if (!isBackNavigation && BackNavigationEnabled)
					_viewStack.Push(CurrentView);

				CurrentViewModel.NavigatedTo();

				//Since this could be called from anywhere, make sure the content is changed on the UI Thread
				_windowOptions.Value.UIDispatcher.TryEnqueue(() => { _contentFrame.Content = newPage; });

				_logger.LogInformation("Successfully navigated to page {newPage}", newPage);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occured trying to navigate to Page {newPage}", newPage);
				throw;
			}
		}
		public void SettingsChanged(MainSettings _)
		{
			_viewStack.Clear();
			if (_options.CurrentValue.UseBackNavigation)
				_viewStack.Push(CurrentView);
		}

		public void Dispose()
		{
			_optionsListener?.Dispose();
			_viewStack.Clear();
		}
	}
}
