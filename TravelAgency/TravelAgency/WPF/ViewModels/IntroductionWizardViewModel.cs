using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Commands;
using TravelAgency.Repositories;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class IntroductionWizardViewModel : INotifyPropertyChanged
    {
        private int i;
        private string backButtonVisibility;
        private string nextButtonText;
        private string text;
        private string imageSource;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Text
        {
            get => text;
            set { if (value != text) { text = value; OnPropertyChanged(); } }
        }
        public string ImageSource
        {
            get => imageSource;
            set { if (value != imageSource) { imageSource = value; OnPropertyChanged(); } }
        }
        public string BackButtonVisibility
        {
            get => backButtonVisibility;
            set { if (value != backButtonVisibility) { backButtonVisibility = value; OnPropertyChanged(); } }
        }
        public string NextButtonText
        {
            get => nextButtonText;
            set { if (value != nextButtonText) { nextButtonText = value; OnPropertyChanged(); } }
        }
        public ButtonCommandNoParameter NextCommand { get; set; }
        public ButtonCommandNoParameter BackCommand { get; set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int guestId;
        public IntroductionWizardViewModel(int id) 
        {
            guestId = id;
            NextCommand = new ButtonCommandNoParameter(Next);
            BackCommand = new ButtonCommandNoParameter(Back);
            NextButtonText = "Next";
            i = 0;
            changeData();
        }
        private void Next()
        {
            if (i<6)
            {
                i++;
                changeData();
            }
            else
            {
                ProgramStatusRepository repository = new ProgramStatusRepository();
                repository.SetProgramStatus();
                Guest2MainWindow guest2Main = new Guest2MainWindow(guestId);
                guest2Main.Show();
            }
        }
        private void Back()
        {
            if (i > 0)
            {
                i--;
                changeData();
            }
        }
        private void changeData()
        {
            if(i == 0) 
            {
                BackButtonVisibility = "Hidden";
                Text = "This is opening page, your profile. Here you can see your personal information" +
                    "\n at top is navigation menu which allows you to navigate through the whole application.";
                ImageSource = "../../Resources/Images/Picture1.PNG";
            }
            else if (i == 1)
            {
                BackButtonVisibility = "Visible";
                Text = "This is offered tours page. It allows you to choose the tour" +
                    "\n and make a reservation for it";
                ImageSource = "../../Resources/Images/Picture2.PNG";
            }
            else if (i == 2)
            {
                Text = "This is form where you enter data for reservation. Number of guests" +
                    "\n and who are the guests";
                ImageSource = "../../Resources/Images/Picture3.PNG";
            }
            else if (i == 3)
            {
                Text = "In this page you can see if you have active tour and follow it. Also you can see" +
                    "\n your status on that tour. Below that are all tours that you have been on." +
                    "\n you can rate tours on which you've been on.";
                ImageSource = "../../Resources/Images/Picture4.PNG";
            }
            else if (i == 4)
            {
                Text = "Tour rating form";
                ImageSource = "../../Resources/Images/Picture5.PNG";
            }
            else if (i == 5)
            {
                Text = "In this page there are all requests for tour that you've made. If someone" +
                    "\n has accepted your request new tour will be made and it will be shown which" +
                    "\n  date is given for that tour.";
                ImageSource = "../../Resources/Images/Picture6.PNG";
                NextButtonText = "Next";
            }
            else if (i == 6)
            {
                Text = "This is form for creating tour request";
                ImageSource = "../../Resources/Images/Picture7.PNG";
                NextButtonText = "Finish";
            }
        }
    }
}
