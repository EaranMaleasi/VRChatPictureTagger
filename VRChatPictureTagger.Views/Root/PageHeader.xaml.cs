// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VRChatPictureTagger.Views.Root
{
	public sealed partial class PageHeader : UserControl
	{
		public Action CopyLinkAction { get; set; }
		public Action ToggleThemeAction { get; set; }

		public TeachingTip TeachingTip1 => ToggleThemeTeachingTip1;
		public TeachingTip TeachingTip2 => ToggleThemeTeachingTip2;
		public TeachingTip TeachingTip3 => ToggleThemeTeachingTip3;

		public object Title
		{
			get { return GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(object), typeof(PageHeader), new PropertyMetadata(null));


		public string ApiNamespace
		{
			get { return (string)GetValue(ApiNamespaceProperty); }
			set { SetValue(ApiNamespaceProperty, value); }
		}

		public static readonly DependencyProperty ApiNamespaceProperty =
			DependencyProperty.Register("ApiNamespace", typeof(string), typeof(PageHeader), new PropertyMetadata(null));

		public object Subtitle
		{
			get { return GetValue(SubtitleProperty); }
			set { SetValue(SubtitleProperty, value); }
		}

		public static readonly DependencyProperty SubtitleProperty =
			DependencyProperty.Register("Subtitle", typeof(object), typeof(PageHeader), new PropertyMetadata(null));



		public Thickness HeaderPadding
		{
			get { return (Thickness)GetValue(HeaderPaddingProperty); }
			set { SetValue(HeaderPaddingProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BackgroundColorOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HeaderPaddingProperty =
			DependencyProperty.Register("HeaderPadding", typeof(Thickness), typeof(PageHeader), new PropertyMetadata(new Thickness(0)));


		public double BackgroundColorOpacity
		{
			get { return (double)GetValue(BackgroundColorOpacityProperty); }
			set { SetValue(BackgroundColorOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BackgroundColorOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BackgroundColorOpacityProperty =
			DependencyProperty.Register("BackgroundColorOpacity", typeof(double), typeof(PageHeader), new PropertyMetadata(0.0));


		public double AcrylicOpacity
		{
			get { return (double)GetValue(AcrylicOpacityProperty); }
			set { SetValue(AcrylicOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BackgroundColorOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AcrylicOpacityProperty =
			DependencyProperty.Register("AcrylicOpacity", typeof(double), typeof(PageHeader), new PropertyMetadata(0.3));

		public double ShadowOpacity
		{
			get { return (double)GetValue(ShadowOpacityProperty); }
			set { SetValue(ShadowOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BackgroundColorOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ShadowOpacityProperty =
			DependencyProperty.Register("ShadowOpacity", typeof(double), typeof(PageHeader), new PropertyMetadata(0.0));

		public CommandBar TopCommandBar
		{
			get { return topCommandBar; }
		}

		public UIElement TitlePanel
		{
			get { return pageTitle; }
		}

		public PageHeader()
		{
			this.InitializeComponent();
			// this.InitializeDropShadow(ShadowHost, TitleTextBlock.GetAlphaMask());
			this.ResetCopyLinkButton();
		}

		private void OnCopyLinkButtonClick(object sender, RoutedEventArgs e)
		{

		}

		public void OnThemeButtonClick(object sender, RoutedEventArgs e)
		{
			ToggleThemeAction?.Invoke();
		}

		public void ResetCopyLinkButton()
		{
			this.CopyLinkButtonTeachingTip.IsOpen = false;
			this.CopyLinkButton.Label = "Generate Link to Page";
			this.CopyLinkButtonIcon.Symbol = Symbol.Link;
		}

		private void OnCopyDontShowAgainButtonClick(TeachingTip sender, object args)
		{

		}

		private void ToggleThemeTeachingTip2_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
		{

		}

		/// <summary>
		/// This method will be called when a <see cref="ItemPage"/> gets unloaded. 
		/// Put any code in here that should be done when a <see cref="ItemPage"/> gets unloaded.
		/// </summary>
		/// <param name="sender">The sender (the ItemPage)</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> of the ItemPage that was unloaded.</param>
		public void Event_ItemPage_Unloaded(object sender, RoutedEventArgs e)
		{

		}

		// private void InitializeDropShadow(UIElement shadowHost, CompositionBrush shadowTargetBrush)
		// {
		//     Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
		//     Compositor compositor = hostVisual.Compositor;

		//     // Create a drop shadow
		//     var dropShadow = compositor.CreateDropShadow();
		//     dropShadow.Color = ColorHelper.FromArgb(102, 0, 0, 0);
		//     dropShadow.BlurRadius = 4.0f;
		//     // Associate the shape of the shadow with the shape of the target element
		//     dropShadow.Mask = shadowTargetBrush;

		//     // Create a Visual to hold the shadow
		//     var shadowVisual = compositor.CreateSpriteVisual();
		//     shadowVisual.Shadow = dropShadow;

		//     // Add the shadow as a child of the host in the visual tree
		//     ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);

		//     // Make sure size of shadow host and shadow visual always stay in sync
		//     var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
		//     bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);

		//     shadowVisual.StartAnimation("Size", bindSizeAnimation);
		// }
	}
}
