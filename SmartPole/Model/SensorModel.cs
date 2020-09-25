using System;
using System.Collections.Generic;
using System.Text;


//Classe Sensor
namespace SmartPole.Model
{
    public class SensorModel
    {
		public string Descricao
		private string descricao;
		{
			get { return descricao; }
			set { descricao = value; }
		}
		private DateTime data;
		public DateTime Data
		{
			get { return data; }
			set { data = value; }
		}

		public string DescricaoFormatada
		{
			get
			{
				return string.Format("{0} - Data {1}", descricao, data.ToString("dd/mm/yyyy"));
			}
		}
	}
}
