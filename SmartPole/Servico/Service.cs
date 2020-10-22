using Newtonsoft.Json;
using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartPole.Servico
{
    public class Service
    {
        public async Task<List<string>> ConsultarDispositivo()
        {

            using (HttpClient cliente = new HttpClient())
            {
                List<string> lista = new List<string>();
                try
                {               

                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_API + Constantes.GET_DISPOSITIVO);
                    if (resposta.IsSuccessStatusCode)
                    {
                       // Dispositivos.Clear();
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        lista = JsonConvert.DeserializeObject<string[]>(conteudo).ToList();                        
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
                return lista;                
            }
        }

        public async Task<Entidade> ConsultarHistorico(string DispositivoSelecionado, DateTime DataDe, DateTime DataAte)
        {
            using (HttpClient cliente = new HttpClient())
            {
                Entidade collection = new Entidade();
                try
                {
                    cliente.DefaultRequestHeaders.Add("Accept", "application/json");
                    string parameter = String.Format("?dispositivo={0}&dataDe={1}&dataAte={2}", DispositivoSelecionado, DataDe.Date.ToString("dd/MM/yyyy"), DataAte.Date.ToString("dd/MM/yyyy"));

                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_API + Constantes.GET_HISTORICO + parameter);
                    //HttpResponseMessage resposta = await cliente.GetAsync("https://localhost:44391/SmartMeter" + Constantes.GET_HISTORICO + parameter);

                    if (resposta.IsSuccessStatusCode)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        collection = JsonConvert.DeserializeObject<Entidade>(conteudo);
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
                return collection;
            }
        }

        public async Task<DadosJson> ConsultarDados(string selecionado)
        {
            using (HttpClient cliente = new HttpClient())
            {
                DadosJson aux = new DadosJson();
                cliente.DefaultRequestHeaders.Add("Accept", "application/json");
                cliente.DefaultRequestHeaders.Add("fiware-service", "helixiot");
                cliente.DefaultRequestHeaders.Add("fiware-servicepath", "/");
                try
                {
                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_HELIX + Constantes.GET_ENTITIES + selecionado);
                    if (resposta.IsSuccessStatusCode)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        aux = JsonConvert.DeserializeObject<DadosJson>(conteudo);
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
                return aux;
            }
        }

        public async Task<bool> ValidaUsuario(UsuarioModel usuario)
        {
            using (HttpClient cliente = new HttpClient())
            {
                bool output = false;                
                //cliente.BaseAddress = new Uri();

                cliente.DefaultRequestHeaders.Add("Accept", "application/json");
                cliente.DefaultRequestHeaders.Add("fiware-service", "helixiot");
                cliente.DefaultRequestHeaders.Add("fiware-servicepath", "/");

                try
                {
                    HttpResponseMessage resposta = await cliente.GetAsync(Constantes.URL_HELIX + Constantes.GET_ENTITIES + usuario.Login);
                    if (resposta.IsSuccessStatusCode)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        UsuarioJson usuariojson = JsonConvert.DeserializeObject<UsuarioJson>(conteudo);

                        if (usuariojson.senha.value == usuario.Senha)
                        {
                            output = true;
                        }
                        else
                        {
                            MessagingCenter.Send<String>("Usuario/Senha incorretos.", "FalhaLogin");
                        }
                    }
                    else
                    {
                        MessagingCenter.Send<String>("Usuario/Senha incorretos.", "FalhaLogin");
                    }

                }
                catch (HttpRequestException)
                {
                    MessagingCenter.Send<String>("Não foi possivel efetuar o login, verifique a sua conexão e tente novamente.", "FalhaLogin");
                }
                return output;
            }
        }
    }
}
