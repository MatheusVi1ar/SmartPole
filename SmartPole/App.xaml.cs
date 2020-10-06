using SmartPole.Model;
using SmartPole.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartPole
{
    public partial class App : Application
    {
        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("@31382e322e30jmKvxNcxXkmfmS0JAWQiPmnipWYn+28UWFwSaWzYPV8=");
            InitializeComponent();
            MainPage = new NavigationPage(new LoginView());
        }

        protected override void OnStart()
        {
            MessagingCenter.Subscribe<UsuarioModel>(this, "SucessoLogin", (usuario) =>
              {
                  MainPage = new TabbedView();
              });            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
