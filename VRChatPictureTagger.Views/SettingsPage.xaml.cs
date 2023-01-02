// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Core.Strings;
using VRChatPictureTagger.Interfaces.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VRChatPictureTagger.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsPage : Page, IView
	{
		public string FriendlyName => FriendlyNames.Settings;

		public SettingsPage()
		{
			this.InitializeComponent();
		}


	}
}
