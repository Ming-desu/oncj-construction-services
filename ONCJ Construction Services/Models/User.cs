namespace ONCJ_Construction_Services.Models
{
    using ONCJ_Construction_Services.Common;
    using System.Collections.Generic;
    using System.Text;

    public class User : Database<User>
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public bool Login()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("username", username);
            parameters.Add("password", CreateMD5(password));

            var result = Query("SELECT * FROM users WHERE [username] = @username AND [password] = @password", parameters);

            if (result == null)
            {
                AppController.ShowMessage("Invalid username or password.", "Notification");
                return false;
            }

            return true;
        }

        public bool Create()
        {
            if (Exists())
            {
                AppController.ShowMessage("Username already taken.", "Notification");
                return false;
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("username", username);
            parameters.Add("password", CreateMD5(password));
            parameters.Add("first_name", first_name);
            parameters.Add("last_name", last_name);

            var result = Query("INSERT INTO users ([username], [password], [first_name], [last_name]) VALUES (@username, @password, @first_name, @last_name)", parameters);

            return true;
        }

        public static bool HasUser()
        {
            User u = new User();
            var result = u.Query("SELECT TOP 1 * FROM users");

            return result == null ? false : true;
        }

        private bool Exists()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("username", username);

            var result = Query("SELECT * FROM users WHERE username = @username", parameters);

            return result == null ? false : true;
        }


        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
