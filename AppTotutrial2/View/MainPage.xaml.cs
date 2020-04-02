using AppTotutrial2.View;
using AppTotutrial2.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTotutrial2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// View contructor, view entry point
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVM(Navigation);            
        }
    }
}
