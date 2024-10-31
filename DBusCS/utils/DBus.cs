using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public static async Task<string> AddUser(string login, string password, string access)
        {
            await ConnectCreate();
            string mes = await _journalServer.AddUserAsync(login, password, access);
            return mes;
        }

        public static async Task<string[]> GetSudent(bool isAsc)
        {
            await ConnectCreate();
            return await _journalServer.GetStudentsAsync(isAsc);
        }

        public static async Task<string[]> GetStudentByParam(string param, bool isAsc)
        {
            await ConnectCreate();
            return await _journalServer.GetStudentBySurnameAsync(param, isAsc);
        }

        public static async Task DeleteStudentById(string id)
        {
            await ConnectCreate();
            await _journalServer.DeleteStudentAsync(id);
        }

        public static async Task<string> AddStudent(string name, string surname, string studentClass)
        {
            await ConnectCreate();
            return await _journalServer.AddStudentAsync(name, surname, studentClass);
        }

        public static async Task<string> UpdateStudentById(string id, string name, string surname, string studentClass) {
            await ConnectCreate();
            return await _journalServer.UpdateStudentByIdAsync(id, name, surname, studentClass);
        }

        public static async Task<string[]> GetAllSubject(bool isAsc)
        {
            await ConnectCreate();
            return await _journalServer.GetAllSubjectAsync(isAsc);
        }

        public static async Task<string[]> GetSubjectByParam(string param, bool isAsc)
        {
            await ConnectCreate();
            return await _journalServer.GetSubjectByParamAsync(param, isAsc);
        }

        public static async Task<string> AddSubject(string subjectName)
        {
            await ConnectCreate();
            return await _journalServer.AddSubjectAsync(subjectName);
        }

        public static async Task DeleteSubjectById(string id)
        {
            await ConnectCreate();
            await _journalServer.DeleteSubjectByIdAsync(id);
        }

        public static async Task<string> UpdateSubject(string id, string subjectName)
        {
            await ConnectCreate();
            return await _journalServer.UpdateSubjectByIdAsync(id, subjectName);
        }

        public static async Task<string> DeleteAllGradeByStudentId(string id)
        {
            await ConnectCreate();
            return await _journalServer.DeleteAllGradeByStudentIdAsync(id);
        }

        public static async Task<string> UpdateGradeByStudent(string data)
        {
            await ConnectCreate();
            return await _journalServer.UpdateGradeByIdAsync(data);
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
