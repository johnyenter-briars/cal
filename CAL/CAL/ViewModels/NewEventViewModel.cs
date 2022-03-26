using CAL.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CAL.ViewModels
{
    public class NewEventViewModel : BaseViewModel
    {
        private string text;
        private string description;

        public NewEventViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(text)
                && !string.IsNullOrWhiteSpace(description);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Event newItem = new Event()
            {
                Name = text,
                Id = 4,
            };

            await DataStore.AddEventAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
