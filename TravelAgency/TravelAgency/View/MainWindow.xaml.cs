using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.View;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// //The main logic for this class was taken from the example uploaded on canvas
    public partial class MainWindow : Window
    {
        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);

            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    if (user.Role == Roles.Guide)
                    {
                        GuideMain guideMain = new GuideMain(user);
                        guideMain.Show();
                    }
                    else if (user.Role == Roles.Owner)
                    {
                        OwnerMain ownerMain = new OwnerMain(user);
                        ownerMain.Show();
                    }
                    else if (user.Role == Roles.Guest1)
                    {
                        Guest1Main guest1Main = new Guest1Main(user);
                        guest1Main.Show();
                    }
                    else if (user.Role == Roles.Guest2)
                    {
                        Guest2Main guest2Main = new Guest2Main(user);
                        guest2Main.Show();
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }

        }
    }
}
