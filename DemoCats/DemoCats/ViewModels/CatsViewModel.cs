using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DemoCats.ViewModels
{
    class CatsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Cat> Cats { get; set; }

        public CatsViewModel()
        {
            Cats = new ObservableCollection<Cat>();
            GetCatsCommand = new Command(
                async () => await GetCats(),
                () => !IsBusy);
        }

        public Command GetCatsCommand { get; set; }

        private bool Busy;

        public bool IsBusy
        {
            get { return Busy; }
            set {
                Busy = value;
                OnPropertyChanged();
                GetCatsCommand.ChangeCanExecute();
            }
        }

        async Task GetCats()
        {
            if (!IsBusy)
            {
                Exception Error = null;
                try
                {
                    IsBusy = true;
                    using (var Client = new HttpClient())
                    {
                        var URLWebAPI = "http://demos.ticapacitacion.com/cats";
                        var JSON = await Client.GetStringAsync(URLWebAPI);
                        var Item = JsonConvert.DeserializeObject<List<Cat>>(JSON);
                        Cats.Clear();
                        foreach (var Cat in Item)
                        {
                            Cats.Add(Cat);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error: {ex}");
                    Error = ex;
                }
                finally
                {
                    IsBusy = false;
                }
                if (Error != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", Error.Message, "OK");
                }
            }
            return;
        }

        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
