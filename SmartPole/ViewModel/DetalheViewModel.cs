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

            MessagingCenter.Send<String>(String.Empty, "ConsultarDispositivo");
        }

        //HTTP metodos
        public async Task ConsultarDispositivo()
        {
            if (Dispositivos.Count > 0)
                return;

            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    Aguardar = true;

                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_API + Constantes.GET_DISPOSITIVO);
                    if (resposta.IsSuccessStatusCode)
                    {
                        Dispositivos.Clear();
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        List<string> lista = JsonConvert.DeserializeObject<string[]>(conteudo).ToList();
                        lista.ForEach((item) =>
                        {
                            Dispositivos.Add(item);
                        });
                    }
                    else
                    {
                        MessagingCenter.Send<String>("Não foi possível acessar o histórico do MongoDB", "FalhaConsulta");
                    }
                }
                catch (HttpRequestException)
                {
                    MessagingCenter.Send<String>("Não foi possivel acessar o servidor, verifique a sua conexão e tente novamente.", "FalhaConsulta");
                }
                Aguardar = false;
            }
        }

        public async Task ConsultarHistorico()
        {
            if(DataDe > DataAte)
                MessagingCenter.Send<String>("Para efetuar a consulta escolha um período válido", "FalhaConsulta");
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    Aguardar = true;
                    cliente.DefaultRequestHeaders.Add("Accept", "application/json");
                    string parameter = String.Format("?dispositivo={0}&dataDe={1}&dataAte={2}",DispositivoSelecionado,DataDe.Date.ToString("dd/MM/yyyy"), DataAte.Date.ToString("dd/MM/yyyy"));
                    
                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_API + Constantes.GET_HISTORICO + parameter);
                    //HttpResponseMessage resposta = await cliente.GetAsync("https://localhost:44391/SmartMeter" + Constantes.GET_HISTORICO + parameter);
                   
                    if (resposta.IsSuccessStatusCode)
                    {                        
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        Collection = JsonConvert.DeserializeObject<Entidade>(conteudo);
                        VazaoVisible = Collection.Vazao.Count > 0;
                        TemperaturaVisible = Collection.Temperatura.Count > 0;
                        LuminosidadeVisible = Collection.Luminosidade.Count > 0;
                        EnergiaVisible = Collection.Energia.Count > 0;
                    }
                    else
                    {
                        MessagingCenter.Send<String>("Não foi possível acessar o histórico do MongoDB", "FalhaConsulta");
                    }
                }
                catch (HttpRequestException)
                {
                    MessagingCenter.Send<String>("Não foi possivel acessar o servidor, verifique a sua conexão e tente novamente.", "FalhaConsulta");
                }
                Aguardar = false;
            }
        }

        public async Task ConsultarDados(string selecionado)
        {
            if (string.IsNullOrEmpty(selecionado))
                return;
            using (HttpClient cliente = new HttpClient())
            {
                cliente.DefaultRequestHeaders.Add("Accept", "application/json");
                cliente.DefaultRequestHeaders.Add("fiware-service", "helixiot");
                cliente.DefaultRequestHeaders.Add("fiware-servicepath", "/");
                try
                {
                    Aguardar = true;

                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_HELIX + Constantes.GET_ENTITIES + selecionado);
                    if (resposta.IsSuccessStatusCode)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                      //  DispositivoJson aux = JsonConvert.DeserializeObject<DispositivoJson>(conteudo);
                        string x = "1";
                    }
                    else
                    {
                        MessagingCenter.Send<String>("Não foi possível acessar o histórico do MongoDB", "FalhaConsulta");
                    }
                }
                catch (HttpRequestException)
                {
                    MessagingCenter.Send<String>("Não foi possivel acessar o servidor, verifique a sua conexão e tente novamente.", "FalhaConsulta");
                }
                Aguardar = false;
            }
        }
    }
}
