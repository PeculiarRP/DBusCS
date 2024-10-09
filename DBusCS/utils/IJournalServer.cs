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
        Task<string[]> GetStudentsAsync();
        Task DeleteStudentAsync(string id);
        Task AddStudentAsync(string name, string surname, string classStudent);
        Task terminateServerAsync();
    }
}
