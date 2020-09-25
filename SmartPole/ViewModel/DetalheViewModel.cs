using Microcharts;
using Newtonsoft.Json;
using SkiaSharp;
using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartPole.ViewModel
{
    public class DetalheViewModel : BaseViewModel
    {
        const string URL_BASE = "https://smartpoleapi.azurewebsites.net";
        const string GET_HISTORICO = "/smartmeter";

        public static readonly SKColor TextColor = SKColors.Black;
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
        }
        public List<SensorArray> Entidades { get; set; }
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
        public void PreencherGraficos()
        {
            foreach (SensorArray entidade in Entidades)
            {
                List<ChartEntry> entryList = new List<ChartEntry>();
                foreach (Sensor sensor in entidade.Energia)
                {
                    var entry = new ChartEntry(33.4f)
                    {
                        Label = sensor.Nome,
                        ValueLabel = sensor.Valor,
                        Color = SKColor.Parse("#E52510"),
                        TextColor = TextColor
                    };
                    entryList.Add(entry);
                }
                Chart = new LineChart() { Entries = entryList };
            }
        }

        public async Task ConsultarHistorico()
        {
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    Aguardar = true;
                    HttpResponseMessage resposta = await cliente.GetAsync(URL_BASE + GET_HISTORICO);
                    if (resposta.IsSuccessStatusCode)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        List<SensorArray> lista = JsonConvert.DeserializeObject<SensorArray[]>(conteudo).ToList();
                        Entidades = lista;
                    }
                    else
                    {
                        MessagingCenter.Send<String>("Não foi possível acessar o histórico do MongoDB", "FalhaLogin");
                    }

                }
                catch (HttpRequestException)
                {
                    MessagingCenter.Send<String>("Não foi possivel efetuar o login, verifique a sua conexão e tente novamente.", "FalhaLogin");
                }
                Aguardar = false;
            }
        }
    }
}
