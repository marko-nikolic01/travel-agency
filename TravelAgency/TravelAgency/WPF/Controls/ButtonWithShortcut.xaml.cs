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
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ButtonWithShortcut.xaml
    /// </summary>
    public partial class ButtonWithShortcut : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ButtonWithShortcut), new PropertyMetadata(string.Empty));


        public string KeyboardShortcutText
        {
            get { return (string)GetValue(KeyboardShortcutTextProperty); }
            set { SetValue(KeyboardShortcutTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyboardShortcutText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyboardShortcutTextProperty =
            DependencyProperty.Register("KeyboardShortcutText", typeof(string), typeof(ButtonWithShortcut), new PropertyMetadata(string.Empty));


        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonWithShortcut), new PropertyMetadata(null));


        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(ButtonWithShortcut), new PropertyMetadata(Brushes.White));

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(ButtonWithShortcut), new PropertyMetadata(Brushes.Black));


        public ButtonWithShortcut()
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
