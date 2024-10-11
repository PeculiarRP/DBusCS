﻿using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DBusCS.utils;
using CommunityToolkit.Mvvm.Input;

namespace DBusCS.ViewModels
{
    public class JournalPageViewModel: ViewModelBase
    {
        private string _id;
        public string ID 
        {
            get => _id;
            set {
                GetStudent();
                this.RaiseAndSetIfChanged(ref _id, value);
            }
        }

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get => _students;
            set => this.RaiseAndSetIfChanged(ref _students, value);
        }

        public ICommand _addStudent => new RelayCommand(AddStudent);
        public ICommand _deleteStudent => new RelayCommand(AddStudent); 

        private async void GetStudent()
        {
            var studList = Task.Run(async () => await DBus.GetSudent()).Result;
            List<Student> students = new List<Student>();
            foreach (string s in studList)
            {
                var parseStr = s.Split(':');
                students.Add(new Student(Guid.Parse(parseStr[0]), parseStr[1], parseStr[2], parseStr[3]));
            }
            Students = new ObservableCollection<Student>(students);
        }

        private async void AddStudent()
        {
            
        }

        public JournalPageViewModel() { }   

    }
}