using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBusCS.utils
{
    public class User
    {
        private Guid _id;
        public Guid Id 
        {
            get => _id;
            set => _id = value;
        }

        private string _login;
        public string Login 
        {
            get => _login;
            set => _login = value;
        }

        private string _access;
        public string Access
        {
            get => _access;
            set => _access = value;
        }

        public User(Guid id, string login, string access)
        {
            Id = id;
            Login = login;
            Access = access;
        }
        public override string ToString()
        {
            return Id.ToString() + "\n" + "Логин пользователя: " + Login + " Статус пользователя: " + (Access == "t" ? "Учитель" : "Студент");
        }
    }
}
