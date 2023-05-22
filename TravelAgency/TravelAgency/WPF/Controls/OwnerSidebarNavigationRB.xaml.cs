using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.WPF.Controls.CustomControls;

namespace TravelAgency.WPF.Controls
{
    /// <summary>
    /// Interaction logic for OwnerSidebarNavigationRadioButton.xaml
    /// </summary>
    public partial class OwnerSidebarNavigationRadioButton : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(OwnerSidebarNavigationRadioButton), new PropertyMetadata(string.Empty));

        public string KeyboardShortcutText
        {
            get { return (string)GetValue(KeyboardShortcutTextProperty); }
            set { SetValue(KeyboardShortcutTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyboardShortcutText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyboardShortcutTextProperty =
            DependencyProperty.Register("KeyboardShortcutText", typeof(string), typeof(OwnerSidebarNavigationRadioButton), new PropertyMetadata(string.Empty));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(OwnerSidebarNavigationRadioButton), new PropertyMetadata(null));


        public RoutedUICommand Command
        {
            get { return (RoutedUICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(RoutedUICommand), typeof(OwnerSidebarNavigationRadioButton), new PropertyMetadata(null));


        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(OwnerSidebarNavigationRadioButton), new PropertyMetadata(Brushes.White));

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(OwnerSidebarNavigationRadioButton), new PropertyMetadata(Brushes.Black));




        public OwnerSidebarNavigationRadioButton()
        {
            InitializeComponent();
        }
    }
}
