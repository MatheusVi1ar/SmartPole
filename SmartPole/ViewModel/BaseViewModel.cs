using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartPole.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
        }

        private bool aguardar { get; set; }
        public bool Aguardar
        {
            get
            {
                return aguardar;
            }
            set
            {
                aguardar = value;
                OnPropertyChanged();
                Completo = !value;
            }
        }

        private bool completo { get; set; }
        public bool Completo
        {
            get
            {
                return completo;
            }
            set
            {
                completo = value;
                OnPropertyChanged();
            }
        }
    }
}
