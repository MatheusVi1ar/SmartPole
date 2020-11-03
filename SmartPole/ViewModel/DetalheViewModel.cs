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
        public ObservableCollection<string> TiposSensores { get; set; }
        private List<Sensor> listaChart { get; set; }
        public List<Sensor> ListaChart
        {
            get
            {
                return listaChart;
            }
            set
            {
                listaChart = value;
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

        private string sensorSelecionado { get; set; }
        public string SensorSelecionado
        {
            get
            {
                return sensorSelecionado;
            }
            set
            {
                sensorSelecionado = value;
                ((Command)CmdBuscar).ChangeCanExecute();
            }
        }

        private string titulo { get; set; }
        public string Titulo
        {
            get
            {
                return titulo;
            }
            set
            {
                titulo = value;
                OnPropertyChanged();
            }
        }

        private bool isVisible { get; set; }
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                OnPropertyChanged();
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
            Aguardar = false;
            CmdBuscar = new Command(() =>
            {
                MessagingCenter.Send<String>(String.Empty, "ConsultarHistorico");
            }, () =>
            {
                return DataDe != null
                && DataAte != null
                && !string.IsNullOrEmpty(dispositivoSelecionado)
                && !string.IsNullOrEmpty(SensorSelecionado);
            });

            if (this.ListaChart == null)
                this.ListaChart = new List<Sensor>();
            if (this.Dispositivos == null)
                this.Dispositivos = new ObservableCollection<string>();
            if (this.TiposSensores == null)
            {
                this.TiposSensores = new ObservableCollection<string>();
                Enum.GetNames(typeof(Constantes.TipoSensor)).Select(b => b).ToList().ForEach((item) =>
                {
                    this.TiposSensores.Add(item);
                });
            }

            if (this.Dispositivos == null)
                IsVisible = false;

            if (this.DataDe == DateTime.MinValue)
                DataDe = DateTime.Today;
            if (this.DataAte == DateTime.MinValue)
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
            IsVisible = false;
            SmartPole.Servico.Service service = new SmartPole.Servico.Service();
            Entidade aux = await service.ConsultarHistorico(DispositivoSelecionado, DataDe, DataAte);

            if (SensorSelecionado == Constantes.TipoSensor.Energia.ToString())
                ListaChart = aux.Energia;

            if (SensorSelecionado == Constantes.TipoSensor.Gás.ToString())
                ListaChart = aux.Gas;

            if (SensorSelecionado == Constantes.TipoSensor.Luz.ToString())
                ListaChart = aux.Luminosidade;

            if (SensorSelecionado == Constantes.TipoSensor.Temperatura.ToString())
                ListaChart = aux.Temperatura;

            if (SensorSelecionado == Constantes.TipoSensor.Umidade.ToString())
                ListaChart = aux.Umidade;

            if (SensorSelecionado == Constantes.TipoSensor.Vazão.ToString())
                ListaChart = aux.Vazao;

            Titulo = SensorSelecionado;
            IsVisible = true;

            /* VazaoVisible = Collection.Vazao.Count > 0;
             TemperaturaVisible = Collection.Temperatura.Count > 0;
             LuminosidadeVisible = Collection.Luminosidade.Count > 0;
             EnergiaVisible = Collection.Energia.Count > 0;
             UmidadeVisible = Collection.Umidade.Count > 0;
             GasVisible = Collection.Gas.Count > 0;
            */
            Aguardar = false;
        }
    }
}
