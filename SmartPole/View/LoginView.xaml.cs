using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartPole.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<String>(this, "Sobre", (msg) =>
            {
                Navigation.PushAsync(new SobreView());
            });
            MessagingCenter.Subscribe<String>(this, "FalhaLogin", (msg) =>
            {
                DisplayAlert("Erro ao efetuar login", msg, "Ok");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<String>(this, "Sobre");
            MessagingCenter.Unsubscribe<String>(this, "FalhaLogin");
        }
    }
}