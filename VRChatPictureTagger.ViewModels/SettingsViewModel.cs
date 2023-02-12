using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Options;

using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Core.Strings;
using VRChatPictureTagger.Interfaces.Navigation;
using VRChatPictureTagger.Interfaces.Services;

using Windows.Storage;
using Windows.Storage.Pickers;

namespace VRChatPictureTagger.ViewModels
{
	public class SettingsViewModel : ObservableObject, IViewModel
	{
		public string FriendlyName => FriendlyNames.Settings;

		private readonly IOptionsMonitor<MainSettings> _settings;
		readonly ISaveSettingsService _saveSettings;
		readonly IWindowHandleService _windowHandleService;
		private bool _isBackNavigationEnabled;
		private string _selectedSearchPath;
		private int _selectedIndex;
		private object _selectedValue;
		private object _selectedValuePath;
		private List<object> _selectedItems;

		public SettingsViewModel(IOptionsMonitor<MainSettings> settings, ISaveSettingsService saveSettings, IWindowHandleService windowHandleService, IMessenger messenger)
		{
			_settings = settings;
			_saveSettings = saveSettings;
			_windowHandleService = windowHandleService;
			SearchFolders = new ObservableCollection<string>(_settings.CurrentValue.PictureSearchPaths);
			IsBackNavigationEnabled = _settings.CurrentValue.UseBackNavigation;

			AddSearchPathCommand = new RelayCommand(AddSearchPath);
			RemoveSearchPathCommand = new RelayCommand(RemoveSearchPath);
			SaveSettingsCommand = new RelayCommand(SaveSettings);
		}

		public ObservableCollection<string> SearchFolders { get; private set; }

		public string SelectedSearchPath
		{
			get => _selectedSearchPath;
			set => SetProperty(ref _selectedSearchPath, value);
		}

		public List<object> SelectedItems
		{
			get => _selectedItems;
			set => SetProperty(ref _selectedItems, value);
		}


		public bool IsBackNavigationEnabled
		{
			get => _isBackNavigationEnabled;
			set => SetProperty(ref _isBackNavigationEnabled, value);
		}

		public ICommand AddSearchPathCommand { get; set; }
		public ICommand RemoveSearchPathCommand { get; set; }
		public ICommand SaveSettingsCommand { get; }

		public void RemoveSearchPath()
		{
			foreach (var item in SelectedItems)
				if (item is string path)
					SearchFolders.Remove(path);
		}

		public async void AddSearchPath()
		{
			FolderPicker fp = new();
			fp.FileTypeFilter.Add("*");
			_windowHandleService.SetWindowHandleOnPicker(fp);

			StorageFolder newFolder = await fp.PickSingleFolderAsync();
			if (newFolder != null && !SearchFolders.Contains(newFolder.Path))
			{
				SearchFolders.Add(newFolder.Path);
				_settings.CurrentValue.PictureSearchPaths.Add(newFolder.Path);
			}
		}

		private async void SaveSettings()
		{
			MainSettings newSettings = new()
			{
				UseBackNavigation = IsBackNavigationEnabled,
				PictureSearchPaths = SearchFolders.ToList()
			};

			await _saveSettings.SaveMainSettings(newSettings);
		}

		public void NavigatedFrom()
		{


		}

		public void NavigatedTo()
		{
			SearchFolders.Clear();
			foreach (var item in _settings.CurrentValue.PictureSearchPaths)
				SearchFolders.Add(item);
		}
	}
}
