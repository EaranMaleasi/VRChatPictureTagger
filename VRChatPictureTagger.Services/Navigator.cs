using System;

using CommunityToolkit.Diagnostics;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Interfaces.Navigation;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services
{
	public class Navigator : INavigator
	{
		private readonly IViewFactory _viewFactory;
		private readonly ILogger<Navigator> _logger;
		private Frame _navigationFrame;

		private IView _currentView;
		private IViewModel _currentViewModel;

		public bool IsNavigatorInitialized { get; private set; }

		public Navigator(IViewFactory viewFactory, ILogger<Navigator> logger)
		{
			_viewFactory = viewFactory;
			_logger = logger;
		}

		public void Initialize(Frame navigationFrame)
		{
			if (IsNavigatorInitialized)
				return;

			_navigationFrame = navigationFrame;
			_navigationFrame.IsNavigationStackEnabled = true;
			IsNavigatorInitialized = true;

			_logger.LogInformation("Navigator initialized");
		}

		public bool CanNavigateTo(string friendlyName)
			=> _viewFactory.IsViewRegistered(friendlyName);

		public void NavigateTo<TViewModel>() where TViewModel : class, IViewModel
		{
			Guard.IsTrue(IsNavigatorInitialized);

			NavigateTo(_viewFactory.ResolveViewType<TViewModel>());
		}

		public void NavigateTo(string friendlyName)
		{
			Guard.IsTrue(IsNavigatorInitialized);
			Guard.IsNotNullOrWhiteSpace(friendlyName);

			if (_viewFactory.IsViewRegistered(friendlyName))
				NavigateTo(_viewFactory.ResolveViewType(friendlyName));
		}
		private void NavigateTo(Type viewType)
		{
			try
			{
				_currentViewModel?.NavigatedFrom();

				_navigationFrame.Navigate(viewType);

				var view = _navigationFrame.Content as Page;
				if (view != null && view.DataContext != null)
					view.DataContext = _viewFactory.ResolveViewModel((IView)view);

				_currentViewModel = view.DataContext as IViewModel;
				_currentView = view as IView;

				_currentViewModel.NavigatedTo();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Exception occured navigating to View {ViewType}", viewType.Name);
				throw;
			}
		}
	}
}
