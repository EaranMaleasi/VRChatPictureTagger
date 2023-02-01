using System;
using System.Collections.Generic;

using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Core.Messages;
using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Navigation;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services
{
	public class Navigator : INavigator, IRecipient<UseBackNavigationChanged>, IDisposable
	{
		private readonly IViewFactory _viewFactory;
		private readonly IMessenger _messenger;
		private readonly ILogger<Navigator> _logger;
		private readonly IOptions<MainSettings> _options;
		private readonly IOptions<WindowAndNavigationOptions> _windowOptions;
		private readonly Stack<IView> _viewStack;
		private readonly Frame _contentFrame;

		private IView _currentView;
		private IViewModel _currentViewModel;

		public bool BackNavigationEnabled => _options.Value.UseBackNavigation;
		public bool CanGoBack => _options.Value.UseBackNavigation ? _viewStack.Count > 0 : false;

		public Navigator(
			ILogger<Navigator> logger,
			IOptions<MainSettings> options,
			IOptions<WindowAndNavigationOptions> windowOptions,
			IViewFactory viewFactory,
			IMessenger messenger)
		{
			_viewFactory = viewFactory;
			_messenger = messenger;
			_logger = logger;
			_options = options;
			_windowOptions = windowOptions;
			_viewStack = new Stack<IView>();
			_contentFrame = _windowOptions.Value.ContentFrame;

			_messenger.Register(this);
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
					_logger.LogInformation("No Views left on the Stack. Show empty page");
					_currentView = null;
					_currentViewModel = null;
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

				_currentViewModel?.NavigatedFrom();

				_currentView = newPage as IView;
				_currentViewModel = newPage.DataContext as IViewModel;

				if (!isBackNavigation && BackNavigationEnabled)
					_viewStack.Push(_currentView);

				_currentViewModel.NavigatedTo();

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
		public void Receive(UseBackNavigationChanged message)
		{
			_viewStack.Clear();
			if (message.UseBackNavigation)
				_viewStack.Push(_currentView);
		}

		public void Dispose()
		{
			_messenger.UnregisterAll(this);
			_viewStack.Clear();
		}
	}
}
