// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using Windows.Foundation.Metadata;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VRChatPictureTagger.Views.Root
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NavigationPage : Page
	{
		public Window RootWindow { get; set; }
		public Action NavigationViewLoaded { get; set; }
		public PageHeader PageHeader
		{
			get
			{
				return UIHelper.GetDescendantsOfType<PageHeader>(NavigationViewControl).FirstOrDefault();
			}
		}

		public NavigationPage()
		{
			this.InitializeComponent();

			NavigationViewControl.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
			NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Required;
			Loaded += NavigationPage_Loaded;
		}

		public void Navigate<TPage>(TPage page) where TPage : Page
		{

		}

		private void NavigationPage_Loaded(object sender, RoutedEventArgs e)
		{
			RootWindow.ExtendsContentIntoTitleBar = true;
			RootWindow.SetTitleBar(AppTitleBar);
		}

		private void OnNavigationViewControlLoaded(object sender, RoutedEventArgs e)
		{
			Task.Delay(500).ContinueWith(_ => this.NavigationViewLoaded?.Invoke(), TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void NavigationViewControl_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
		{
			UpdateAppTitleMargin(sender);
		}

		private void NavigationViewControl_PaneOpening(NavigationView sender, object args)
		{
			UpdateAppTitleMargin(sender);
		}

		private void NavigationViewControl_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
		{
			Thickness currMargin = AppTitleBar.Margin;
			if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
			{
				AppTitleBar.Margin = new Thickness() { Left = (sender.CompactPaneLength * 2), Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };

			}
			else
			{
				AppTitleBar.Margin = new Thickness() { Left = sender.CompactPaneLength, Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };
			}

			UpdateAppTitleMargin(sender);
			UpdateHeaderMargin(sender);
		}


		private void UpdateAppTitleMargin(NavigationView sender)
		{
			const int smallLeftIndent = 4, largeLeftIndent = 24;

			if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
			{
				AppTitle.TranslationTransition = new Vector3Transition();

				if ((sender.DisplayMode == NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
						 sender.DisplayMode == NavigationViewDisplayMode.Minimal)
				{
					AppTitle.Translation = new System.Numerics.Vector3(smallLeftIndent, 0, 0);
				}
				else
				{
					AppTitle.Translation = new System.Numerics.Vector3(largeLeftIndent, 0, 0);
				}
			}
			else
			{
				Thickness currMargin = AppTitle.Margin;

				if ((sender.DisplayMode == NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
						 sender.DisplayMode == NavigationViewDisplayMode.Minimal)
				{
					AppTitle.Margin = new Thickness() { Left = smallLeftIndent, Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };
				}
				else
				{
					AppTitle.Margin = new Thickness() { Left = largeLeftIndent, Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };
				}
			}
		}

		private void UpdateHeaderMargin(NavigationView sender)
		{
			if (PageHeader != null)
			{
				if (sender.DisplayMode == NavigationViewDisplayMode.Minimal)
				{
					PageHeader.HeaderPadding = (Thickness)Resources["PageHeaderMinimalPadding"];
				}
				else
				{
					PageHeader.HeaderPadding = (Thickness)Resources["PageHeaderDefaultPadding"];
				}
			}
		}


		public Frame GetRootFrame() => rootFrame;
	}

	public static class UIHelper
	{
		public static bool IsScreenshotMode { get; set; }
#if UNPACKAGED
        public static StorageFolder ScreenshotStorageFolder { get; set; } = Task.Run(async () => await StorageFolder.GetFolderFromPathAsync(System.AppContext.BaseDirectory)).Result;
#else
		public static StorageFolder ScreenshotStorageFolder { get; set; } = ApplicationData.Current.LocalFolder;
#endif

		public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject
		{
			return start.GetDescendants().OfType<T>();
		}

		public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start)
		{
			var queue = new Queue<DependencyObject>();
			var count1 = VisualTreeHelper.GetChildrenCount(start);

			for (int i = 0; i < count1; i++)
			{
				var child = VisualTreeHelper.GetChild(start, i);
				yield return child;
				queue.Enqueue(child);
			}

			while (queue.Count > 0)
			{
				var parent = queue.Dequeue();
				var count2 = VisualTreeHelper.GetChildrenCount(parent);

				for (int i = 0; i < count2; i++)
				{
					var child = VisualTreeHelper.GetChild(parent, i);
					yield return child;
					queue.Enqueue(child);
				}
			}
		}
	}
}
