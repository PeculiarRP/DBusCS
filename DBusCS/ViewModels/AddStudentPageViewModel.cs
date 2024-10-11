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
        private string _name;
        public string Name 
        {
            get => _name;
            set 
            {
                if (_name != "") NameBrush = new SolidColorBrush(Colors.Black);
                NameBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _name, value); 
            }
        }
        private string _surname;
        public string Surname 
        {
            get => _surname;
            set
            {
                if (_surname != "") NameBrush = new SolidColorBrush(Colors.Black);
                NameBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _surname, value);
            }
        }
        private string _studentClass;
        public string StudentClass 
        {
            get => _studentClass;
            set
            {
                if (_name != "") NameBrush = new SolidColorBrush(Colors.Black);
                else 
                NameBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _studentClass, value); 
            }
        }

        public ICommand AddStudentEv => new RelayCommand(addStudent);

        private void addStudent()
        {

            DBus.AddStudent(Name);
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
