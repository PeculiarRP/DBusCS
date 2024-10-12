using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DBusCS.utils;
using CommunityToolkit.Mvvm.Input;

namespace DBusCS.ViewModels
{
    public class JournalPageViewModel: ViewModelBase
    {
        public delegate void AddStudentDelegate ();
        public event AddStudentDelegate OnAddStudent;

        public delegate void DeleteStudentDelegate (Student student);
        public event DeleteStudentDelegate OnDeleteStudent;

        private string _id;
        public string ID 
        {
            get => _id;
            set {
                RefreshPage();
                this.RaiseAndSetIfChanged(ref _id, value);
            }
        }

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get => _students;
            set => this.RaiseAndSetIfChanged(ref _students, value);
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set => this.RaiseAndSetIfChanged(ref _selectedStudent, value);
        }

        public ICommand AddStudentEv => new RelayCommand(_AddStudent);
        public ICommand DeleteStudentEv => new RelayCommand(_DeleteStudent); 

        public void RefreshPage()
        {
            _GetStudent();
        }

        private void _GetStudent()
        {
            var studList = Task.Run(async () => await DBus.GetSudent()).Result;
            List<Student> students = new List<Student>();
            foreach (string s in studList)
            {
                var parseStr = s.Split(':');
                students.Add(new Student(Guid.Parse(parseStr[0]), parseStr[1], parseStr[2], parseStr[3]));
            }
            Students = new ObservableCollection<Student>(students);
        }

        private void _DeleteStudent() 
        {
            if (SelectedStudent != null)
            {
                OnDeleteStudent?.Invoke(SelectedStudent);
            }
        }

        private void _AddStudent()
        {
            OnAddStudent?.Invoke();
        }

        public JournalPageViewModel() { }   

    }
}
