using ReactiveUI;
using DBusCS.utils;
using System.ComponentModel;

namespace DBusCS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private void _AuthComl(string id) {
            CurrentPage = pages[1];
            ((JournalPageViewModel)pages[1]).ID = id;
        }

        private void _AddStudent()
        {
            CurrentPage = pages[2];
        }

        private void _BackToJournal() 
        {
            CurrentPage = pages[1];
            ((JournalPageViewModel)pages[1]).RefreshPage();
        }

        private void _DeleteStudent(Student student)
        {
            CurrentPage = pages[3];
            ((DeleteStudentPageViewModel)pages[3]).UploadData(student);
        }

        private ViewModelBase[] pages = {
            new AuthPageViewModel(),
            new JournalPageViewModel(),
            new AddStudentPageViewModel(),
            new DeleteStudentPageViewModel()
        };

        private ViewModelBase _currentPage;
        
        public ViewModelBase CurrentPage { 
            get => _currentPage; 
            set => this.RaiseAndSetIfChanged(ref _currentPage, value); }

        public MainWindowViewModel() {
            CurrentPage = pages[0];
            ((AuthPageViewModel)pages[0]).OnUserAuth += _AuthComl;
            ((JournalPageViewModel)pages[1]).OnAddStudent += _AddStudent;
            ((JournalPageViewModel)pages[1]).OnDeleteStudent += _DeleteStudent;
            ((AddStudentPageViewModel)pages[2]).OnReturnBackToJournal += _BackToJournal;
            ((DeleteStudentPageViewModel)pages[3]).OnReturnBackToJournal += _BackToJournal;
        }   
    }
}
