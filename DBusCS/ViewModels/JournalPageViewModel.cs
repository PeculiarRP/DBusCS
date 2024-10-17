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
using Avalonia.Media;

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

        private string[] _boxItems = {
            "Журнал",
            "Предметы",
            "Студенты"
        };

        public string[] BoxItems
        {
            get => _boxItems;
        }

        private string _selectedItem = "Журнал";
        public string SelectedItem
        {
            get => _selectedItem;
            set {
                this.RaiseAndSetIfChanged(ref _selectedItem, value);
                RefreshPage();
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

        private ObservableCollection<Subject> _subjects;
        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set => this.RaiseAndSetIfChanged(ref _subjects, value);
        }

        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set => this.RaiseAndSetIfChanged(ref _selectedSubject, value);
        }

        private bool _isJournal = true;
        public bool IsJournal
        {
            get=> _isJournal;
            set => this.RaiseAndSetIfChanged(ref _isJournal, value);
        }

        private bool _isStudent = true;
        public bool IsStudent
        {
            get => _isStudent;
            set => this.RaiseAndSetIfChanged(ref _isStudent, value);
        }

        public ICommand AddStudentEv => new RelayCommand(_AddStudent);
        public ICommand DeleteStudentEv => new RelayCommand(_DeleteStudent);

        public void RefreshPage()
        {
            switch (SelectedItem)
            {
                case "Предметы":
                    IsStudent = false;
                    _GetSubject();
                    break;
                case "Журнал":
                    IsStudent = true;
                    IsJournal = true;
                    _GetStudent();
                    break;
                case "Студенты":
                    IsStudent = true;
                    IsJournal = false;
                    _GetStudent();
                    break;
            }
        }

        private void _GetStudent()
        {
            var studList = Task.Run(async () => await DBus.GetSudent())?.Result;
            List<Student> students = new List<Student>();
            foreach (string s in studList)
            {
                var parseStr = s.Split('[');
                parseStr[0] = parseStr[0].Remove(parseStr[0].Length - 1);
                parseStr[1] = parseStr[1].Remove(parseStr[1].Length - 1);
                var studentInf = parseStr[0].Split(":");
                var gradeList = parseStr[1] != "" ? parseStr[1].Split(",") : null;
                List<Subject> subjects = new List<Subject>();
                if (gradeList != null)
                {
                    foreach (string grade in gradeList)
                    {
                        var parsSub = grade.Trim().Split(":");
                        subjects.Add(new Subject(Guid.Parse(parsSub[0]), parsSub[1], Int32.Parse(parsSub[2])));
                    }
                }
                students.Add(new Student(Guid.Parse(studentInf[0]), studentInf[1], studentInf[2], studentInf[3], subjects));
            }
            Students = new ObservableCollection<Student>(students);
        }

        private void _GetSubject()
        {
            var subjectList = Task.Run(async () => await DBus.GetAllSubject()).Result;
            List<Subject> subjects = new List<Subject>();
            foreach (var subject in subjectList)
            {
                var parseSub = subject.Trim().Split(":");
                subjects.Add(new Subject(Guid.Parse(parseSub[0]), parseSub[1]));
            }
            Subjects = new ObservableCollection<Subject>(subjects);
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
