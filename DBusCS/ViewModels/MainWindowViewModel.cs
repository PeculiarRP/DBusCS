using ReactiveUI;
using DBusCS.utils;
using System.ComponentModel;
using System.Collections.Generic;

namespace DBusCS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private void _BackToAuth()
        {
            CurrentPage = pages[0];
        }

        private void _RegEvent()
        {
            CurrentPage = pages[4];
        }

        private void _AuthComl(User user) {
            CurrentPage = pages[1];
            ((JournalPageViewModel)pages[1]).ID = user.ToString();
        }

        private void _AddEvent(string type)
        {
            switch (type)
            {
                case "Журнал":
                    break;
                case "Предметы":
                    CurrentPage = pages[5];
                    ((AddObjectPageViewModel)pages[5]).UpData();
                    break;
                case "Студенты":
                    CurrentPage = pages[2];
                    ((AddStudentPageViewModel)pages[2]).UpData();
                    break;
            }
        }

        private void _BackToJournal() 
        {
            CurrentPage = pages[1];
            ((JournalPageViewModel)pages[1]).RefreshPage();
        }

        private void _DeleteEvent(Dictionary<string, object> deleteInfo)
        {
            CurrentPage = pages[3];
            if (deleteInfo.ContainsKey("предмет"))
            {
                ((DeleteStudentPageViewModel)pages[3]).UploadData("предмет", (Subject)deleteInfo["предмет"]);
            }
            else if (deleteInfo.ContainsKey("журнал"))
            {

            }
            else
            {
                ((DeleteStudentPageViewModel)pages[3]).UploadData("студент", (Student)deleteInfo["студент"]);
            }
        }

        private void _UpdateEvent(Dictionary<string, object> updateInfo)
        {
            if (updateInfo.ContainsKey("предмет"))
            {
                CurrentPage = pages[5];
                ((AddObjectPageViewModel)pages[5]).UpData((Subject)updateInfo["предмет"]);
            }
            else if (updateInfo.ContainsKey("журнал"))
            {

            }
            else
            {
                CurrentPage = pages[2];
                ((AddStudentPageViewModel)pages[2]).UpData((Student)updateInfo["студент"]);

            }
        }

        private ViewModelBase[] pages = {
            new AuthPageViewModel(),
            new JournalPageViewModel(),
            new AddStudentPageViewModel(),
            new DeleteStudentPageViewModel(),
            new RegUserPageViewModel(),
            new AddObjectPageViewModel(),
        };

        private ViewModelBase _currentPage;
        
        public ViewModelBase CurrentPage { 
            get => _currentPage; 
            set => this.RaiseAndSetIfChanged(ref _currentPage, value); }

        public MainWindowViewModel() {
            CurrentPage = pages[0];
            ((AuthPageViewModel)pages[0]).OnUserAuth += _AuthComl;
            ((AuthPageViewModel)pages[0]).OnUserReg += _RegEvent;
            ((JournalPageViewModel)pages[1]).OnAdd += _AddEvent;
            ((JournalPageViewModel)pages[1]).OnDelete += _DeleteEvent;
            ((JournalPageViewModel)pages[1]).OnUpdate += _UpdateEvent;
            ((AddStudentPageViewModel)pages[2]).OnReturnBackToJournal += _BackToJournal;
            ((DeleteStudentPageViewModel)pages[3]).OnReturnBackToJournal += _BackToJournal;
            ((RegUserPageViewModel)pages[4]).OnBackAuth += _BackToAuth;
            ((AddObjectPageViewModel)pages[5]).OnReturnBackToJournal += _BackToJournal;
        }   
    }
}
