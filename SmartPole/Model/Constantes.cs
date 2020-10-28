using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPole.Model
{
    public static class Constantes
    {
        public const string URL_HELIX = "http://143.107.145.32";
        public const string GET_ENTITIES = ":1026/v2/entities/";
        public const string URL_API = "https://smartpoleapi.azurewebsites.net";
        public const string GET_DISPOSITIVO = "/smartmeter";
        public const string GET_HISTORICO = "/smartmeter/GetHistorico";
        public const string POST_LOGIN = "/smartmeter/PostLogin";

        public enum TipoSensor
        {                       
            Energia,
            Gás,
            Luz,
            Temperatura,
            Umidade,
            Vazão
        }
    }
}
