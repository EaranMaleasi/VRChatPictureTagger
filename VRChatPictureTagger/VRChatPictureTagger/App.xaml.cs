// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Bootstrap;
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
		private ILogger<App> _logger;

		public static DispatcherQueue UiDispatcherQueue { get; private set; }

		public static Window MainWindow { get; private set; }
		public static Frame ContentFrame { get; private set; }

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();
			UnhandledException += App_UnhandledException;
		}

		private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			_logger?.LogCritical(e.Exception, "Unhandled exception {message} occured", e.Message);
		}

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs args)
		{
			MainWindow = new();

			NavigationPage navView = new();
			ContentFrame = navView.GetRootFrame();

			UiDispatcherQueue = DispatcherQueue.GetForCurrentThread();

			_appHost = AppConfiguration.BuildHost();

			AppConfiguration.ValidateSettings(_appHost.Services);
			AppConfiguration.ConfigureViews(_appHost.Services);

			_appHost.Start();

			_logger = _appHost.Services.GetService<ILogger<App>>();

			var navViewModel = _appHost.Services.GetService<NavigationBaseViewModel>();

			navView.DataContext = navViewModel;
			MainWindow.Content = navView;

			navView.RootWindow = MainWindow;
			MainWindow.Activate();


		}
	}
}
