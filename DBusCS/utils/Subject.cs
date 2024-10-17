using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBusCS.utils
{
    public class Subject
    {
        public Guid _id;
        public string _subjectName;
        public int _grade;

        public Guid Id 
        { 
            get => _id;
            set => _id = value; 
        }
        public string SubjectName
        {
            get => _subjectName;
            set => _subjectName = value;
        }
        public int Grade
        {
            get => _grade;
            set => _grade = value;
        }

        public override string ToString()
        {
            return SubjectName + " " + Grade;
        }

        public Subject(Guid id, string name)
        {
            Id = id;
            SubjectName = name;
        }

        public Subject(Guid id, string subjectName, int grade)
        {
            Id = id;
            SubjectName = subjectName;
            Grade = grade;
        }
    }
}
