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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzM0NDEwQDMxMzgyZTMzMmUzMFpGeEU3a2ZoUHBlL2ZveExodlg1RnpYbDBwTC9vWk1VUkx6QnE1WHZwZHM9");
            InitializeComponent();
            MainPage = new NavigationPage(new LoginView())
            {
                BarBackgroundColor = Color.FromHex("#b2fefc"),
                BarTextColor = Color.Black
            };
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
