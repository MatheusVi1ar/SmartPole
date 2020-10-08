using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPole.Model
{

public class Email : JSON
    {
        public string type;
        public string value;
        public Metadata metadata;
    }

    public class Login : JSON
    {
        public string type;
        public string value;
        public Metadata metadata;
    }

    public class Nome : JSON
    {
        public string type;
        public string value;
        public Metadata metadata;
    }

    public class Senha : JSON
    {
        public string type;
        public string value;
        public Metadata metadata;
    }

    public class UsuarioJson
    {
        public string id;
        public string type;
        public Email email;
        public Login login;
        public Nome nome;
        public Senha senha;
    }
}
