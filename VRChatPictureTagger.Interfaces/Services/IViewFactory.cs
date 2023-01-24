using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Interfaces.Navigation;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface IViewFactory
	{
		bool IsViewModelRegistered(string friendlyName);
		bool IsViewRegistered(string friendlyName);
		void Register<TViewModel, TView>() where TViewModel : ObservableObject, IViewModel where TView : Page, IView;
		/// <summary>
		/// Resolves the View from the <typeparamref name="TViewModel"/> type and optionally attaches the corresponding ViewModel to the DataContext
		/// </summary>
		/// <typeparam name="TViewModel">The type of the ViewModel</typeparam>
		/// <param name="attachDataContext">Resolve and attach the ViewModel to the DataContext of the View</param>
		/// <returns>A Page with optionally attached DataContext</returns>
		Page ResolveView<TViewModel>(bool attachDataContext = true) where TViewModel : class, IViewModel;
		Page ResolveView<TViewModel>(TViewModel viewModel, bool attachDataContext = true) where TViewModel : class, IViewModel;
		/// <summary>
		/// Resolves the View from the <paramref name="friendlyName"/> and optionally attaches the corresponding ViewModel to the DataContext
		/// </summary>
		/// <param name="friendlyName">The name of the View/ViewModel to resolve</param>
		/// <param name="attachDataContext">Resolve and attach the ViewModel to the DataContext of the View</param>
		/// <returns>A Page with optionally attached DataContext</returns>
		Page ResolveView(string friendlyViewModelName, bool attachDataContext = true);
		IViewModel ResolveViewModel<TView>(TView view) where TView : class, IView;
		IViewModel ResolveViewModel<TView>() where TView : class, IView;
		IViewModel ResolveViewModel(string friendlyViewModelName);
		Type ResolveViewType<TViewModel>() where TViewModel : class, IViewModel;
		Type ResolveViewType<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
		Type ResolveViewType(string friendlyViewName);
	}
}