using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace VRChatPictureTagger.Views.Behaviours
{
	public class IsSettingsItemBehaviour : Behavior<NavigationView>
	{

		public static readonly DependencyProperty IsSettingsItemProperty = DependencyProperty.Register(
																		   "IsSettingsItem",
																		   typeof(bool),
																		   typeof(IsSettingsItemBehaviour),
																		   new PropertyMetadata(false));
		public bool IsSettingsItem
		{
			get => (bool)GetValue(IsSettingsItemProperty);
			set => SetValue(IsSettingsItemProperty, value);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (AssociatedObject != null)
				AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
		}


		private void AssociatedObject_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			IsSettingsItem = args.IsSettingsSelected;
		}
	}
}
