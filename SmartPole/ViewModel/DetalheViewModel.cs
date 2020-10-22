using Newtonsoft.Json;
using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartPole.ViewModel
{
    public class DetalheViewModel : BaseViewModel
    {
        public ObservableCollection<string> Dispositivos { get; set; }
        private Entidade collection { get; set; }
        public Entidade Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
                OnPropertyChanged();
            }
        }

        private bool vazaoVisible { get; set; }
        public bool VazaoVisible
        {
            get
            {
                return vazaoVisible;

            }
            set
            {
                vazaoVisible = value;
                OnPropertyChanged();
            }
        }
        private bool temperaturaVisible { get; set; }
        public bool TemperaturaVisible
        {
            get
            {
                return temperaturaVisible;

            }
            set
            {
                temperaturaVisible = value;
                OnPropertyChanged();
            }
        }

        private bool luminosidadeVisible { get; set; }
        public bool LuminosidadeVisible
        {
            get
            {
                return luminosidadeVisible;

            }
            set
            {
                luminosidadeVisible = value;
                OnPropertyChanged();
            }
        }

        private bool energiaVisible { get; set; }
        public bool EnergiaVisible
        {
            get
            {
                return energiaVisible;

            }
            set
            {
                energiaVisible = value;
                OnPropertyChanged();
            }
        }

        private bool gasVisible { get; set; }
        public bool GasVisible
        {
            get
            {
                return gasVisible;

            }
            set
            {
                gasVisible = value;
                OnPropertyChanged();
            }
        }

        private bool umidadeVisible { get; set; }
        public bool UmidadeVisible
        {
            get
            {
                return umidadeVisible;

            }
            set
            {
                umidadeVisible = value;
                OnPropertyChanged();
            }
        }

        private string dispositivoSelecionado { get; set; }
        public string DispositivoSelecionado
        {
            get
            {
                return dispositivoSelecionado;
            }
            set
            {
                dispositivoSelecionado = value;
                ((Command)CmdBuscar).ChangeCanExecute();
            }
        }
        private bool aguardar { get; set; }
        public bool Aguardar
        {
            get
            {
                return aguardar;
            }
            set
            {
                aguardar = value;
                OnPropertyChanged();
            }
        }

        public bool Completo
        {
            get
            {
                return !aguardar;
            }
        }
        private DateTime dataDe { get; set; }
        public DateTime DataDe
        {
            get
            {
                return dataDe;
            }
            set
            {
                dataDe = value;
                OnPropertyChanged();
                ((Command)CmdBuscar).ChangeCanExecute();
            }
        }

        private DateTime dataAte { get; set; }
        public DateTime DataAte
        {
            get
            {
                return dataAte;
            }
            set
            {
                dataAte = value;
                OnPropertyChanged();
                ((Command)CmdBuscar).ChangeCanExecute();
            }
        }

        public ICommand CmdBuscar { get; set; }

        public DetalheViewModel()
        {
            CmdBuscar = new Command(() =>
            {
                MessagingCenter.Send<String>(String.Empty, "ConsultarHistorico");
            }, () =>
            {
                return DataDe != null
                && DataAte != null
                && !string.IsNullOrEmpty(dispositivoSelecionado);
            });

            this.Dispositivos = new ObservableCollection<string>();
            this.Collection = new Entidade();
            DataDe = DateTime.Today;
            DataAte = DateTime.Today;
        }

        //HTTP metodos
        public async void ConsultarDispositivo()
        {
            if (this.Dispositivos.Count > 0)
                Dispositivos.Clear();

            Aguardar = true;
            //Dispositivos.Clear();            
            SmartPole.Servico.Service service = new SmartPole.Servico.Service();
            List<string> lista = await service.ConsultarDispositivo();
            lista.ForEach((item) =>
            {
                Dispositivos.Add(item);
            });
            Aguardar = false;
        }

        public async Task ConsultarHistorico()
        {
            if (DataDe > DataAte)
                MessagingCenter.Send<String>("Para efetuar a consulta escolha um período válido", "FalhaConsulta");

            Aguardar = true;
            SmartPole.Servico.Service service = new SmartPole.Servico.Service();
            Collection = await service.ConsultarHistorico(DispositivoSelecionado, DataDe, DataAte);

            VazaoVisible = Collection.Vazao.Count > 0;
            TemperaturaVisible = Collection.Temperatura.Count > 0;
            LuminosidadeVisible = Collection.Luminosidade.Count > 0;
            EnergiaVisible = Collection.Energia.Count > 0;
            UmidadeVisible = Collection.Umidade.Count > 0;
            GasVisible = Collection.Gas.Count > 0;

            Aguardar = false;
        }
    }
}
