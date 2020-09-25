using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPole.ViewModel
{
    public class GeralViewModel
    {
        public List<string> Regioes { get; set; }        
        public GeralViewModel()
        {
            Regioes = new List<string>()
            {
                "Santo André",
                "São Bernardo",
                "São Caetano",
                "Diadema",
                "Mauá",
                "Riberão Pires"
            };
        }
    }
}
