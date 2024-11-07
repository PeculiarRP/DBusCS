using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using DBusCS.utils;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Media;
using Tmds.DBus.Protocol;
using DynamicData.Binding;

namespace DBusCS.ViewModels
{
    public class JournalPageViewModel: ViewModelBase
    {
        private static string[] _teacher = {
            "Журнал",
            "Предметы",
            "Студенты"
        };
        private static string[] _student = { "Журнал" };

        private ObservableCollection<string> _headerList  = new ObservableCollection<string>();
        private ObservableCollection<string> _comporList = new ObservableCollection<string> {
            "=",
            "!=",
            "<",
            ">"
        };

        private string _selHeader = "";
        public string SelHeader
        {
            get => _selHeader;
            set => this.RaiseAndSetIfChanged(ref _selHeader, value);
        }

        private string _selCompor = "=";
        public string SelCompor
        {
            get => _selCompor;
            set => this.RaiseAndSetIfChanged(ref _selCompor, value);
        }

        public ObservableCollection<string> HeaderList
        {
            get => _headerList;
            set => this.RaiseAndSetIfChanged(ref _headerList, value);
        }
        public ObservableCollection<string> ComporList
        {
            get => _comporList;
        }


        public delegate void Refresh(string flag);
        public event Refresh OnRefresh;

        public delegate void AddDelegate (string type);
        public event AddDelegate OnAdd;

        public delegate void DeleteDelegate (Dictionary<string, object> deleteInfo);
        public event DeleteDelegate OnDelete;

        public delegate void UpdateDelegate (Dictionary<string, object> deleteInfo);
        public event UpdateDelegate OnUpdate;

        private ObservableCollection<string> _filterList = new ObservableCollection<string>();
        public ObservableCollection<string> FilterList
        {
            get => _filterList;
            set => this.RaiseAndSetIfChanged(ref _filterList, value);
        }

        private string _selFilter;
        public string SelFilter
        {
            get => _selFilter;
            set => this.RaiseAndSetIfChanged(ref _selFilter, value);
        }

        private string _id;
        public string ID 
        {
            get => _id;
            set {
                RefreshPage();
                this.RaiseAndSetIfChanged(ref _id, value);
                if (value.IndexOf("Студент") != -1)
                {
                    BoxItems = _student;
                    IsTeacher = false;
                }
                else
                {
                    BoxItems = _teacher;
                    IsTeacher = true;
                }
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        private string[] _boxItems = _teacher;

        public string[] BoxItems
        {
            get => _boxItems;
            set => this.RaiseAndSetIfChanged(ref _boxItems, value);
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

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        private int _filterText;
        public int FilterText
        {
            get => _filterText;
            set => this.RaiseAndSetIfChanged(ref _filterText, value);
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

        private bool _isTeacher = true;
        public bool IsTeacher
        {
            get => _isTeacher;
            set => this.RaiseAndSetIfChanged(ref _isTeacher, value);
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
        public ICommand SearchData => new RelayCommand(_SearchData);
        public ICommand AddFilter => new RelayCommand(_AddFilter);
        public ICommand DelFilter => new RelayCommand(_DelFilter);
        public ICommand DelAllFilter => new RelayCommand(_DelAllFilter);

        private void _DelFilter()
        {
            if (SelFilter != null)
            {
                Message = "";
                FilterList.Remove(SelFilter);
                FilterList = new ObservableCollection<string>(FilterList);
            }
            else Message = "Фильтр не выбран!";
        }

        private void _DelAllFilter()
        {
            FilterList = new ObservableCollection<string>();
        }

        private void _AddFilter()
        {
            var filterList = FilterList;
            var filterStr = SelHeader + " " + SelCompor + " " + FilterText.ToString();
            bool CrF = false;
            foreach(var str in filterList)
            {
                var strSplit = str.Split();
                if (SelHeader == strSplit[0])
                {
                    CrF = true;
                    break;
                }
            }
            if (!CrF)
            {
                filterList.Add(filterStr);
                FilterList = new ObservableCollection<string>(filterList);
            }
        }

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

        private void _GetStudent(string[] data = null)
        {
            var studList = data == null ? Task.Run(async () => await DBus.GetSudent(IsASC))?.Result : data;
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
                    subjects.Sort((sub1, sub2) => sub1.SubjectName.CompareTo(sub2.SubjectName));
                }
                students.Add(new Student(Guid.Parse(studentInf[0]), studentInf[1], studentInf[2], studentInf[3], subjects));
            }
            DataObjects = new ObservableCollection<object>(students);
        }

        private void _GetSubject(string[] data = null)
        {
            var subjectList = data == null ? Task.Run(async () => await DBus.GetAllSubject(IsASC)).Result : data;
            List<Subject> subjects = new List<Subject>();
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

        private void _SearchData()
        {
            if (SelectedItem != "Предметы") {
                var paramStr = SearchText.Trim();
                if (FilterList.Count != 0)
                {
                    foreach(string filter in FilterList){
                        paramStr += ":" + filter;
                    }
                }
                _GetStudent(Task.Run(async () => await DBus.GetStudentByParam(paramStr, IsASC)).Result);
            }
            else _GetSubject(Task.Run(async () => await DBus.GetSubjectByParam(SearchText.Trim(), IsASC)).Result);
        }

        public JournalPageViewModel() { }   

    }
}
