using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPole.Model
{    
    public class PosteModel
    {
		private string id;
		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		private string descricao;
		public string Descricao
		{
			get { return String.Format("Descricao: {0}", descricao); }
			set { descricao = value; }
		}
		
		private string status;
		public string Status
		{	
			//get { return String.Format("Status: {0}",status); }
			get { return String.Format("Status: {0}",status); }
			set { status = value; }
		}
	}
}
