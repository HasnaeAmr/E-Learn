using Elearn.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Elearn.Pages.Course
{
    public class TestModel : PageModel
    {

        public String Message { get; set; } = "";
        public static int Score { get; set; } = 0;
        public int score { get; set; } = 0;
        public User user = new User();
        public Progress user_progress = new Progress();
        public String UserName { get; set; } = "";
        List<String> Reponses = new List<String>() { "A bridge that allows different software systems to communicate",
                                                     "They ensure APIs are reliable, secure, and work well with other systems",
                                                     "To define what a system can do without specifying how it does it",
                                                     "They provide a clear structure to connect with other systems",
                                                     "APIs become more reliable and future-proof ",
                                                    
        };
        public double moyenne => (Reponses.Count * 10) * 0.5;

        public IActionResult OnGet()
        {

            int id_user = (int)HttpContext.Session.GetInt32("UserID");
            // Retrieve username from session
            UserName = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(UserName))
            {
                // Redirect to login page if session is empty
                return RedirectToPage("/Login");
            }
            try
            {
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
            }
            return Page();
        }

        public void OnPost()
        {

            List<String> ReponsesUser = new List<String>() {Request.Form["reponse1"],
                                                            Request.Form["reponse2"],
                                                            Request.Form["reponse3"],
                                                            Request.Form["reponse4"],
                                                            Request.Form["reponse5"]
            };


            for (int i = 0; i < Reponses.Count; i++)
            {
                if (ReponsesUser[i] == Reponses[i])
                {
                    score += 10;
                }
            }
            Score = score+10;
            if (score > moyenne)
            {
                Response.Redirect("/Course/Certificate");
            }
            else
            {
                Message = "Your Score is low !Try Again";
            }
        }
    }
}