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
        public delegate void AuthDelegate(User user);
        public event AuthDelegate OnUserAuth;

        public delegate void RegDelegate();
        public event RegDelegate OnUserReg;

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

        public ICommand AuthUser => new RelayCommand(_AuthUser);
        public ICommand RegUser => new RelayCommand(_RegUser);

        private async void _AuthUser()
        {
            string req = await DBus.AuthUser(Login, Password);
            if (req != "error")
            {
                var splitReq = req.Split(":");
                User user = new User(Guid.Parse(splitReq[0]), splitReq[1], splitReq[2]);
                OnUserAuth?.Invoke(user);
            }
            else
            {
                Login = "";
                Password = "";
            }
        }

        private void _RegUser()
        {
            OnUserReg?.Invoke();
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
