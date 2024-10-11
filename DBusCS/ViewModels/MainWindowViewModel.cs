using ReactiveUI;
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

        private ViewModelBase[] pages = {
            new AuthPageViewModel(),
            new JournalPageViewModel(),
            new AddStudentPageViewModel()
        };

        private ViewModelBase _currentPage;
        
        public ViewModelBase CurrentPage { 
            get => _currentPage; 
            set => this.RaiseAndSetIfChanged(ref _currentPage, value); }

        public MainWindowViewModel() {
            CurrentPage = pages[0];
            ((AuthPageViewModel)pages[0]).userAuth += _AuthComl;
            ((JournalPageViewModel)pages[1]).OnAddStudent += _AddStudent;
            ((AddStudentPageViewModel)pages[2]).ReturnBackToJournal += _BackToJournal;
        }   
    }
}
