﻿using Newtonsoft.Json;
using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartPole.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        
        public UsuarioModel Usuario { get; set; }
        public string Login
        {
            get
            {
                return Usuario.Login;
            }
            set
            {
                Usuario.Login = value;
                OnPropertyChanged();
                ((Command)CmdLogin).ChangeCanExecute();
            }
        }
        public string Senha
        {
            get
            {
                return Usuario.Senha;
            }
            set
            {
                Usuario.Senha = value;
                OnPropertyChanged();
                ((Command)CmdLogin).ChangeCanExecute();
            }
        }


        public LoginViewModel()
        {
            Aguardar = false;
            Usuario = new UsuarioModel();
            CmdLogin = new Command(async () =>
            {
                Aguardar = true;
                Servico.Service service = new Servico.Service();
                if (await service.ValidaUsuario(Usuario))
                    MessagingCenter.Send<UsuarioModel>(Usuario, "SucessoLogin");
                Aguardar = false;
            }, () =>
             {
                 return !string.IsNullOrEmpty(Login)
                 && !string.IsNullOrEmpty(Senha);
             });
            CmdSobre = new Command(() =>
            {
                MessagingCenter.Send<String>(String.Empty, "Sobre");
            });
        }
        public ICommand CmdLogin { get; set; }
        public ICommand CmdSobre { get; set; }
    }
}
