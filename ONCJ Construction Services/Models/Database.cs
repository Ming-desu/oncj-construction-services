namespace ONCJ_Construction_Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Reflection;
    using ONCJ_Construction_Services.Common;

    public class Database<T> where T : class, new()
    {
        private OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.accdb");
        private OleDbCommand cmd;
        private OleDbDataReader dr;

        public List<T> Query(string query, Dictionary<string, object> parameters = null)
        {
            List<T> models = new List<T>();

            try
            {
                con.Open();

                cmd = new OleDbCommand(query, con);

                // Set the parameters available if there are any
                SetParameters(parameters);

                if (query.Split(' ')[0].ToLower() == "select")
                {
                    dr = cmd.ExecuteReader();

                    while(dr.Read())
                    {
                        T model = new T();

                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            SetProperty(dr.GetName(i), dr[i], model);
                        }

                        models.Add(model);
                    }

                    dr.Close();
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                return models.Count == 0 ? null : models;
            }
            catch (OleDbException e)
            {
                AppController.ShowMessage("Contact your system administrator, error: " + e.Message, "Error", System.Windows.Forms.MessageBoxIcon.Error);
            }

            return null;
        }

        private void SetParameters(Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count <= 0)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
        }

        /// <summary>
        /// A method that will add the value to the property of the object
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="obj">The object where to put the property</param>
        private void SetProperty(string propertyName, object value, object obj)
        {
            PropertyInfo propInfo = obj.GetType().GetProperty(propertyName);
            if (propInfo != null && propInfo.CanWrite)
                propInfo.SetValue(obj, value, null);
        }
    }
}
