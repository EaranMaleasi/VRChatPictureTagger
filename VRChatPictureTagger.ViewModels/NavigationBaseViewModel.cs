using System;
using System.Collections.Generic;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.ViewModels
{
	public class NavigationBaseViewModel : ObservableObject, IDisposable
	{
		private List<string> _menuEntries;
		private object _selectedItem;
		private bool _isSettingsItem;
		private List<string> _menuFooterEntries;
		private bool? _isBackNavigationEnabled;
		private bool _isCurrentlyNavigatingBack;
		private readonly INavigator _navigator;
		private readonly ILogger<NavigationBaseViewModel> _logger;
		private readonly IOptionsMonitor<MainSettings> _options;
		readonly IOptions<WindowAndNavigationOptions> _windowOptions;
		private readonly IDisposable _optionsListener;

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
				if (SetProperty(ref _selectedItem, value))
					if (!_isCurrentlyNavigatingBack && value != null)
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
			IOptionsMonitor<MainSettings> options,
			IOptions<WindowAndNavigationOptions> windowOptions,
			INavigator navigator,
			IMessenger messenger)
		{
			MenuEntries = new List<string>() { "Menu 1", "Menu 2" };
			MenuFooterEntries = new List<string>() { "Logs" };

			_navigator = navigator;
			_logger = logger;
			_options = options;
			_windowOptions = windowOptions;
			_optionsListener = _options.OnChange(SettingsChanged);
			IsBackNavigationEnabled = _options.CurrentValue.UseBackNavigation;

			GoToViewCommand = new RelayCommand<NavigationViewItem>(GoToView);
			SelectionChangedCommand = new RelayCommand<object>(SelectionChanged);
			BackRequestedCommand = new RelayCommand(BackRequested);
		}

		public ICommand BackRequestedCommand { get; }
		public ICommand GoToViewCommand { get; }
		public ICommand SelectionChangedCommand { get; }

		private void SelectionChanged(object selectionArgs)
			=> _logger.LogInformation("this actually worked, type is {type}", selectionArgs.GetType());


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


		public void BackRequested()
		{
			_isCurrentlyNavigatingBack = true;

			_navigator.NavigateBack();
			SelectedItem = _navigator.CurrentView?.FriendlyName;

			_isCurrentlyNavigatingBack = false;
		}

		public void SettingsChanged(MainSettings settings)
			=> _windowOptions.Value.UIDispatcher.TryEnqueue(() => IsBackNavigationEnabled = _options.CurrentValue.UseBackNavigation);

		public void Dispose()
			=> _optionsListener?.Dispose();
	}
}
