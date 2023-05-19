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
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Injector;
using TravelAgency.Repositories;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// //The main logic for this class was taken from the example uploaded on canvas
    public partial class MainWindow : Window
    {
        private readonly IUserRepository _repository;
        private UserService _userService;

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
            _repository = Injector.Injector.CreateInstance<IUserRepository>();
            _userService = new UserService();
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
                        GuideView guideView = new GuideView(user.Id);
                        guideView.Show();
                        //GuideMain guideMain = new GuideMain(user);
                        //guideMain.Show();
                    }
                    else if (user.Role == Roles.Owner)
                    {
                        OwnerMain ownerMain = new OwnerMain(user);
                        ownerMain.Show();
                    }
                    else if (user.Role == Roles.Guest1)
                    {
                        Guest1MainView guest1MainView = new Guest1MainView(user);
                        guest1MainView.Show();
                    }
                    else if (user.Role == Roles.Guest2)
                    {
                        ProgramStatusRepository repository = new ProgramStatusRepository();
                        if (repository.GetProgramStatus().IsFirstTimeOpening)
                        {
                            IntroductionWizardWindow introduction = new IntroductionWizardWindow(user.Id);
                            introduction.Show();
                        }
                        else
                        {
                            Guest2MainWindow guest2Main = new Guest2MainWindow(user.Id);
                            guest2Main.Show();
                        }
                    }
                    _userService.LogInUser(user);
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
