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
                if (value != null)
                {
                    ConsultarDados(dispositivoSelecionado);
                }
                LocalVisible = (value != null);                
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

        //HTTP metodos
        public async void ConsultarDispositivo()
        {
            if (Dispositivos.Count > 0)
                Dispositivos.Clear();

            Aguardar = true;            
            SmartPole.Servico.Service service = new SmartPole.Servico.Service();
            List<string> lista = await service.ConsultarDispositivo();
            lista.ForEach((item) =>
            {
                Dispositivos.Add(item);
            });
            Aguardar = false;
        }

        public async void ConsultarDados(string selecionado)
        {
            Aguardar = true;
            SmartPole.Servico.Service service = new SmartPole.Servico.Service();
            DadosRecentes = await service.ConsultarDados(selecionado);

            if (DadosRecentes.vazao != null)
            {
                VazaoVisible = !string.IsNullOrEmpty(DadosRecentes.vazao.value);
            }
            else
            {
                VazaoVisible = false;
            }

            if (DadosRecentes.temperatura != null)
            {
                TemperaturaVisible = !string.IsNullOrEmpty(DadosRecentes.temperatura.value);
            }
            else
            {
                TemperaturaVisible = false;
            }

            if (DadosRecentes.luminosidade != null)
            {
                LuminosidadeVisible = !string.IsNullOrEmpty(DadosRecentes.luminosidade.value);
            }
            else
            {
                LuminosidadeVisible = false;
            }

            if (DadosRecentes.energia != null)
                EnergiaVisible = !string.IsNullOrEmpty(DadosRecentes.energia.value);
            else
            {
                EnergiaVisible = false;
            }

            if (DadosRecentes.umidade != null)
            {
                UmidadeVisible = !string.IsNullOrEmpty(DadosRecentes.umidade.value);
            }
            else
            {
                UmidadeVisible = false;
            }

            if (DadosRecentes.gas != null)
            {
                GasVisible = !string.IsNullOrEmpty(DadosRecentes.gas.value);
            }
            else
            {
                GasVisible = false;
            }

            Aguardar = false;
        }

        public Command cmdGPS { get; set; }
        public GeralViewModel()
        {
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
            });

            this.Dispositivos = new ObservableCollection<string>();
            this.Collection = new Entidade();
        }
    }
}
