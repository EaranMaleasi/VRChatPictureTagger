using System.Collections.Generic;
using System.Linq;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace VRChatPictureTagger.Views.Behaviours
{
	public class SelectedItemsBehaviour : Behavior<ListBox>
	{
		public static readonly DependencyProperty ReadonlySelectedItemsProperty = DependencyProperty.Register(
																				  nameof(ReadonlySelectedItems),
																				  typeof(List<object>),
																				  typeof(SelectedItemsBehaviour),
																				  new PropertyMetadata(new List<object>()));

		public List<object> ReadonlySelectedItems
		{
			get => (List<object>)GetValue(ReadonlySelectedItemsProperty);
			set => SetValue(ReadonlySelectedItemsProperty, value);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged; ;
		}

		private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ReadonlySelectedItems = AssociatedObject.SelectedItems.ToList();
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (AssociatedObject != null)
				AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
		}
	}
}
