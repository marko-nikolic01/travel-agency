using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;

namespace TravelAgency.WPF.ViewModels
{

    public class TourPhotosViewModel : INotifyPropertyChanged
    {
        private string imageUrl;
        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                if (value != imageUrl)
                {
                    imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        public ButtonCommandNoParameter PreviousPhotoCommand { get; set; }
        public ButtonCommandNoParameter NextPhotoCommand { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private List<string>? imageUrls;
        private int index;

        public TourPhotosViewModel(List<Photo>? photos)
        {
            imageUrls = new List<string>();
            foreach (Photo photo in photos)
            {
                imageUrls.Add(photo.Link);
            }
            Initialize();
        }
        public TourPhotosViewModel(List<string>? photoLinks)
        {
            imageUrls = new List<string>();
            foreach (string link in photoLinks)
            {
                imageUrls.Add(link);
            }
            Initialize();
        }
        private void Initialize()
        {
            index = 0;
            ImageUrl = imageUrls[0];
            PreviousPhotoCommand = new ButtonCommandNoParameter(ShowPreviousPhoto);
            NextPhotoCommand = new ButtonCommandNoParameter(ShowNextPhoto);
        }
        public void ShowNextPhoto()
        {
            if (index != imageUrls.Count - 1)
                index++;
            else
                index = 0;
            ImageUrl = imageUrls[index];
        }

        public void ShowPreviousPhoto()
        {
            if (index != 0)
                index--;
            else
                index = imageUrls.Count - 1;
            ImageUrl = imageUrls[index];
        }
    }
}
