using Newtonsoft.Json;
using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartPole.ViewModel
{
    public class GeralViewModel : BaseViewModel
    {
        public ObservableCollection<string> Dispositivos { get; set; }
        public ObservableCollection<string> TiposSensores { get; set; }
        DadosJson DadosRecentes { get; set; }
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

        private bool localVisible { get; set; }
        public bool LocalVisible
        {
            get
            {
                return localVisible;
            }
            set
            {
                localVisible = value;
                ((Command)cmdGPS).ChangeCanExecute();
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
                if (value != null)
                {
                    ConsultarDados(dispositivoSelecionado);
                }
                LocalVisible = (value != null);
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
                if (value != null)
                {
                    MostrarGrafico(sensorSelecionado);
                }
                OnPropertyChanged();
            }
        }

        private string valorGrafico { get; set; }
        public string ValorGrafico
        {
            get
            {
                return valorGrafico;
            }
            set
            {
                valorGrafico = value;
                OnPropertyChanged();
            }
        }

        private bool gaugeVisible { get; set; }
        public bool GaugeVisible
        {
            get
            {
                return gaugeVisible;
            }
            set
            {
                gaugeVisible = value;
                OnPropertyChanged();
            }
        }
        
        private bool sensorVisible { get; set; }
        public bool SensorVisible
        {
            get
            {
                return sensorVisible;
            }
            set
            {
                sensorVisible = value;
                OnPropertyChanged();
            }
        }

        //HTTP metodos
        public async void ConsultarDispositivo()
        {
            if (Dispositivos.Count > 0)
                Dispositivos.Clear();

            Aguardar = true;
            SmartPole.Servico.Service service = new SmartPole.Servico.Service();
            List<string> lista = await service.ConsultarDispositivo();
            Dispositivos.Clear();
            lista.ForEach((item) =>
            {
                Dispositivos.Add(item);
            });
            Aguardar = false;
        }

        public async void ConsultarDados(string selecionado)
        {
            Aguardar = true;
            GaugeVisible = false;
            SmartPole.Servico.Service service = new SmartPole.Servico.Service();
            DadosRecentes = await service.ConsultarDados(selecionado);
            TiposSensores.Clear();
            if (DadosRecentes.vazao != null &&  !String.IsNullOrWhiteSpace(DadosRecentes.vazao.value))
                TiposSensores.Add(Constantes.TipoSensor.Vazão.ToString());
            if (DadosRecentes.temperatura != null && !String.IsNullOrWhiteSpace(DadosRecentes.temperatura.value))
                TiposSensores.Add(Constantes.TipoSensor.Temperatura.ToString());
            if (DadosRecentes.luminosidade != null && !String.IsNullOrWhiteSpace(DadosRecentes.luminosidade.value))
                TiposSensores.Add(Constantes.TipoSensor.Luz.ToString());
            if (DadosRecentes.energia != null && !String.IsNullOrWhiteSpace(DadosRecentes.energia.value))
                TiposSensores.Add(Constantes.TipoSensor.Energia.ToString());
            if (DadosRecentes.umidade != null && !String.IsNullOrWhiteSpace(DadosRecentes.umidade.value))
                TiposSensores.Add(Constantes.TipoSensor.Umidade.ToString());
            if (DadosRecentes.gas != null && !String.IsNullOrWhiteSpace(DadosRecentes.gas.value))
                TiposSensores.Add(Constantes.TipoSensor.Gás.ToString());

            SensorVisible = TiposSensores.Count > 0;            
            Aguardar = false;
        }

        public void MostrarGrafico(string selecionado)
        {
            GaugeVisible = true;
            if (selecionado == Constantes.TipoSensor.Vazão.ToString())
                ValorGrafico = DadosRecentes.vazao.value;
            if (selecionado == Constantes.TipoSensor.Temperatura.ToString())
                ValorGrafico = DadosRecentes.temperatura.value;
            else if (selecionado == Constantes.TipoSensor.Luz.ToString())
                ValorGrafico = DadosRecentes.luminosidade.value;
            else if (selecionado == Constantes.TipoSensor.Energia.ToString())
                ValorGrafico = DadosRecentes.energia.value;
            else if (selecionado == Constantes.TipoSensor.Umidade.ToString())
                ValorGrafico = DadosRecentes.umidade.value;
            else if (selecionado == Constantes.TipoSensor.Gás.ToString())
                ValorGrafico = DadosRecentes.gas.value;
        }

        public Command cmdGPS { get; set; }
        public GeralViewModel()
        {
            Aguardar = false;
            cmdGPS = new Command(async () =>
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Localizacao gps = new Localizacao();
                    gps.Altitude = location.Altitude;
                    gps.Latitude = location.Latitude;
                    gps.Longitude = location.Longitude;
                    MessagingCenter.Send(gps, "GPS");
                }
                else
                {
                    MessagingCenter.Send("Localização não encontrada","FalhaConsulta");
                }
            },
            () =>
            {
                return LocalVisible;
            });

            if (this.Dispositivos == null)
                this.Dispositivos = new ObservableCollection<string>();
            if (this.TiposSensores == null)
                this.TiposSensores = new ObservableCollection<string>();
            if (this.Collection == null)
                this.Collection = new Entidade();

            GaugeVisible = false;
            SensorVisible = false;
        }
    }
}
