using System;

using CommunityToolkit.Diagnostics;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace VRChatPictureTagger.Core.Converters
{
	public class BoolToNavigationViewBackButtonVisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (targetType != typeof(NavigationViewBackButtonVisible))
				ThrowHelper.ThrowArgumentException(nameof(targetType), $"Target type must be {nameof(NavigationViewBackButtonVisible)}");

			if (value is bool visible)
				return visible ? NavigationViewBackButtonVisible.Visible : NavigationViewBackButtonVisible.Collapsed;
			return NavigationViewBackButtonVisible.Auto;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (targetType != typeof(bool?))
				ThrowHelper.ThrowArgumentException(nameof(targetType), $"Target type must be {nameof(Boolean)}");

			if (value is NavigationViewBackButtonVisible visibility)
			{
				return visibility switch
				{
					NavigationViewBackButtonVisible.Collapsed => false,
					NavigationViewBackButtonVisible.Visible => true,
					NavigationViewBackButtonVisible.Auto => null,
					_ => false
				};
			}
			return false;
		}
	}
}
