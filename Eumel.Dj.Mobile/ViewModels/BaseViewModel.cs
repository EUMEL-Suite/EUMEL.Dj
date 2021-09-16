using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Services;
using Eumel.Dj.Mobile.Views;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected ISettingsService Settings => DependencyService.Get<ISettingsService>();
        protected IPlaylistService PlaylistService => DependencyService.Get<IPlaylistService>();
        protected IPlayerService PlayerService => DependencyService.Get<IPlayerService>();
        protected ISongService SongService => DependencyService.Get<ISongService>();
        protected ISyslogService SyslogService => DependencyService.Get<ISyslogService>();
        protected IChatService ChatService => DependencyService.Get<IChatService>();

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _title = string.Empty;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        protected async void TryOrRedirectToLoginAsync(Func<Task> action, string actionTitle = "unknown")
        {
            try
            {
                SyslogService.Debug($"[{actionTitle}] Executing code in a login-safe environment.");
                await action();
            }
            catch (ApiException ex)
            {
                if (ex.Response.Contains(Constants.InvalidTokenException))
                {
                    SyslogService.Information($"The token for user {Settings.Username} is invalid and user needs to login again.");
                    await Application.Current.MainPage.DisplayAlert("Token Invalid", "Your login token expired. Please login again.", "OK");
                    Application.Current.MainPage = new LoginPage() { BackgroundColor = Color.White };
                }
                else if (ex.Response.Contains(Constants.UnauthorizedEumelException))
                {
                    SyslogService.Information($"The user {Settings.Username} has not sufficient permissions to execute the action.");
                    await Application.Current.MainPage.DisplayAlert("Permission Denied", "You are not allowed to do this. Please request permissions.", "OK");
                }
                else
                {
                    SyslogService.Error($"A REST service exception was raised: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                SyslogService.Error($"An exception was raised: {ex.Message}");
                throw;
            }
        }
    }
}
