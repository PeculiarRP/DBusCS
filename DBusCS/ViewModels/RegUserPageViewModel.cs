using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using DBusCS.utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DBusCS.ViewModels
{
    public class RegUserPageViewModel: ViewModelBase
    {
        public delegate void BackAuthDelegate();
        public event BackAuthDelegate OnBackAuth;

        private Brush _userNameBrush = new SolidColorBrush(Colors.Red);
        public Brush UserNameBrush
        {
            get => _userNameBrush;
            set => this.RaiseAndSetIfChanged(ref _userNameBrush, value);
        }
        
        private Brush _userPasswordBrush = new SolidColorBrush(Colors.Red);
        public Brush UserPasswordBrush
        {
            get => _userPasswordBrush;
            set => this.RaiseAndSetIfChanged(ref _userPasswordBrush, value);
        }

        private bool _isTeacher = true;
        public bool IsTeacher
        {
            get => _isTeacher;
            set => this.RaiseAndSetIfChanged(ref _isTeacher, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set 
            {
                if (value != "") UserNameBrush = new SolidColorBrush(Colors.Black);
                else UserNameBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _userName, value); 
            }
        }

        private string _userPassword;
        public string UserPassword
        {
            get => _userPassword;
            set
            {
                if (value != "") UserPasswordBrush = new SolidColorBrush(Colors.Black);
                else UserPasswordBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _userPassword, value);
            }
        }

        public ICommand BackToAuth => new RelayCommand(_BackToAuth);
        public ICommand RegUser => new RelayCommand(_RegUser);

        private async void _RegUser()
        {
            string res = "";
            if ((UserName != "") && (UserPassword != ""))
            {
                string access = IsTeacher ? "t" : "s";
                res = await DBus.AddUser(UserName, UserPassword, access);
            }
            if (res != "Successful")
            {
                UserName = res;
                UserNameBrush = new SolidColorBrush(Colors.Red);
            }
            else _BackToAuth();
        }
        private void _BackToAuth()
        {
            OnBackAuth?.Invoke();
        }

        public RegUserPageViewModel() { }
    }
}
