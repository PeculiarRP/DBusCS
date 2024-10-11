using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DBusCS.utils;
using System.Reactive;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Media;
using System.Drawing;

namespace DBusCS.ViewModels
{
    public class AuthPageViewModel : ViewModelBase
    {
        public delegate void AuthDelegate(string id);
        public event AuthDelegate userAuth;
        private string _login;
        public string Login
        {
            get => _login;
            set {
                if (value != "") LoginBrush = new SolidColorBrush(Colors.Black);
                else LoginBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _login, value);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set {
                if (value != "") PassBrush = new SolidColorBrush(Colors.Black);
                else PassBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }

        public ICommand AuthUser => new RelayCommand(authUser);

        private async void authUser()
        {
            string id = await DBus.AuthUser(Login, Password);
            if (id != "error")
            {
                userAuth?.Invoke(id);
            }
            else
            {
                Login = "";
                Password = "";
            }
        }

        private Brush _loginBrush;
        public Brush LoginBrush 
        {
            set => this.RaiseAndSetIfChanged(ref _loginBrush, value); 
            get => _loginBrush;        
        }

        private Brush _passBrush;
        public Brush PassBrush
        {
            set => this.RaiseAndSetIfChanged(ref _passBrush, value);
            get => _passBrush;
        }
        public AuthPageViewModel() {
            LoginBrush = new SolidColorBrush(Colors.Red);
            PassBrush =  new SolidColorBrush(Colors.Red);
        }
    }
}
