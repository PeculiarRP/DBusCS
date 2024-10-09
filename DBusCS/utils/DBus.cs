using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus;

namespace DBusCS.utils
{
    public static class DBus
    {
        private static IJournalServer _journalServer;

        public static async Task<string> AuthUser(string login, string password)
        {
            await ConnectCreate();
            string mes = await _journalServer.AuthUserAsync(login, password);
            return mes;
        }

        public static async Task<string[]> GetSudent()
        {
            await ConnectCreate();
            return await _journalServer.GetStudentsAsync();
        }

        public static async Task DeleteStudentById(string id)
        {
            await ConnectCreate();
            await _journalServer.DeleteStudentAsync(id);
        }

        public static async Task AddStudent(Student student)
        {
            await ConnectCreate();
            await _journalServer.AddStudentAsync(student.Name, student.Surname, student.studentClass);
        }

        public static async Task ConnectCreate()
        {
            if (_journalServer == null)
            {
                var conn = new Connection(Address.Session);
                await conn.ConnectAsync();

                var objectPath = new ObjectPath("/");
                var objectName = "test.dbusjava.server";
                _journalServer = conn.CreateProxy<IJournalServer>(objectName, objectPath);
            }
        }
    }
}
