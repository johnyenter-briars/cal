using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CAL.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        //public IDataStore<Event> EventDataStore => DependencyService.Get<IDataStore<Event>>();
        protected ICalClient CalClientSingleton = DependencyService.Get<ICalClient>();
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private bool fetchingData = false;
        public bool FetchingData
        {
            get { return fetchingData; }
            set
            {
                SetProperty(ref fetchingData, value);
            }
        }
        private bool notFetchingData = true;
        public bool NotFetchingData
        {
            get { return notFetchingData; }
            set { SetProperty(ref notFetchingData, value); }
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
    }
}
