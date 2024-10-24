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
    public class AUGradePageViewModel: ViewModelBase
    {
        public delegate void BackDelegate();
        public event BackDelegate OnReturnBackToJournal;

        private string _buttonLabel;
        public string ButtonLabel
        {
            get => _buttonLabel;
            set => this.RaiseAndSetIfChanged(ref _buttonLabel, value);
        }

        private List<Subject> _subjectList;

        private string _id;

        private Student _student;
        public Student Student
        {
            get => _student;
        }

        public void UpData(Student student)
        {
            _student = student;
            ButtonLabel = "Добавить";
        }

        public void UpData(String id, Student student)
        {
            _id = id;
            _student = student;
            ButtonLabel = "Изменить";
        }

        ICommand BackToJournal => new RelayCommand(_BackToJournal);
        ICommand ButtonEv => new RelayCommand(_ButtonEv);

        private void _BackToJournal()
        {
            OnReturnBackToJournal?.Invoke();
        }
        private async void _ButtonEv()
        {

        }

        private async void getSubject()
        {
            var subjects = await DBus.GetAllSubject();

        }

        public AUGradePageViewModel() { }
    }
}
