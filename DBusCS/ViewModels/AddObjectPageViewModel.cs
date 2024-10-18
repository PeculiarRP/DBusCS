using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using DBusCS.utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DBusCS.ViewModels
{
    public class AddObjectPageViewModel: ViewModelBase
    {
        public delegate void BackDelegate();
        public event BackDelegate OnReturnBackToJournal;

        private Subject _currentSubject;

        public void UpData()
        {
            ButtonLabel = "Добавить";
            SubjectName = "";
        }
        public void UpData(Subject subject)
        {
            _currentSubject = subject;
            ButtonLabel = "Изменить";
            SubjectName = subject.SubjectName;
        }

        private string _buttonLabel = "Добавить";
        public string ButtonLabel
        {
            get => _buttonLabel;
            set => this.RaiseAndSetIfChanged(ref _buttonLabel, value);
        }

        private string _subjectName;
        public string SubjectName
        {
            get => _subjectName;
            set 
            {
                if (value != "") SubjectBrush = new SolidColorBrush(Colors.Black);
                else SubjectBrush = new SolidColorBrush(Colors.Red);
                this.RaiseAndSetIfChanged(ref _subjectName, value);
            }
        }

        private Brush _subjectBrush = new SolidColorBrush(Colors.Red);
        public Brush SubjectBrush
        {
            get => _subjectBrush;
            set => this.RaiseAndSetIfChanged(ref _subjectBrush, value);
        }

        public ICommand SubjectEvent => new RelayCommand(_SubjectEvent);
        public ICommand BackToJournal => new RelayCommand(_BackToJournal);

        private void _BackToJournal()
        {
            OnReturnBackToJournal?.Invoke();
        }

        private async void _SubjectEvent()
        {
            if (SubjectName != "")
            {
                var req = _currentSubject == null ? await DBus.AddSubject(SubjectName) : await DBus.UpdateSubject(_currentSubject.Id.ToString(), SubjectName);
                if (req != "Successful")
                {
                    SubjectName = req;
                    SubjectBrush = new SolidColorBrush(Colors.Red);
                }
                else _BackToJournal();
            }
        }

        public AddObjectPageViewModel() { }
    }
}
