using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

		private readonly IOptions<MainSettings> _settings;
		readonly ISaveSettingsService _saveSettings;
		readonly IWindowHandleService _windowHandleService;
		private bool _dontSearchVrcFolder;
		private string _selectedSearchPath;

		public SettingsViewModel(IOptions<MainSettings> settings, ISaveSettingsService saveSettings, IWindowHandleService windowHandleService)
		{
			_settings = settings;
			_saveSettings = saveSettings;
			_windowHandleService = windowHandleService;
			SearchFolders = new ObservableCollection<string>(_settings.Value.PictureSearchPaths);

			AddSearchPathCommand = new RelayCommand(AddSearchPath);
			RemoveSearchPathCommand = new RelayCommand(RemoveSearchPath);
		}

		public ObservableCollection<string> SearchFolders { get; private set; }

		public string SelectedSearchPath
		{
			get => _selectedSearchPath;
			set => SetProperty(ref _selectedSearchPath, value);
		}

		public bool DontSearchVrcFolder
		{
			get => _dontSearchVrcFolder;
			set => SetProperty(ref _dontSearchVrcFolder, value);
		}

		public ICommand AddSearchPathCommand { get; set; }
		public ICommand RemoveSearchPathCommand { get; set; }

		public void RemoveSearchPath()
		{
			SearchFolders.Remove(_selectedSearchPath);
			_settings.Value.PictureSearchPaths.Remove(_selectedSearchPath);
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
