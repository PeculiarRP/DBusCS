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
using Tmds.DBus.Protocol;

namespace DBusCS.ViewModels
{
    public class JournalPageViewModel: ViewModelBase
    {
        public delegate void Refresh(string flag);
        public event Refresh OnRefresh;

        public delegate void AddDelegate (string type);
        public event AddDelegate OnAdd;

        public delegate void DeleteDelegate (Dictionary<string, object> deleteInfo);
        public event DeleteDelegate OnDelete;

        public delegate void UpdateDelegate (Dictionary<string, object> deleteInfo);
        public event UpdateDelegate OnUpdate;

        private string _id;
        public string ID 
        {
            get => _id;
            set {
                RefreshPage();
                this.RaiseAndSetIfChanged(ref _id, value);
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
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
                if (value != "Журнал") IsNotJournal = true;
                else IsNotJournal = false;
                this.RaiseAndSetIfChanged(ref _selectedItem, value);
                RefreshPage();
            }
        }

        private bool _isNotJournal = false;
        public bool IsNotJournal
        {
            get => _isNotJournal;
            set => this.RaiseAndSetIfChanged(ref _isNotJournal, value);
        }

        private bool _isASC = true;
        public bool IsASC
        {
            get => _isASC;
            set { 
                this.RaiseAndSetIfChanged(ref _isASC, value); 
                RefreshPage();
            }
        }

        private ObservableCollection<object> _dataObjects;
        public ObservableCollection<object> DataObjects
        {
            get => _dataObjects;
            set => this.RaiseAndSetIfChanged(ref _dataObjects, value);
        }

        private object _selectedObject;
        public object SelectedObject
        {
            get => _selectedObject;
            set => this.RaiseAndSetIfChanged(ref _selectedObject, value);
        }

        public ICommand AddStudentEv => new RelayCommand(_AddEvent);
        public ICommand DeleteStudentEv => new RelayCommand(_DeleteEvent);

        public ICommand UpdateData => new RelayCommand(_UpdateData);

        public void RefreshPage()
        {
            string flag = "";
            Message = "";
            switch (SelectedItem)
            {
                case "Предметы":
                    flag = "p";
                    _GetSubject();
                    break;
                case "Журнал":
                    flag = "j";
                    _GetStudent();
                    break;
                case "Студенты":
                    flag = "s";
                    _GetStudent();
                    break;
            }
            OnRefresh?.Invoke(flag);
        }

        private void _GetStudent()
        {
            var studList = Task.Run(async () => await DBus.GetSudent(IsASC))?.Result;
            List<Student> students = new List<Student>();
            List<string> headers = new List<string>() { "Имя", "Фамилия", "Класс" };
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
                        headers.Add(parsSub[1]);
                        subjects.Add(new Subject(Guid.Parse(parsSub[0]), parsSub[1], Int32.Parse(parsSub[2])));
                    }
                }
                students.Add(new Student(Guid.Parse(studentInf[0]), studentInf[1], studentInf[2], studentInf[3], subjects));
            }
            DataObjects = new ObservableCollection<object>(students);
        }

        private void _GetSubject()
        {
            var subjectList = Task.Run(async () => await DBus.GetAllSubject(IsASC)).Result;
            List<Subject> subjects = new List<Subject>();
            List<string> headers = new List<string>() { "Предметы" };
            foreach (var subject in subjectList)
            {
                var parseSub = subject.Trim().Split(":");
                subjects.Add(new Subject(Guid.Parse(parseSub[0]), parseSub[1]));
            }
            DataObjects = new ObservableCollection<object>(subjects);
        }
        private void _EventHandl(string type)
        {
            if (SelectedObject != null)
            {
                Message = "";
                var dict = new Dictionary<string, object>();
                switch (SelectedItem)
                {
                    case "Предметы":
                        dict.Add("предмет", SelectedObject);
                        break;
                    case "Журнал":
                        dict.Add("журнал", SelectedObject);
                        break;
                    case "Студенты":
                        dict.Add("студент", SelectedObject);
                        break;
                }
                if (type == "del") OnDelete?.Invoke(dict);
                else OnUpdate?.Invoke(dict);
            }
            else Message = "Значение не выбрано!";
        }

        private void _DeleteEvent() 
        {
            _EventHandl("del");
        }

        private void _AddEvent()
        {
            OnAdd?.Invoke(SelectedItem);
        }

        private void _UpdateData()
        {
            _EventHandl("add");
        }

        public JournalPageViewModel() { }   

    }
}
