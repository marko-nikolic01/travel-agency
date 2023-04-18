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
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourGuideRating.xaml
    /// </summary>
    public partial class TourRatingWindow : Window
    {
        private int currentGuestId;
        private TourOccurrence tourOccurrence;
        private TourRatingRepository tourRatingRepository;
        public TourRatingWindow(TourOccurrence selectedTourOccurrence, int currentGuestId)
        {
            InitializeComponent();
            tourRatingRepository = new TourRatingRepository();
            this.currentGuestId = currentGuestId;
            descriptionLabel.Content = selectedTourOccurrence.Tour.Name+ " in " +selectedTourOccurrence.Tour.Location.Country+
                ", "+selectedTourOccurrence.Tour.Location.City+ ". "+ selectedTourOccurrence.Tour.Description;
            tourOccurrence= selectedTourOccurrence;
        }
        private void AddUrl_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(urlText.Text))
            {
                return;
            }
            urlsList.Items.Add(urlText.Text);
            urlText.Clear();
            urlText.Focus();
        }
            private void SubmitRating_Click(object sender, RoutedEventArgs e)
            {
            int guideKnowledge;
            int guideLanguage;
            int interesting;
            string additionalComment;
            guideKnowledge = int.Parse(knowledgeCb.Text);
            guideLanguage = int.Parse(languageCb.Text);
            interesting = int.Parse(interestingCb.Text);
            additionalComment = commentTb.Text;
            TourRating tourRating = new TourRating(currentGuestId, tourOccurrence.Id, guideKnowledge, guideLanguage, interesting, additionalComment, null);
            TourRating savedTourRating = tourRatingRepository.Save(tourRating);
            savePhotos(savedTourRating.Id);
            Close();
        }

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if(urlsList.SelectedIndex != -1)
            {
                urlsList.Items.RemoveAt(urlsList.SelectedIndex);
            }
        }

        private void PreviewImage_Click(object sender, RoutedEventArgs e)
        {

        }
        private void savePhotos(int id)
        {
            TourRatingPhotoRepository tourRatingPhotoRepository = new TourRatingPhotoRepository();
            for (int i = 0; i < urlsList.Items.Count; i++)
            {
                TourRatingPhoto photo = new TourRatingPhoto();
                photo.Link = urlsList.Items.GetItemAt(i).ToString();
                photo.TourRatingId = id;
                tourRatingPhotoRepository.Save(photo);
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
