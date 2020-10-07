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


        /*   public static readonly SKColor TextColor = SKColors.Black;
           private Chart chart { get; set; }
           public Chart Chart
           {
               get
               {
                   return chart;
               }
               set
               {
                   chart = value;
                   OnPropertyChanged();
               }
           }*/

        public ObservableCollection<string> Dispositivos { get; set; }
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
                MessagingCenter.Send<String>("ConsultarDados",dispositivoSelecionado);
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
            }
        }

        public ICommand btnConsultar { get; set; }

        public DetalheViewModel()
        {
            this.Dispositivos = new ObservableCollection<string>();     

            DataDe = DateTime.Today;
            DataAte = DateTime.Today;

            MessagingCenter.Send<String>(String.Empty, "ConsultarDispositivo");
        }

        /* public void PreencherGraficos()
         {
             foreach (SensorArray entidade in Entidades)
             {
                 List<ChartEntry> entryList = new List<ChartEntry>();
                 foreach (Sensor sensor in entidade.Energia)
                 {
                     if (sensor.Data.Date >= dataDe.Date && sensor.Data.Date <= dataAte.Date)
                     {
                         ChartEntry entry = new ChartEntry(float.Parse(sensor.Valor))
                         {
                             Label = sensor.Nome,
                             Color = SKColor.Parse("#E52510"),
                             TextColor = TextColor
                         };
                         entryList.Add(entry);
                     }
                 }
                 Chart = new LineChart() { Entries = entryList };
             }
         }*/

        public async Task ConsultarDispositivo()
        {
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    Aguardar = true;

                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_API + Constantes.GET_DISPOSITIVO);
                    if (resposta.IsSuccessStatusCode)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        List<string> lista = (JsonConvert.DeserializeObject<string[]>(conteudo).ToList());
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
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    Aguardar = true;

                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_API + Constantes.GET_HISTORICO);
                    if (resposta.IsSuccessStatusCode)
                    {
                        Dispositivos.Clear();
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        List<string> lista = (JsonConvert.DeserializeObject<string[]>(conteudo).ToList());
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

        public async Task ConsultarDados(string selecionado)
        {
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    Aguardar = true;

                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_HELIX + Constantes.GET_ENTITIES + selecionado);
                    if (resposta.IsSuccessStatusCode)
                    {
                        Dispositivos.Clear();
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        List<string> lista = (JsonConvert.DeserializeObject<string[]>(conteudo).ToList());
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
    }
}
