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

namespace TravelAgency.WPF.Controls.CustomControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TravelAgency.WPF.Controls.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TravelAgency.WPF.Controls.CustomControls;assembly=TravelAgency.WPF.Controls.CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:OwnerSidebarNavRadioButton/>
    ///
    /// </summary>
    public class OwnerSidebarNavRadioButton : RadioButton
    {

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(OwnerSidebarNavRadioButton), new PropertyMetadata(string.Empty));

        public string KeyboardShortcutText
        {
            get { return (string)GetValue(KeyboardShortcutTextProperty); }
            set { SetValue(KeyboardShortcutTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyboardShortcutText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyboardShortcutTextProperty =
            DependencyProperty.Register("KeyboardShortcutText", typeof(string), typeof(OwnerSidebarNavRadioButton), new PropertyMetadata(string.Empty));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(OwnerSidebarNavRadioButton), new PropertyMetadata(null));



        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(OwnerSidebarNavRadioButton), new PropertyMetadata(Brushes.Transparent));


        static OwnerSidebarNavRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OwnerSidebarNavRadioButton), new FrameworkPropertyMetadata(typeof(OwnerSidebarNavRadioButton)));
        }
    }
}
