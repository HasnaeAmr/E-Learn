using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Elearn.Model;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
namespace Elearn.Pages.Course
{
    public class loginModel : PageModel
    {
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public User user = new User();
        public  Progress user_progress = new Progress();
        public void onGet()
        {

        }
        public void OnPost()
        {
            string connexionString = "Server=127.0.0.1;Database=elearn;User=root;Password=;";
            using (var con = new MySqlConnection(connexionString))
            {
                try
                {
                    con.Open();


                    user.UserName = Request.Form["username"];
                    user.Password = Request.Form["password"];

                    if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
                    {
                        ErrorMessage = "All Fields are Required.";
                        return;
                    }


                    string query = "SELECT id_user, username FROM user WHERE username = @username AND password = @password LIMIT 1";
                    using (var cmd = new MySqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@username", user.UserName);
                        cmd.Parameters.AddWithValue("@password", user.Password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int userId = reader.GetInt32("id_user");
                                HttpContext.Session.SetInt32("UserID", userId);
                                HttpContext.Session.SetString("Username", reader["username"].ToString());
                                Console.WriteLine($"Retrieved user ID: {user.UserName}");
                                SuccessMessage = "Hello, " + user.UserName + "!";
                               
                            }
                            else
                            {
                                ErrorMessage = "User not found or incorrect password!";
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    ErrorMessage = "Database connection error: " + ex.Message;
                }
            }
            try
            {
                string connectionString1 = "Server=localhost;Database=elearn;User=root;Password=;";
                string sql = "SELECT * FROM progress WHERE id_user=@d";

                using (MySqlConnection connection = new MySqlConnection(connectionString1))
                {
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("d", user.id_user);

                        connection.Open();
                        using MySqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                user_progress.module1 = reader.GetBoolean("module1");
                                user_progress.module2 = reader.GetBoolean("module2");
                                user_progress.module3 = reader.GetBoolean("module3");
                                user_progress.module4 = reader.GetBoolean("module4");
                                user_progress.module5 = reader.GetBoolean("module5");
                                user_progress.module6 = reader.GetBoolean("module6");
                                user_progress.introduction = reader.GetBoolean("introduction");
                                user_progress.conclusion = reader.GetBoolean("conclusion");
                                user_progress.certified = reader.GetBoolean("certified");

                               
                            }
                            Response.Redirect("/Course/Landing");
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

    }
}