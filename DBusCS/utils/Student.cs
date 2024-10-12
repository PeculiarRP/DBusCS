using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBusCS.utils
{
    public class Student
    {
        private Guid _id;
        private string _name;
        private string _surname;
        private string _studentClass;

        public Guid Id { 
            get=>_id; 
            set=>_id = value; 
        }
        public string Name { 
            get => _name; 
            set => _name = value; 
        }
        public string Surname {
            get => _surname; 
            set => _surname = value; 
        }
        public string StudentClass 
        { 
            get => _studentClass; 
            set => _studentClass = value; 
        }

        public override string ToString()
        {
            return Name + " " + Surname + " из класса " + StudentClass + " с номером " + Id;
        }

        public Student(Guid id, string name, string surname, string studentClass)
        {
            Id = id;
            Name = name;
            Surname = surname;
            StudentClass = studentClass;
           
        }
    }
}
