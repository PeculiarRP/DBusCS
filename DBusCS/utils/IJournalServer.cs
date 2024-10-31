﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus;

namespace DBusCS.utils
{
    [DBusInterface("test.dbusjava.server")]
    public interface IJournalServer: IDBusObject
    {
        Task<string> AuthUserAsync(string login, string password);
        Task<string> AddUserAsync(string login, string password, string access);

        Task<string[]> GetStudentsAsync(bool isAsc);
        Task<string[]> GetStudentBySurnameAsync(string param, bool isAsc);
        Task<string> UpdateStudentByIdAsync(string id, string name, string surname, string classStudent);
        Task DeleteStudentAsync(string id);
        Task<string> AddStudentAsync(string name, string surname, string classStudent);

        Task<string> DeleteAllGradeByStudentIdAsync(string id);
        Task<string> UpdateGradeByIdAsync(string data);

        Task<string[]> GetAllSubjectAsync(bool isAsc);
        Task<string[]> GetSubjectByParamAsync(string param, bool isAsc);
        Task<string> AddSubjectAsync(string subjectName);
        Task DeleteSubjectByIdAsync(string id);
        Task<string> UpdateSubjectByIdAsync(string id, string subjectName);

        Task terminateServerAsync();
    }
}
