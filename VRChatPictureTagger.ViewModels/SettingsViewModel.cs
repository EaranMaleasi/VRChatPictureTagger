using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Options;

using VRChatPictureTagger.Core.Messages;
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

		private readonly IOptions<MainSettings> _settings;
		readonly ISaveSettingsService _saveSettings;
		readonly IWindowHandleService _windowHandleService;
		readonly IMessenger _messenger;
		private bool _isBackNavigationEnabled;
		private string _selectedSearchPath;

		public SettingsViewModel(IOptions<MainSettings> settings, ISaveSettingsService saveSettings, IWindowHandleService windowHandleService, IMessenger messenger)
		{
			_settings = settings;
			_saveSettings = saveSettings;
			_windowHandleService = windowHandleService;
			_messenger = messenger;
			SearchFolders = new ObservableCollection<string>(_settings.Value.PictureSearchPaths);
			IsBackNavigationEnabled = _settings.Value.UseBackNavigation;

			AddSearchPathCommand = new RelayCommand(AddSearchPath);
			RemoveSearchPathCommand = new RelayCommand(RemoveSearchPath);
		}

		public ObservableCollection<string> SearchFolders { get; private set; }

		public string SelectedSearchPath
		{
			get => _selectedSearchPath;
			set => SetProperty(ref _selectedSearchPath, value);
		}

		public bool IsBackNavigationEnabled
		{
			get => _isBackNavigationEnabled;
			set
			{
				if (SetProperty(ref _isBackNavigationEnabled, value))
					_messenger.Send(new UseBackNavigationChanged { UseBackNavigation = value });
			}
		}

		public ICommand AddSearchPathCommand { get; set; }
		public ICommand RemoveSearchPathCommand { get; set; }

		public void RemoveSearchPath()
		{
			SearchFolders.Remove(_selectedSearchPath);
			_settings.Value.PictureSearchPaths.Remove(_selectedSearchPath);
			_messenger.Send<SettingsChangedMessage>();
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
				_settings.Value.PictureSearchPaths.Add(newFolder.Path);
			}
		}

		public async void NavigatedFrom()
		{
			await _saveSettings.SavePathSettings(_settings.Value);

		}

		public void NavigatedTo()
		{
			SearchFolders.Clear();
			foreach (var item in _settings.Value.PictureSearchPaths)
				SearchFolders.Add(item);
		}
	}
}
