using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.Controls
{
    /// <summary>
    /// Interaction logic for PhotosViewer.xaml
    /// </summary>
    public partial class PhotosViewer : UserControl
    {
        //public static readonly RoutedCommand PreviousPhotoCommand = new RoutedCommand();
        //public static readonly RoutedCommand NextPhotoCommand = new RoutedCommand();

        public ObservableCollection<AccommodationPhoto> Images
        {
            get { return (ObservableCollection<AccommodationPhoto>)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Images.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(ObservableCollection<AccommodationPhoto>), typeof(PhotosViewer), new PropertyMetadata(null, ImagesPropertyChanged));


        public ImageSource CurrentPhoto
        {
            get { return (ImageSource)GetValue(CurrentPhotoProperty); }
            set { SetValue(CurrentPhotoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPhoto.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPhotoProperty =
            DependencyProperty.Register("CurrentPhoto", typeof(ImageSource), typeof(PhotosViewer), new PropertyMetadata(null));

        public AccommodationPhoto SelectedItem
        {
            get { return (AccommodationPhoto)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(AccommodationPhoto), typeof(PhotosViewer), new PropertyMetadata(null));



        private int currentIndex;

        public MyICommand PreviousPhotoCommand { get; set; }
        public MyICommand NextPhotoCommand { get; set; }

        private static void ImagesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PhotosViewer viewer)
            {
                if (e.OldValue is ObservableCollection<ImageSource> oldCollection)
                {
                    oldCollection.CollectionChanged -= viewer.ImagesCollectionChanged;
                }

                if (e.NewValue is ObservableCollection<ImageSource> newCollection)
                {
                    newCollection.CollectionChanged += viewer.ImagesCollectionChanged;
                }

                viewer.LoadFirstPhoto();
            }
        }

        private void ImagesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LoadFirstPhoto();
        }


        public PhotosViewer()
        {
            InitializeComponent();

            this.Loaded += PhotosViewer_Loaded;
            this.Loaded += (s, e) => LoadFirstPhoto();
            this.Loaded += (s, e) => Keyboard.Focus(this);
            this.Loaded += (s, e) => Images.CollectionChanged += Images_CollectionChanged;

            NextPhotoCommand = new MyICommand(Execute_NextPhotoCommand);
            PreviousPhotoCommand = new MyICommand(Execute_PreviousPhotoCommand);
            //CommandManager.RegisterClassCommandBinding(typeof(PhotosViewer), new CommandBinding(PreviousPhotoCommand, Execute_PreviousPhotoCommand));
            //CommandManager.RegisterClassCommandBinding(typeof(PhotosViewer), new CommandBinding(NextPhotoCommand, Execute_NextPhotoCommand));
        }

        private void PhotosViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if (Images != null)
            {
                Images.CollectionChanged += ImagesCollectionChanged;
                LoadFirstPhoto();
            }
        }

        private void Images_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            LoadFirstPhoto();
        }

        public void Execute_PreviousPhotoCommand()
        {
            if (Images.Count > 0)
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    SelectedItem = Images[currentIndex];
                    CurrentPhoto = new BitmapImage(new Uri(SelectedItem.Path, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    currentIndex = Images.Count - 1;
                    SelectedItem = Images[currentIndex];
                    CurrentPhoto = new BitmapImage(new Uri(SelectedItem.Path, UriKind.RelativeOrAbsolute));
                }
            }
        }

        public void Execute_NextPhotoCommand()
        {
            if (Images.Count > 0)
            {
                if (currentIndex < Images.Count - 1)
                {
                    currentIndex++;
                    SelectedItem = Images[currentIndex];
                    CurrentPhoto = new BitmapImage(new Uri(SelectedItem.Path, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    currentIndex = 0;
                    SelectedItem = Images[currentIndex];
                    CurrentPhoto = new BitmapImage(new Uri(SelectedItem.Path, UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void LoadFirstPhoto()
        {
            if (Images.Count > 0)
            {
                currentIndex = 0;
                SelectedItem = Images[currentIndex];
                CurrentPhoto = new BitmapImage(new Uri(SelectedItem.Path, UriKind.RelativeOrAbsolute));
            }
            else
            {
                CurrentPhoto = null;
            }
        }

        private void NextPhoto(object sender, RoutedEventArgs e)
        {
            Execute_NextPhotoCommand();
        }

        private void PreviousPhoto(object sender, RoutedEventArgs e)
        {
            Execute_PreviousPhotoCommand();
        }
    }
}
