using SmartPole.ViewModel;
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
    public partial class DetalhesView : ContentPage
    {
        DetalheViewModel viewModel { get; set; }
        public DetalhesView()
        {
            InitializeComponent();
            viewModel = new DetalheViewModel();
            this.BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<String>(this, "FalhaConsulta", (msg) =>
            {
                DisplayAlert("Erro de conexão", msg, "Ok");
            });
            MessagingCenter.Subscribe<String>(this, "ConsultarDispositivo", async (msg) =>
            {
                await ConsultarDispositivo();
            });
        }

        private Task ConsultarDispositivo()
        {
            throw new NotImplementedException();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<String>(this, "FalhaConsulta");
            MessagingCenter.Unsubscribe<String>(this, "ConsultarDispositivo");
        }
    }
}
