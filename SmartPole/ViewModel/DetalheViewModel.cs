﻿using Newtonsoft.Json;
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

        //HTTP metodos
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
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    Aguardar = true;
                    //string dispositivo, DateTime dataDe, DateTime dataAte 
                    string parameter = String.Format("?dispositivo={0}&dataDe={1}=&dataAte{2}",DispositivoSelecionado,DataDe,DataAte);
                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_API + Constantes.GET_HISTORICO + parameter);
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
                        DispositivoJson aux = JsonConvert.DeserializeObject<DispositivoJson>(conteudo);
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
