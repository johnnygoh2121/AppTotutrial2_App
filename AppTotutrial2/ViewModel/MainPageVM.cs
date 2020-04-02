using AppTotutrial2.View;
using Xamarin.Forms;

namespace AppTotutrial2.ViewModel
{
    public class MainPageVM
    {
        public Command CmdOpenDoView { get; private set; }
        INavigation Navigation;

        /// <summary>
        /// Constructor, class entry point
        /// </summary>
        /// <param name="navigation"></param>
        public MainPageVM (INavigation navigation)
        {
            Navigation = navigation;
            InitCmd();
        }

        /// <summary>
        /// Init the command 
        /// </summary>
        void InitCmd ()
        {
            // setup the button clicked action step
            CmdOpenDoView = new Command( async () => 
            {
               await Navigation.PushAsync(new DeliveryOrderListView());
            });
        }

    }
}
