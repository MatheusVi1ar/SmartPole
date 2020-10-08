using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPole.Model
{
    public class Energia : JSON
    {
        public string type { get; set; }
        public string value { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Luz : JSON
    {
        public string type { get; set; }
        public string value { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Temperatura : JSON
    {
        public string type { get; set; }
        public string value { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Vazao : JSON
    {
        public string type { get; set; }
        public string value { get; set; }
        public Metadata metadata { get; set; }
    }

    public class DispositivoJson : JSON
    {
        public string id { get; set; }
        public string type { get; set; }
        public new TimeInstant TimeInstant { get; set; }
        public Energia energia { get; set; }
        public Luz luz { get; set; }
        public Temperatura temperatura { get; set; }
        public Vazao vazao { get; set; }
    }
}
