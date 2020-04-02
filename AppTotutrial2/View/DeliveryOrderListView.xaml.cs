using AppTotutrial2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTotutrial2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveryOrderListView : ContentPage
    {
        public DeliveryOrderListView()
        {
            InitializeComponent();
            BindingContext = new DeliveryOrderListVM(Navigation);
        }
    }
}