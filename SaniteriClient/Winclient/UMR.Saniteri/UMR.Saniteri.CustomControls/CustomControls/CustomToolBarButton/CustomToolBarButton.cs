using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls;

namespace UMR.Saniteri.CustomControls
{
    public class CustomToolBarButton : System.Windows.Controls.Button
    {
        #region " Shared Declarations "

        public static DependencyProperty ButtonLayoutProperty = DependencyProperty.Register("ButtonLayout", typeof(Orientation), typeof(CustomToolBarButton), new PropertyMetadata(Orientation.Horizontal));
        public static DependencyProperty ButtonPressedBackgroundProperty = DependencyProperty.Register("ButtonPressedBackground", typeof(Brush), typeof(CustomToolBarButton));
        public static DependencyProperty ButtonPressedBorderProperty = DependencyProperty.Register("ButtonPressedBorder", typeof(Brush), typeof(CustomToolBarButton));
        public static DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText", typeof(string), typeof(CustomToolBarButton));
        public static DependencyProperty DisabledButtonImageProperty = DependencyProperty.Register("DisabledButtonImage", typeof(string), typeof(CustomToolBarButton));
        public static DependencyProperty EnabledButtonImageProperty = DependencyProperty.Register("EnabledButtonImage", typeof(string), typeof(CustomToolBarButton));
        public static DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(CustomToolBarButton));
        public static DependencyProperty MouseOverBorderProperty = DependencyProperty.Register("MouseOverBorder", typeof(Brush), typeof(CustomToolBarButton), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public static DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(CustomToolBarButton));
        public static DependencyProperty ShowButtonImageProperty = DependencyProperty.Register("ShowButtonImage", typeof(bool), typeof(CustomToolBarButton), new PropertyMetadata(true));
        public static DependencyProperty ShowButtonTextProperty = DependencyProperty.Register("ShowButtonText", typeof(bool), typeof(CustomToolBarButton), new PropertyMetadata(false));
        #endregion
        #region " Properties "

        [Category("Custom"), Description("This sets the position of the text in relation to the button image.")]
        public Orientation ButtonLayout
        {
            get { return (Orientation)GetValue(ButtonLayoutProperty); }
            set { SetValue(ButtonLayoutProperty, value); }
        }

        [Category("Custom"), Description("Button pressed background brush.")]
        public Brush ButtonPressedBackground
        {
            get { return (Brush)GetValue(ButtonPressedBackgroundProperty); }
            set { SetValue(ButtonPressedBackgroundProperty, value); }
        }

        [Category("Custom"), Description("Button pressed border brush.")]
        public Brush ButtonPressedBorder
        {
            get { return (Brush)GetValue(ButtonPressedBorderProperty); }
            set { SetValue(ButtonPressedBorderProperty, value); }
        }

        [Category("Custom"), Description("Text for the button.")]
        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        /// <summary>
        /// I did this since we have derived from button, but don't use the
        /// content property like other buttons. The control template for
        /// this control is the content of this button. Doing this, just
        /// prevents this property from showing up in the GUI designers.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public new object Content
        {
            get { return base.Content; }
            set { base.Content = value; }
        }

        [Category("Custom"), Description("Image to display when the button is disabled.")]
        public string DisabledButtonImage
        {
            get { return (string)GetValue(DisabledButtonImageProperty); }
            set { SetValue(DisabledButtonImageProperty, value); }
        }

        [Category("Custom"), Description("Image to display when the button is enabled.")]
        public string EnabledButtonImage
        {
            get { return (string)GetValue(EnabledButtonImageProperty); }
            set { SetValue(EnabledButtonImageProperty, value); }
        }

        [Category("Custom"), Description("Mouse over background brush.")]
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        [Category("Custom"), Description("Mouse over border brush.")]
        public Brush MouseOverBorder
        {
            get { return (Brush)GetValue(MouseOverBorderProperty); }
            set { SetValue(MouseOverBorderProperty, value); }
        }

        [Category("Custom"), Description("Mouse over foreground text brush.")]
        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        [Category("Custom"), Description("Display the image on the button.")]
        public bool ShowButtonImage
        {
            get { return (bool)GetValue(ShowButtonImageProperty); }
            set { SetValue(ShowButtonImageProperty, value); }
        }

        [Category("Custom"), Description("Display the text on the button.")]
        public bool ShowButtonText
        {
            get { return (bool)GetValue(ShowButtonTextProperty); }
            set { SetValue(ShowButtonTextProperty, value); }
        }
        #endregion
        #region " Constructor "

        static CustomToolBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomToolBarButton), new FrameworkPropertyMetadata(typeof(CustomToolBarButton)));

        }
        #endregion

    }
}
