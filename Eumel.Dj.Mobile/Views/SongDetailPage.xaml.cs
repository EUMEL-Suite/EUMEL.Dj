using System;
using Eumel.Dj.Mobile.ViewModels;
using System.Net.Http;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Views
{
    public partial class SongDetailPage : ContentPage
    {
        public SongDetailPage()
        {
            InitializeComponent();
            BindingContext = new SongDetailViewModel();
        }
    }
}