using System;
using System.Collections.Generic;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Core.Strings;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.ViewModels
{
	public class NavigationBaseViewModel : ObservableObject
	{
		private List<string> _menuEntries;
		private object _selectedItem;
		private bool _isSettingsItem;
		readonly INavigator _navigator;
		readonly ILogger<NavigationBaseViewModel> _logger;

		public List<string> MenuEntries
		{
			get => _menuEntries;
			set => SetProperty(ref _menuEntries, value);
		}

		public object SelectedItem
		{
			get => _selectedItem;
			set
			{
				SetProperty(ref _selectedItem, value);
				GoToView((NavigationViewItem)_selectedItem);
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
		public void GoToView(NavigationViewItem menuItem)
		{
			try
			{
				if (_isSettingsItem)
				{
					_navigator.NavigateTo(FriendlyNames.Settings);
					return;
				}
				if (_navigator.CanNavigateTo((string)menuItem.Tag))
					_navigator.NavigateTo((string)menuItem.Tag);
			}
			catch (Exception)
			{

			}
		}
	}
}
