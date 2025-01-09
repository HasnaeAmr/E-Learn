using Elearn.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System;

namespace Elearn.Pages.Course
{
    public class LandingModel : PageModel
    {
        public string error = "";
        public User user = new User();
        public Progress user_progress = new Progress();

        public void OnGet()
        {
            int id_user = HttpContext.Session.GetInt32("UserID") ?? throw new InvalidOperationException("UserID is not set.");

            try
            {
                // Connexion et récupération des données de progression
                string connectionString1 = "Server=localhost;Database=elearn;User=root;Password=;";
                string sql = "SELECT * FROM progress WHERE id_user=@d";

                using (MySqlConnection connection = new MySqlConnection(connectionString1))
                {
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("d", id_user);

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
                error = "There was an issue retrieving your progress.";
            }

            try
            {
                // Connexion et récupération des données de l'utilisateur
                string connectionString2 = "Server=localhost;Database=elearn;User=root;Password=;";
                string sql = "SELECT * FROM user WHERE id_user=@d";

                using (MySqlConnection connection = new MySqlConnection(connectionString2))
                {
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("d", id_user);

                        connection.Open();
                        using MySqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                user.UserName = reader.GetString("username");
                            }
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
                error = "There was an issue retrieving your user data.";
            }
        }
    }
}
