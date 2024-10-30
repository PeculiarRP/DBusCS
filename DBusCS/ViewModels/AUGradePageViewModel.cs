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

        private List<Subject> _subjectList;

        public List<Subject> SubjectList
        {
            get => _subjectList;
            set => this.RaiseAndSetIfChanged(ref _subjectList, value);
        }

        private Student _student;
        public Student Student
        {
            get => _student;
        }

        public void UpData(Student student)
        {
            _student = student;
            getSubject();
        }

        public ICommand BackToJournal => new RelayCommand(_BackToJournal);
        public ICommand ButtonEv => new RelayCommand(_ButtonEv);

        private void _BackToJournal()
        {
            OnReturnBackToJournal?.Invoke();
        }
        private async void _ButtonEv()
        {
            string data = Student.Id.ToString() + "~";
            foreach (var subject in SubjectList)
            {
                data += subject.GetToUp() + ",";
            }
            data = data.TrimEnd(',');
            await DBus.UpdateGradeByStudent(data);
            _BackToJournal();
        }

        private void getSubject()
        {
            List<Subject> stList = new List<Subject>();
            var subjects = Task.Run(async () => await DBus.GetAllSubject(true))?.Result;
            foreach (var subject in subjects)
            {
                var subSplit = subject.Trim().Split(":");
                var grade = Student.Grades.FirstOrDefault(g => g.SubjectName.Equals(subSplit[1], StringComparison.OrdinalIgnoreCase))?.Grade;
                stList.Add(new Subject(Guid.Parse(subSplit[0]), subSplit[1], grade ?? 0));
            }
            SubjectList = stList;
        }

        public AUGradePageViewModel() { }
    }
}
