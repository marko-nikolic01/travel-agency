using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TravelAgency.WPF.Controls
{
    /// <summary>
    /// Interaction logic for AccommodationsNavigationButton.xaml
    /// </summary>
    public partial class AccommodationsNavigationButton : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AccommodationsNavigationButton), new PropertyMetadata(string.Empty));

        public string KeyboardShortcutText
        {
            get { return (string)GetValue(KeyboardShortcutTextProperty); }
            set { SetValue(KeyboardShortcutTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyboardShortcutText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyboardShortcutTextProperty =
            DependencyProperty.Register("KeyboardShortcutText", typeof(string), typeof(AccommodationsNavigationButton), new PropertyMetadata(string.Empty));

        public RoutedUICommand Command
        {
            get { return (RoutedUICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(RoutedUICommand), typeof(AccommodationsNavigationButton), new PropertyMetadata(null));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(AccommodationsNavigationButton), new PropertyMetadata(null));

        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(AccommodationsNavigationButton), new PropertyMetadata(Brushes.Transparent));

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(AccommodationsNavigationButton), new PropertyMetadata(Brushes.Black));



        public AccommodationsNavigationButton()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler Click;

        void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }
    }
}
