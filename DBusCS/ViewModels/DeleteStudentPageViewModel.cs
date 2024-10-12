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

        private string _startOfText = "Вы уверены что хотите удалить студента ";
        private string _stId;

        private string _pageText;

        public string PageText 
        {
            get => _pageText;
            set => this.RaiseAndSetIfChanged(ref _pageText, value);
        }

        public ICommand DeleteStudent => new RelayCommand(_DeleteStudent);
        public ICommand BackToJournal => new RelayCommand(_Back);

        public void UploadData(Student student) {
            _stId = student.Id.ToString();
            PageText = _startOfText + student.ToString();
        }

        private async void _DeleteStudent()
        {
            await DBus.DeleteStudentById(_stId);
            _Back();
        }

        private void _Back()
        {
            OnReturnBackToJournal?.Invoke();
        }

        public DeleteStudentPageViewModel() { }
    }
}
