using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.TransferObjects;

namespace TaskScheduler.DataSource
{
    internal class TaskDaoImpl : ITaskDao
    {
        public void CreateTask(TaskDTO task)
        {
            SqlConnection conn = null;
            string queryString = null;
            try
            {
                DataSource ds = new DataSource();
                using (conn = ds.CreateConnection())
                {
                    queryString = "INSERT INTO [task] (name, type, due, status, notes) " +
                        "VALUES (@Name, @Type, @Due, @Status, @Notes)";

                    SqlCommand command = new SqlCommand(queryString, conn);

                    command.Parameters.AddWithValue("@Name", task.Name);
                    command.Parameters.AddWithValue("@Type", task.Type);
                    command.Parameters.AddWithValue("@Due", task.Due.HasValue ? task.Due : DBNull.Value);
                    command.Parameters.AddWithValue("@Status", task.Status != null ? task.Status : DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", task.Notes != null ? task.Notes : DBNull.Value);

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SqlException occurred: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }

        public void DeleteTask(TaskDTO task)
        {
            SqlConnection conn = null;
            string queryString = null;
            try
            {
                DataSource ds = new DataSource();
                using (conn = ds.CreateConnection())
                {
                    queryString = "DELETE FROM [task] WHERE id = @Id";
                    SqlCommand command = new SqlCommand(queryString, conn);

                    command.Parameters.AddWithValue("@Id", task.Id);

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SqlException occurred: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }

        public List<TaskDTO> GetAllTasks()
        {
            List<TaskDTO> tasks = new List<TaskDTO>();

            SqlConnection conn = null;
            string queryString = null;

            try
            {
                DataSource ds = new DataSource();
                using (conn = ds.CreateConnection())
                {
                    queryString = "SELECT id, name, type, due, status, notes " +
                                  "FROM [task] " +
                                  "ORDER BY status, due";
                    SqlCommand command = new SqlCommand(queryString, conn);

                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskDTO task = new TaskDTO();
                            task.Id = Convert.ToInt32(reader["id"]);
                            task.Name = Convert.IsDBNull(reader["name"]) ? string.Empty : Convert.ToString(reader["name"]);
                            task.Type = Convert.IsDBNull(reader["type"]) ? string.Empty : Convert.ToString(reader["type"]);
                            task.Due = Convert.IsDBNull(reader["due"]) ? null : Convert.ToDateTime(reader["due"]);
                            task.Status = Convert.IsDBNull(reader["status"]) ? string.Empty : Convert.ToString(reader["status"]);
                            task.Notes = Convert.IsDBNull(reader["notes"]) ? string.Empty : Convert.ToString(reader["notes"]);

                            tasks.Add(task);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SqlException occurred: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }

            return tasks;
        }


        public TaskDTO GetTaskById(int id)
        {
            SqlConnection conn = null;
            string queryString = null;
            TaskDTO task = new TaskDTO();
            try
            {
                DataSource ds = new DataSource();
                using (conn = ds.CreateConnection())
                {
                    queryString = "SELECT name, type, due, status, notes " +
                          "FROM [task] " +
                          "WHERE id = @Id";
                    SqlCommand command = new SqlCommand(queryString, conn);
                    command.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            task.Name = Convert.IsDBNull(reader["name"]) ? string.Empty : Convert.ToString(reader["name"]);
                            task.Type = Convert.IsDBNull(reader["type"]) ? string.Empty : Convert.ToString(reader["type"]);
                            task.Due = Convert.IsDBNull(reader["due"]) ? null : Convert.ToDateTime(reader["due"]);
                            task.Status = Convert.IsDBNull(reader["status"]) ? string.Empty : Convert.ToString(reader["status"]);
                            task.Notes = Convert.IsDBNull(reader["notes"]) ? string.Empty : Convert.ToString(reader["notes"]);

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SqlException occurred: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }

            return task;
        }

        public void UpdateTask(TaskDTO task)
        {
            SqlConnection conn = null;
            string queryString = null;
            try
            {
                DataSource ds = new DataSource();
                using (conn = ds.CreateConnection())
                {

                    queryString = "UPDATE [task] SET name = @Name, type = @Type, due = @Due, status = @Status, notes = @Notes " +
                                "WHERE id = @Id";
                    SqlCommand command = new SqlCommand(queryString, conn);

                    command.Parameters.AddWithValue("@Name", task.Name);
                    command.Parameters.AddWithValue("@Type", task.Type);
                    command.Parameters.AddWithValue("@Due", task.Due.HasValue ? task.Due : DBNull.Value);
                    command.Parameters.AddWithValue("@Status", task.Status != null ? task.Status : DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", task.Notes != null ? task.Notes : DBNull.Value);
                    command.Parameters.AddWithValue("@Id", task.Id);

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SqlException occurred: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }

    }

}
