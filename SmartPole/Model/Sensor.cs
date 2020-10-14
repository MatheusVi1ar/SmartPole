using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPole.Model
{
    public class Entidade
    {
        public List<Sensor> Vazao { get; set; }
        public List<Sensor> Luminosidade { get; set; }
        public List<Sensor> Energia { get; set; }
        public List<Sensor> Temperatura { get; set; }

        public string Collection { get; set; }

        public Dados DadosRecentes { get; set; }

        public Entidade()
        {
            Vazao = new List<Sensor>();
            Luminosidade = new List<Sensor>();
            Energia = new List<Sensor>();
            Temperatura = new List<Sensor>();
            DadosRecentes = new Dados();
        }
    }

    public class Sensor
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public DateTime Data { get; set; }
    }

    public class Dados
    {
        public Sensor Vazao { get; set; }
        public Sensor Luminosidade { get; set; }
        public Sensor Energia { get; set; }
        public Sensor Temperatura { get; set; }
    }
}
