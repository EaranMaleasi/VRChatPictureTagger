// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using VRChatPictureTagger.Bootstrap;
using VRChatPictureTagger.Interfaces.Services;
using VRChatPictureTagger.ViewModels;
using VRChatPictureTagger.Views.Root;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VRChatPictureTagger
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : Application
	{
		private IHost _appHost;
		public static Window _mainWindow { get; private set; }

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			_mainWindow = new();

			_appHost = AppConfiguration.BuildHost();
			AppConfiguration.ConfigureViews(_appHost.Services);
			AppConfiguration.ConfigureServices(_appHost.Services);

			var navView = _appHost.Services.GetService<NavigationPage>();
			var navViewModel = _appHost.Services.GetService<NavigationBaseViewModel>();

			INavigator navigator = _appHost.Services.GetService<INavigator>();
			navigator.Initialize(navView.GetRootFrame());

			navView.DataContext = navViewModel;
			_mainWindow.Content = navView;

			navView.RootWindow = _mainWindow;


			_mainWindow.Activate();
		}
	}
}
