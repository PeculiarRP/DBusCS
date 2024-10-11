using ReactiveUI;
using System.ComponentModel;

namespace DBusCS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private void auth_coml(string id) {
            CurrentPage = pages[1];
            ((JournalPageViewModel)pages[1]).ID = id;
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
            ((AuthPageViewModel)pages[0]).userAuth += auth_coml;
        }   
    }
}
