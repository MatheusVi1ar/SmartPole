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

            if(viewModel.Dispositivos.Count == 0)
                viewModel.ConsultarDispositivo();

            MessagingCenter.Subscribe<String>(this, "FalhaConsulta", (msg) =>
            {
                DisplayAlert("Erro de conexão", msg, "Ok");
            });

            MessagingCenter.Subscribe<String>(this, "ConsultarHistorico",async (msg) =>
            {
               await viewModel.ConsultarHistorico();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<String>(this, "FalhaConsulta");
            MessagingCenter.Unsubscribe<String>(this, "ConsultarHistorico");
        }
    }
}
