using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPole.Model
{
    public class DadosJson : JSON
    {
        public class Energia
        {
            public string type { get; set; }
            public string value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Luz
        {
            public string type { get; set; }
            public string value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Luminosidade
        {
            public string type { get; set; }
            public string value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Temperatura
        {
            public string type { get; set; }
            public string value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Vazao
        {
            public string type { get; set; }
            public string value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Gas
        {
            public string type { get; set; }
            public string value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Umidade
        {
            public string type { get; set; }
            public string value { get; set; }
            public Metadata metadata { get; set; }
        }

        public string id { get; set; }
        public string type { get; set; }
        public Energia energia { get; set; }
        public Luz luz { get; set; }
        public Luminosidade luminosidade { get; set; }
        public Temperatura temperatura { get; set; }
        public Vazao vazao { get; set; }
        public Gas gas { get; set; }
        public Umidade umidade { get; set; }
    }
}
