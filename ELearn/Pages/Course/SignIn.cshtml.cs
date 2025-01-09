using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Elearn.Pages.Course
{
    public class SignInModel : PageModel
    {
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public UserInfo user { get; set; } = new UserInfo();



        public void OnPost()
        {
            string connexionString = "Server=127.0.0.1;Database=courses;User=root;Password=;";
            using (var con = new MySqlConnection(connexionString))
            {
                try
                {
                    con.Open();

                    // Recuperation des données du formulaire:
                    user.name = Request.Form["username"];
                    user.email = Request.Form["email"];
                    user.password = Request.Form["password"];

                    if (string.IsNullOrWhiteSpace(user.name) || string.IsNullOrWhiteSpace(user.email) || string.IsNullOrWhiteSpace(user.password))
                    {
                        ErrorMessage = "Tous les champs sont obligatoires.";
                        return;
                    }

                    string query = "INSERT INTO  user(username,email,password) VALUES(@username,@email,@password)";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user.name);

                        cmd.Parameters.AddWithValue("@email", user.email);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        int count = cmd.ExecuteNonQuery();

                        if (count > 0)
                        {
                            SuccessMessage = "you've been registed successfully, " + user.name + "!";
                        }
                        else
                        {
                            ErrorMessage = " Register Failled.";
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    ErrorMessage = "Erreur de connexion à la base de données : " + ex.Message;
                }
            }
        }


        public class UserInfo
        {
            public string name { get; set; } = "";
            public string email { get; set; } = "";
            public string password { get; set; } = "";
        }
    }
}