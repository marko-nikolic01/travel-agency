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
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerAddCommentView.xaml
    /// </summary>
    public partial class OwnerAddCommentView : Page
    {
        public OwnerAddCommentViewModel ViewModel { get; set; }

        public OwnerAddCommentView(OwnerAddCommentViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigateBackCommand.Execute();
        }

        private void AddComment_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddCommentCommand.Execute();
        }
    }
}
