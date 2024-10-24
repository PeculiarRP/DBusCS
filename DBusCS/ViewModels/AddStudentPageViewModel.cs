using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DBusCS.utils;

namespace DBusCS.ViewModels
{
    public class AddStudentPageViewModel: ViewModelBase
    {
        public delegate void ReturnBackDelegate ();
        public event ReturnBackDelegate OnReturnBackToJournal;

        private Student _student;

        public void UpData()
        {
            _student = null;
            ButtonLabel = "Добавить";
            Name = "";
            Surname = "";
            StudentClass = "";
        }

        public void UpData(Student student)
        {
            ButtonLabel = "Изменить";
            _student = student;
            Name = student.Name;
            Surname = student.Surname;
            StudentClass = student.StudentClass;
        }

        private string _buttonLabel;
        public string ButtonLabel
        {
            get => _buttonLabel;
            set => this.RaiseAndSetIfChanged(ref _buttonLabel, value);
        }

        private string _name;
        public string Name 
        {
            get => _name;
            set 
            {
                if (value != "") NameBrush = new SolidColorBrush(Colors.Black);
                else NameBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _name, value); 
            }
        }
        private string _surname;
        public string Surname 
        {
            get => _surname;
            set
            {
                if (value != "") SurnameBrush = new SolidColorBrush(Colors.Black);
                else SurnameBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _surname, value);
            }
        }
        private string _studentClass;
        public string StudentClass 
        {
            get => _studentClass;
            set
            {
                if (_RightClass(value)) StudentClassBrush = new SolidColorBrush(Colors.Black);
                else StudentClassBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _studentClass, value); 
            }
        }

        public ICommand AddStudentEv => new RelayCommand(_StudentEvent);
        public ICommand ReturnBackEv => new RelayCommand(_RetBack);

        private bool _RightClass(string value)
        {
            if (value == null) return false;
            else if (value.Length == 2 && Char.IsNumber(value[0]) && Char.IsLetter(value[1])) return true;
            else if (value.Length == 3 && Char.IsNumber(value[0]) && Char.IsNumber(value[1]) && Char.IsLetter(value[2])) return true;
            else return false;
        }

        private void _RetBack()
        {
            OnReturnBackToJournal?.Invoke();
        }

        private async void _StudentEvent()
        {
            if (Name != "" && Surname != "" && _RightClass(StudentClass))
            {
                var req = _student == null ? await DBus.AddStudent(Name, Surname, StudentClass) : await DBus.UpdateStudentById(_student.Id.ToString(), Name, Surname, StudentClass);
                if (req != "Successful") {
                    Name = req;
                    NameBrush = new SolidColorBrush(Colors.Red);
                }
                else _RetBack();
            }
            
        }

        private Brush _nameBrush = new SolidColorBrush(Colors.Red);
        private Brush _surnameBrush = new SolidColorBrush(Colors.Red);
        private Brush _studentClassBrush = new SolidColorBrush(Colors.Red);

        public Brush NameBrush
        {
            get => _nameBrush;
            set => this.RaiseAndSetIfChanged(ref _nameBrush, value);
        }

        public Brush SurnameBrush
        {
            get => _surnameBrush;
            set => this.RaiseAndSetIfChanged(ref _surnameBrush, value);
        }

        public Brush StudentClassBrush
        {
            get => _studentClassBrush;
            set => this.RaiseAndSetIfChanged(ref _studentClassBrush, value);
        }

        public AddStudentPageViewModel() { }
    }
}
