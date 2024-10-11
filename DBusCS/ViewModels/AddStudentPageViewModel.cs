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
        public delegate void ReturnBack ();
        public event ReturnBack ReturnBackToJournal;

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
                if (value.Length == 2 && Char.IsNumber(value[0]) && Char.IsLetter(value[1])) StudentClassBrush = new SolidColorBrush(Colors.Black);
                else if (value.Length == 3 && Char.IsNumber(value[0]) && Char.IsNumber(value[1]) && Char.IsLetter(value[2])) StudentClassBrush = new SolidColorBrush(Colors.Black);
                else StudentClassBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _studentClass, value); 
            }
        }

        public ICommand AddStudentEv => new RelayCommand(addStudent);
        public ICommand ReturnBackEv => new RelayCommand(retBack);

        private void retBack()
        {
            ReturnBackToJournal?.Invoke();
        }

        private async void addStudent()
        {
            await DBus.AddStudent(Name, Surname, StudentClass);
            ReturnBackToJournal?.Invoke();
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
