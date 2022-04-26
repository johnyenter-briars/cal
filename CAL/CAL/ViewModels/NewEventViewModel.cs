using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Cal.Request;
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
            return !string.IsNullOrWhiteSpace(text);
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
                Description = description,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                CalUserId = new Guid("a188e597-29f9-4e2f-aa46-e3713d9939da"),
             };

            await EventDataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
