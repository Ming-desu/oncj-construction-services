namespace ONCJ_Construction_Services.Models
{
    using ONCJ_Construction_Services.Common;
    using System.Collections.Generic;

    public class Employee : Database<Employee>
    {
        public int ID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string position { get; set; }
        public string picture_url { get; set; } = "";

        public bool Create()
        {
            if (Exists())
            {
                AppController.ShowMessage("Employee already exists.", "Notification");
                return false;
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("first_name", first_name);
            parameters.Add("last_name", last_name);
            parameters.Add("position", position);
            parameters.Add("picture_url", picture_url);

            var result = Query("INSERT INTO employees ([first_name], [last_name], [position], [picture_url]) VALUES (@first_name, @last_name, @position, @picture_url)", parameters);

            return true;
        }

        public List<Employee> Read(string query = "")
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("first_name", query + "%");
            parameters.Add("last_name", query + "%");
            parameters.Add("position", query + "%");

            return Query("SELECT * FROM employees WHERE [first_name] LIKE @first_name OR [last_name] = @last_name OR [position] LIKE @position", parameters);
        }

        public Employee Find()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ID", ID);

            var results = Query("SELECT * FROM employees WHERE [ID] = @ID", parameters);

            return results == null ? null : results[0];
        }

        public bool Update()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("first_name", first_name);
            parameters.Add("last_name", last_name);
            parameters.Add("position", position);
            parameters.Add("ID", ID);

            var results = Query("SELECT * FROM employees WHERE [first_name] = @first_name AND [last_name] = @last_name AND [position] = @position AND [ID] <> @ID", parameters);

            if (results != null)
            {
                AppController.ShowMessage("Employee already exists.", "Notification");
                return false;
            }

            parameters = new Dictionary<string, object>();
            parameters.Add("first_name", first_name);
            parameters.Add("last_name", last_name);
            parameters.Add("position", position);
            parameters.Add("picture_url", picture_url);
            parameters.Add("ID", ID);

            Query("UPDATE employees SET [first_name] = @first_name, [last_name] = @last_name, [position] = @position, [picture_url] = @picture_url WHERE [ID] = @ID", parameters);
            return true;
        }

        public bool Delete()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ID", ID);

            Query("DELETE FROM employees WHERE [ID] = @ID", parameters);

            return true;
        }

        private bool Exists()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("first_name", first_name);
            parameters.Add("last_name", last_name);
            parameters.Add("position", position);

            var result = Query("SELECT * FROM employees WHERE [first_name] = @first_name AND [last_name] = @last_name AND [position] = @position", parameters);

            return result == null ? false : true;
        }
    }
}
