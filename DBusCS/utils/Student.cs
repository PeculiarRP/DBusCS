using Avalonia.Controls;
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
        private List<Subject> _grades;

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

        public List<Subject> Grades
        {
            get => _grades;
            set => _grades = value;
        }

        public string GradeToString
        {
            get => _gradeToString();
        }

        private string _gradeToString()
        {
            string res = "";
            foreach (var grade in Grades) {
                res += grade.ToString() + '\n';
            }
            res = res != "" ? res.Remove(res.Length - 1) : "";
            return res;
        }

        public override string ToString()
        {
            return Name + " " + Surname + " из класса " + StudentClass + " с номером " + Id;
        }

        public Student(Guid id, string name, string surname, string studentClass, List<Subject> subjects)
        {
            Id = id;
            Name = name;
            Surname = surname;
            StudentClass = studentClass;
            Grades = subjects;        
        }
    }
}
