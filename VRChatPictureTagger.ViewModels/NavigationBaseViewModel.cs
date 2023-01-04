using System;
using System.Collections.Generic;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.ViewModels
{
	public class NavigationBaseViewModel : ObservableObject
	{
		private List<string> _menuEntries;
		private object _selectedItem;
		private bool _isSettingsItem;
		private List<string> _menuFooterEntries;
		readonly INavigator _navigator;
		readonly ILogger<NavigationBaseViewModel> _logger;

		public List<string> MenuEntries
		{
			get => _menuEntries;
			set => SetProperty(ref _menuEntries, value);
		}

		public List<string> MenuFooterEntries
		{
			get => _menuFooterEntries;
			set => SetProperty(ref _menuFooterEntries, value);
		}

		public object SelectedItem
		{
			get => _selectedItem;
			set
			{
				SetProperty(ref _selectedItem, value);
				GoToView(_selectedItem);
			}
		}

		public bool IsSettingsItem
		{
			get => _isSettingsItem;
			set => SetProperty(ref _isSettingsItem, value);
		}

		public NavigationBaseViewModel(INavigator navigator, ILogger<NavigationBaseViewModel> logger)
		{
			MenuEntries = new List<string>() { "Menu 1", "Menu 2" };
			MenuFooterEntries = new List<string>() { "Logs" };
			_navigator = navigator;
			_logger = logger;
			GoToViewCommand = new RelayCommand<NavigationViewItem>(GoToView);
			SelectionChangedCommand = new RelayCommand<object>(SelectionChanged);
		}

		public ICommand SelectionChangedCommand { get; }

		private void SelectionChanged(object selectionArgs)
		{
			_logger.LogInformation("this actually worked, type is {type}", selectionArgs.GetType());
		}

		public ICommand GoToViewCommand { get; }
		public void GoToView(object menuItem)
		{
			try
			{
				if (_isSettingsItem && menuItem is NavigationViewItem settingsItem)
				{
					_navigator.NavigateTo(settingsItem.Tag.ToString());
					return;
				}

				string friendlyName = string.Empty;

				if (menuItem is string menuString && _navigator.CanNavigateTo(menuString))
					friendlyName = menuString;

				if (menuItem is NavigationViewItem navItem && _navigator.CanNavigateTo(navItem.Tag.ToString()))
					friendlyName = navItem.Tag.ToString();

				if (!string.IsNullOrWhiteSpace(friendlyName))
					_navigator.NavigateTo(friendlyName);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Exception occured trying to navigate to item {menuitem}", menuItem);
			}
		}
	}
}
