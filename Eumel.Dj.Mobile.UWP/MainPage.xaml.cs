namespace Eumel.Dj.Mobile.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new Mobile.App());
        }
    }
}