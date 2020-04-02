using AppTotutrial2.Class;
using AppTotutrial2.Model;
using FTA_Demo.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace AppTotutrial2.ViewModel
{
    public class DeliveryOrderListVM : IDisposable, INotifyPropertyChanged
    {
        #region Declaration of view binding
        public event PropertyChangedEventHandler PropertyChanged;

        ODLN _selectedItem;
        public ODLN selectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (_selectedItem == null) return;

                // handle the selected item here
                HanlderSelectItem(_selectedItem);

                selectedItem = null; // de select the selcted item from list
                OnPropertyChanged(nameof(selectedItem)); // update the listview to de selected
            }
        }

        List<ODLN> _list;
        public ObservableCollection<ODLN> list { get; private set; }

        // for search
        string _searhText; 
        public string searhText
        {
            get
            {
                return _searhText;
            }
            set
            {
                _searhText = value;
                
                /// handle the string text serach
                HanlderSearchText(_searhText);

                OnPropertyChanged(nameof(searhText));
            }
        }

        #endregion of binding declaration


        //Inner private declaration
        INavigation Navigation;
        readonly string webAddr = @"http://192.168.137.1:44365/mobws/cs";  //"http://192.168.137.1/"; 

        /// <summary>
        /// Constructor, class entry point
        /// </summary>
        public DeliveryOrderListVM(INavigation nagivation)
        {
            Navigation = nagivation;
            IntiList();
        }

        /// <summary>
        /// Sample to handle selected item
        /// </summary>
        /// <param name="selectedItem"></param>
        void HanlderSelectItem(ODLN selectedItem)
        {
            try
            {
                DisplayAlert("Doc info", selectedItem.DocNum + "\n" + selectedItem.DocTotal, "OK");
            }
            catch (Exception excep)
            {
                DisplayAlert("Alert", excep.ToString(), "OK");
            }
        }

        /// <summary>
        /// Handler and filter the listview data
        /// </summary>
        /// <param name="query"></param>
        void HanlderSearchText(string query)
        {

            if (_list == null)
            {
                return;
            }
                
            if (String.IsNullOrEmpty(query))
            {
                list = new ObservableCollection<ODLN>(_list);
                OnPropertyChanged(nameof(list));
            }
            else
            {
                string smallCaseQuery = query.ToLower();
                var filterList = _list.Where(x => x.detailsDisplay.ToLower().Contains(smallCaseQuery) ||
                            x.textDisplay.ToLower().Contains(smallCaseQuery)).ToList();

                if (filterList == null)
                {
                    list = new ObservableCollection<ODLN>(_list);
                    OnPropertyChanged(nameof(list));
                }
                else
                {
                    list = new ObservableCollection<ODLN>(filterList);
                    OnPropertyChanged(nameof(list));
                }
            }

        }

        /// <summary>
        /// Initial list from server
        /// </summary>
        async void IntiList ()
        {
            try
            {
                string webServerAddress = webAddr;   // App.su.WEBAPI_HOST + "/api/SapQp/";
                using (var client = new HttpClientWapi())
                {
                    Cio request = new Cio()
                    {
                        token = "any token bearer",
                        request = "any string"
                    };

                    var content = await client.RequestSvrAsync(webServerAddress, request);
                    if (client.isSuccessStatusCode)
                    {                                              
                        _list = JsonConvert.DeserializeObject<List<ODLN>>(content);

                        if (_list == null)
                        {
                            DisplayAlert("Alert", "List query from server is empty" + "\n" +
                                client.lastErrorDesc, "OK");
                            return;
                        }

                        list = new ObservableCollection<ODLN>(_list);
                        OnPropertyChanged(nameof(list));
                        return;
                    }

                    DisplayAlert("Alert", client.lastErrorDesc + 
                        "\nThere is some issue with server comm, please contact system support for this. Thx", "OK");
                }
            }
            catch (Exception excep)
            {
                DisplayAlert("Alert", excep.ToString(), "Ok");
            }
        }

        /// <summary>
        /// to show display message dialog on phone screen
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okBtn"></param>
        void DisplayAlert(string title, string message, string okBtn)
        {
            App.Current.MainPage.DisplayAlert(title, message, okBtn);
        }

        /// <summary>
        /// Handle the on property changed, value update to screen
        /// </summary>
        /// <param name="propertyname"></param>
        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
