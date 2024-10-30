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
    public class DeleteStudentPageViewModel: ViewModelBase
    {
        public delegate void ReturnBackDelegate();
        public event ReturnBackDelegate OnReturnBackToJournal;

        private string _startOfText = "Вы уверены что хотите удалить ";
        private string _stId;
        private string _typeOper;

        private string _pageText;

        public string PageText 
        {
            get => _pageText;
            set => this.RaiseAndSetIfChanged(ref _pageText, value);
        }

        public ICommand DeleteStudent => new RelayCommand(_DeleteStudent);
        public ICommand BackToJournal => new RelayCommand(_Back);

        public void UploadData(string typeOper, Student student) {
            _typeOper = typeOper;
            _stId = student.Id.ToString();
            var type = typeOper == "студент" ? "студента " : "все оценки студента ";
            PageText = _startOfText + type + student.ToString();
        }
        public void UploadData(string typeOper, Subject subject)
        {
            _typeOper = typeOper;
            _stId = subject.Id.ToString();
            PageText = _startOfText + "предмет " + subject.GetInfo();
        }

        private async void _DeleteStudent()
        {
            switch (_typeOper)
            {
                case "предмет":
                    await DBus.DeleteSubjectById(_stId);
                    break;
                case "студент":
                    await DBus.DeleteStudentById(_stId);
                    break;
                case "журнал":
                    await DBus.DeleteAllGradeByStudentId(_stId);
                    break;
            }
            _Back();
        }

        private void _Back()
        {
            OnReturnBackToJournal?.Invoke();
        }

        public DeleteStudentPageViewModel() { }
    }
}
