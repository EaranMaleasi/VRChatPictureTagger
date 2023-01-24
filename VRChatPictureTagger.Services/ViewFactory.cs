using System;
using System.Collections.Generic;

using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Interfaces.Navigation;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services
{
	internal class ViewFactory : IViewFactory
	{
		private Dictionary<string, Type> _mapViewToViewModel = new();
		private Dictionary<string, Type> _mapViewModelToView = new();

		private Dictionary<string, Type> _mapFriendlyToView = new();
		private Dictionary<string, Type> _mapFriendlyToViewModel = new();

		private readonly IServiceProvider _services;
		private readonly ILogger<ViewFactory> _logger;

		public ViewFactory(IServiceProvider services, ILogger<ViewFactory> logger)
		{
			_services = services;
			_logger = logger;
		}

		public void Register<TViewModel, TView>() where TViewModel : ObservableObject, IViewModel where TView : Page, IView
		{
			var viewModel = _services.GetService<TViewModel>();
			var view = _services.GetService<TView>();

			Type vmType = viewModel.GetType();
			Type vType = view.GetType();

			string vmName = vmType.Name;
			string vName = vType.Name;

			string vmFriendly = viewModel.FriendlyName;
			string vFriendly = view.FriendlyName;

			Guard.IsNotNullOrWhiteSpace(vmFriendly);
			Guard.IsNotNullOrWhiteSpace(vFriendly);
			Guard.IsEqualTo(vmFriendly, vFriendly);

			if (_mapFriendlyToView.ContainsKey(vFriendly))
				ThrowHelper.ThrowArgumentException(nameof(vFriendly), "FriendlyName of View already Registered. FriendlyName must be unique!");
			if (_mapFriendlyToViewModel.ContainsKey(vmFriendly))
				ThrowHelper.ThrowArgumentException(nameof(vFriendly), "FriendlyName of ViewModel already Registered. FriendlyName must be unique!");
			if (_mapViewModelToView.ContainsKey(vmName))
				ThrowHelper.ThrowArgumentException(nameof(vmName), "ViewModel already Registered. Every ViewModel can only be registered once!");
			if (_mapViewToViewModel.ContainsKey(vName))
				ThrowHelper.ThrowArgumentException(nameof(vFriendly), "View already Registered. Every View can only be registered once!");


			_mapFriendlyToView[vFriendly] = vType;
			_mapFriendlyToViewModel[vmFriendly] = vmType;

			_mapViewModelToView[vmName] = vType;
			_mapViewToViewModel[vName] = vmType;
		}

		/// <summary>
		/// Resolves the View from the <typeparamref name="TViewModel"/> type and optionally attaches the corresponding ViewModel to the DataContext
		/// </summary>
		/// <typeparam name="TViewModel">The type of the ViewModel</typeparam>
		/// <param name="attachDataContext">Resolve and attach the ViewModel to the DataContext of the View</param>
		/// <returns>A Page with optionally attached DataContext</returns>
		public Page ResolveView<TViewModel>(bool attachDataContext = true) where TViewModel : class, IViewModel
		{
			try
			{
				Page view = _services.GetService(_mapViewModelToView[nameof(TViewModel)]) as Page;
				if (attachDataContext)
					view.DataContext = _services.GetService<TViewModel>();

				return view;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Exception occured while trying to resolve View for ViewModel {ViewModel}", typeof(TViewModel).Name);
				throw;
			}
		}
		public Page ResolveView<TViewModel>(TViewModel viewModel, bool attachDataContext = true) where TViewModel : class, IViewModel
		{
			Guard.IsNotNull(viewModel, nameof(viewModel));
			Guard.IsNotNullOrWhiteSpace(viewModel.FriendlyName, nameof(viewModel.FriendlyName));

			if (!_mapFriendlyToView.TryGetValue(viewModel.FriendlyName, out Type ViewType))
				ThrowHelper.ThrowArgumentException(nameof(viewModel), $"No View registered for FriendlyName {viewModel.FriendlyName}!");

			try
			{
				Page view = _services.GetService(ViewType) as Page;
				if (attachDataContext)
					view.DataContext = viewModel;

				return view;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Exception occured while trying to resolve View for ViewModel {ViewModel}", nameof(TViewModel));
				throw;
			}
		}
		/// <summary>
		/// Resolves the View from the <paramref name="friendlyName"/> and optionally attaches the corresponding ViewModel to the DataContext
		/// </summary>
		/// <param name="friendlyName">The name of the View/ViewModel to resolve</param>
		/// <param name="attachDataContext">Resolve and attach the ViewModel to the DataContext of the View</param>
		/// <returns>A Page with optionally attached DataContext</returns>
		public Page ResolveView(string friendlyName, bool attachDataContext = true)
		{
			Guard.IsNotNullOrWhiteSpace(friendlyName);

			if (!_mapFriendlyToView.TryGetValue(friendlyName, out Type viewType))
				ThrowHelper.ThrowArgumentException(nameof(friendlyName), $"FriendlName {friendlyName} not registered for Views");

			try
			{
				Page view = _services.GetService(viewType) as Page;
				if (attachDataContext)
					view.DataContext = view.DataContext = _services.GetService(_mapFriendlyToViewModel[friendlyName]);

				return view;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Exception occured while trying to resolve FriendlyName {friendlyName} to View.", friendlyName);
				throw;
			}
		}


		public Type ResolveViewType<TViewModel>() where TViewModel : class, IViewModel
		{
			string viewModelName = typeof(TViewModel).Name;

			if (!_mapViewModelToView.TryGetValue(viewModelName, out Type viewType))
				ThrowHelper.ThrowArgumentException($"ViewModel {viewModelName} not registered!");

			return viewType;
		}
		public Type ResolveViewType<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel
		{
			Guard.IsNotNull(viewModel, nameof(viewModel));
			Guard.IsNotNullOrWhiteSpace(viewModel.FriendlyName, nameof(viewModel.FriendlyName));

			if (!_mapFriendlyToView.TryGetValue(viewModel.FriendlyName, out Type ViewType))
				ThrowHelper.ThrowArgumentException(nameof(viewModel), $"No View registered for FriendlyName {viewModel.FriendlyName}!");

			return ViewType;
		}
		public Type ResolveViewType(string friendlyName)
		{
			Guard.IsNotNullOrWhiteSpace(friendlyName);

			if (!_mapFriendlyToView.TryGetValue(friendlyName, out Type viewType))
				ThrowHelper.ThrowArgumentException(nameof(friendlyName), $"FriendlName {friendlyName} not registered for Views");

			return viewType;
		}


		public IViewModel ResolveViewModel<TView>(TView view) where TView : class, IView
		{
			Guard.IsNotNull(view, nameof(view));
			Guard.IsNotNullOrWhiteSpace(view.FriendlyName, nameof(view.FriendlyName));

			if (!_mapFriendlyToViewModel.TryGetValue(view.FriendlyName, out Type ViewModelType))
				ThrowHelper.ThrowArgumentException(nameof(view), $"No ViewModel registered for FriendlyName {view.FriendlyName}!");

			try
			{
				return _services.GetService(ViewModelType) as IViewModel;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Exception occured while trying to resolve ViewModel for View {friendlyName}", view.FriendlyName);
				throw;
			}
		}
		public IViewModel ResolveViewModel<TView>() where TView : class, IView
		{
			string viewName = typeof(TView).Name;

			if (!_mapViewToViewModel.TryGetValue(viewName, out Type viewModelType))
				ThrowHelper.ThrowArgumentException($"View {viewName} not registered!");

			try
			{
				return _services.GetService(viewModelType) as IViewModel;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Exception occured while trying to resolve ViewModel for View {View}", nameof(TView));
				throw;
			}
		}
		public IViewModel ResolveViewModel(string friendlyName)
		{
			Guard.IsNotNullOrWhiteSpace(friendlyName);

			if (!_mapFriendlyToViewModel.TryGetValue(friendlyName, out Type viewModelType))
				ThrowHelper.ThrowArgumentException(nameof(friendlyName), $"Friendly name {friendlyName} not registered for ViewModels");

			return _services.GetService(viewModelType) as IViewModel;
		}

		public bool IsViewRegistered(string friendlyName)
			=> _mapFriendlyToView.ContainsKey(friendlyName);
		public bool IsViewModelRegistered(string friendlyName)
			=> _mapFriendlyToViewModel.ContainsKey(friendlyName);
	}
}
