using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace cs_todo_list
{
    internal class Todo
    {
        public string Name { get; set; }
        public bool Done { get; set; }
        public int Id { get; set; }

        private int ExecuteWrite(string query, Dictionary<string, object> args)
        {
            int numberOfRowsAffected;

            // setup the connection to the db
            using (var con = new SQLiteConnection("Data source=test.db"))
            {
                con.Open();

                // open a new command
                using (var cmd = new SQLiteCommand(query, con))
                {
                    // set the argument given in the query
                    foreach (var pair in args)
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    // execute the query and get the number of rows affected
                    numberOfRowsAffected = cmd.ExecuteNonQuery();
                }
                return numberOfRowsAffected;
            }
        }

        private DataTable ExecuteRead(string query, Dictionary<string, object> args)
        {
            if (string.IsNullOrEmpty(query.Trim()))
            {
                return null;
            }

            using (var con = new SQLiteConnection("Data source=test.db"))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(query, con))
                {
                    foreach (KeyValuePair<string, object> entry in args)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }

                    var da = new SQLiteDataAdapter(cmd);

                    var dt = new DataTable();
                    da.Fill(dt);

                    da.Dispose();

                    return dt;
                }
            }
        }

        private int AddTodo(Todo todo)
        {
            const string query = "INSERT INTO Todo(Name, Done) VALUES (@name, @done)";

            var args = new Dictionary<string, object>
            {
                {"@name", todo.Name },
                {"@done", todo.Done}
            };

            return ExecuteWrite(query, args);
        }

        private int EditTodo(Todo todo)
        {
            const string query = "UPDATE Todo SET Name = @name, Done = @done WHERE Id = @id";

            var args = new Dictionary<string, object>
            {
                {"@id", todo.Id },
                {"@name", todo.Name},
                {"@done", todo.Done }
            };

            return ExecuteWrite(query, args);
        }

        private int DeleteTodo(Todo todo)
        {
            const string query = "DELETE FROM Todo WHERE Id = @id";

            var args = new Dictionary<string, object>
            {
                {"@id", todo.Id }
            };

            return ExecuteWrite(query, args);
        }

        private Todo GetTodoById(int id)
        {
            const string query = "SELECT Id, Name, Done FROM Todo WHERE Id = @id";

            var args = new Dictionary<string, object>
            {
                {"@id", id},
            };

            DataTable dt = ExecuteRead(query, args);

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var todo = new Todo
            {
                Id = Convert.ToInt32(dt.Rows[0]["ID"]),
                Name = Convert.ToString(dt.Rows[0]["Name"]),
                Done = Convert.ToBoolean(dt.Rows[0]["Done"])
            };

            return todo;
        }

        private List<Todo> GetAllTodos()
        {
            const string query = "SELECT ID, Name, Done FROM Todo";

            var args = new Dictionary<string, object>();

            DataTable dt = ExecuteRead(query, args);

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var todo = new Todo
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Name = Convert.ToString(dt.Rows[0]["Name"]),
                Done = Convert.ToBoolean(dt.Rows[0]["Done"])
            };

            return List<todo>
        }
    }
}
