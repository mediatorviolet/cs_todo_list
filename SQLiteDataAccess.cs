using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Data.SQLite;

namespace cs_todo_list
{
    class SQLiteDataAccess
    {
        public static List<Todo> LoadToDo()
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = conn.Query<Todo>("select * from Todos order by Id desc", new DynamicParameters());
                return output.ToList();
            }
        }
        public static void SaveToDo(Todo todo)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("insert into Todos (Name, Status) values (@Name, @Status)", todo);
            }
        }
        public static void UpdateToDo(Todo todo)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("update Todos set Name = @Name, Status = @Status where Id = @Id", todo);
            }
        }
        public static void DeleteToDo(int todo)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("delete from Todos where id = @Id", new { Id = todo });
            }
        }
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
