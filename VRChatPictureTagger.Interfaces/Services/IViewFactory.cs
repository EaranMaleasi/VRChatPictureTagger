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
		Page ResolveView<TViewModel>(bool attachDataContext = true) where TViewModel : class, IViewModel;
		Page ResolveView<TViewModel>(TViewModel viewModel, bool attachDataContext = true) where TViewModel : class, IViewModel;
		Page ResolveView(string friendlyViewModelName, bool attachDataContext = true);
		IViewModel ResolveViewModel<TView>(TView view) where TView : class, IView;
		IViewModel ResolveViewModel<TView>() where TView : class, IView;
		IViewModel ResolveViewModel(string friendlyViewModelName);
		Type ResolveViewType<TViewModel>() where TViewModel : class, IViewModel;
		Type ResolveViewType<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
		Type ResolveViewType(string friendlyViewName);
	}
}