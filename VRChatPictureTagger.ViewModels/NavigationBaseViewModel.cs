using System;
using System.Collections.Generic;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Core.Messages;
using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.ViewModels
{
	public class NavigationBaseViewModel : ObservableObject, IRecipient<UseBackNavigationChanged>, IDisposable
	{
		private List<string> _menuEntries;
		private object _selectedItem;
		private bool _isSettingsItem;
		private List<string> _menuFooterEntries;
		private bool? _isBackNavigationEnabled;
		readonly INavigator _navigator;
		readonly IMessenger _messenger;
		readonly ILogger<NavigationBaseViewModel> _logger;
		readonly IOptions<MainSettings> _options;

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

		public bool? IsBackNavigationEnabled
		{
			get => _isBackNavigationEnabled;
			set => SetProperty(ref _isBackNavigationEnabled, value);
		}

		public NavigationBaseViewModel(
			ILogger<NavigationBaseViewModel> logger,
			IOptions<MainSettings> options,
			INavigator navigator,
			IMessenger messenger)
		{
			MenuEntries = new List<string>() { "Menu 1", "Menu 2" };
			MenuFooterEntries = new List<string>() { "Logs" };

			_navigator = navigator;
			_messenger = messenger;
			_logger = logger;
			_options = options;

			_messenger.Register(this);

			IsBackNavigationEnabled = _options.Value.UseBackNavigation;

			GoToViewCommand = new RelayCommand<NavigationViewItem>(GoToView);
			SelectionChangedCommand = new RelayCommand<object>(SelectionChanged);
			BackRequestedCommand = new RelayCommand(BackRequested);
		}

		public ICommand SelectionChangedCommand { get; }
		private void SelectionChanged(object selectionArgs)
			=> _logger.LogInformation("this actually worked, type is {type}", selectionArgs.GetType());

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

		public ICommand BackRequestedCommand { get; }
		public void BackRequested()
		{
			_navigator.NavigateBack();
		}

		public void Receive(UseBackNavigationChanged message)
			=> IsBackNavigationEnabled = message.UseBackNavigation;
		public void Dispose()
			=> _messenger.UnregisterAll(this);
	}
}
