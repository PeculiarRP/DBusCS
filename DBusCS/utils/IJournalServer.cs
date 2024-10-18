using System;
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
        Task<string[]> GetStudentsAsync();
        Task<string> UpdateStudentByIdAsync();
        Task DeleteStudentAsync(string id);
        Task AddStudentAsync(string name, string surname, string classStudent);
        Task<string[]> GetAllSubjectAsync();
        Task<string> AddSubjectAsync(string subjectName);
        Task DeleteSubjectByIdAsync(string id);
        Task<string> UpdateSubjectByIdAsync(string id, string subjectName);

        Task terminateServerAsync();
    }
}
