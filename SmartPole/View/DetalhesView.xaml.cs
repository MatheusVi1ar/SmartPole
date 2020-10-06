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
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.DataDe = DateTime.Today;
            viewModel.DataAte = DateTime.Today;
            await viewModel.ConsultarHistorico();
        }
    }
}
