using ReactiveUI;
using System.ComponentModel;

namespace DBusCS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        private ViewModelBase[] pages = {
            new AuthPageViewModel()
        };
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage { 
            get => _currentPage; 
            set => this.RaiseAndSetIfChanged(ref _currentPage, value); }

        public MainWindowViewModel() {
            CurrentPage = pages[0];
        }   
    }
}
