using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.ViewModel
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private List<string>? imageUrls;
        private int index;
        public TourPhotosViewModel(List<Photo>? photos) 
        {
            index = 0;
            imageUrls = new List<string>();
            foreach(Photo p in photos)
            {
                imageUrls.Add(p.Link);
            }
            ImageUrl = imageUrls[0];
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
