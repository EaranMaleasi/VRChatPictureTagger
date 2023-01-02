using System;
using System.Collections.Generic;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Core.Strings;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.ViewModels
{
	public class NavigationBaseViewModel : ObservableObject
	{
		private List<string> _menuEntries;
		private NavigationViewItem _selectedItem;
		readonly INavigator _navigator;

		public List<string> MenuEntries
		{
			get => _menuEntries;
			set => SetProperty(ref _menuEntries, value);
		}

		public NavigationViewItem SelectedItem
		{
			get => _selectedItem;
			set
			{
				SetProperty(ref _selectedItem, value);
				GoToView(_selectedItem);
			}
		}

		public NavigationBaseViewModel(INavigator navigator)
		{
			MenuEntries = new List<string>() { "Menu 1", "Menu 2" };
			_navigator = navigator;
			GoToViewCommand = new RelayCommand<NavigationViewItem>(GoToView);
		}


		public ICommand GoToViewCommand { get; }
		public void GoToView(NavigationViewItem menuItem)
		{
			try
			{
				if (menuItem.Content.ToString() == "Einstellungen")
				{
					_navigator.NavigateTo(FriendlyNames.Settings);
					return;
				}
				if (_navigator.CanNavigateTo(menuItem.Name))
					_navigator.NavigateTo(menuItem.Name);
			}
			catch (Exception)
			{

			}
		}
	}
}
