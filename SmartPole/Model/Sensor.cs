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
        public List<Sensor> Umidade { get; set; }
        public List<Sensor> Gas { get; set; }

        public string Collection { get; set; }

        public Entidade()
        {
            Vazao = new List<Sensor>();
            Luminosidade = new List<Sensor>();
            Energia = new List<Sensor>();
            Temperatura = new List<Sensor>();
            Umidade = new List<Sensor>();
            Gas = new List<Sensor>();
        }
    }

    public class Sensor
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
