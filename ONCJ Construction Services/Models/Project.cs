namespace ONCJ_Construction_Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Project : Database<Project>
    {
        public int ID { get; set; }
        public string subject { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public List<ProjectMember> members { get; set; } = new List<ProjectMember>();

        public int Create()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("subject", subject);
            parameters.Add("date_start", date_start.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("date_end", date_end.ToString("yyyy-MM-dd HH:mm:ss"));

            Query("INSERT INTO projects ([subject], [date_start], [date_end]) VALUES (@subject, @date_start, @date_end)", parameters);
            var results = Query("SELECT TOP 1 [ID] FROM projects ORDER BY ID DESC");

            return results == null ? -1 : results[0].ID;
        }

        public List<Project> Read(string query = "")
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("search", "%" + query + "%");

            return Query("SELECT * FROM projects WHERE [subject] LIKE @search", parameters);
        }

        public Project Find()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ID", ID);

            var results = Query("SELECT * FROM projects WHERE [ID] = @ID", parameters);
            return results == null ? null : results[0];
        }

        public bool Update()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("subject", subject);
            parameters.Add("date_start", date_start.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("date_end", date_end.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("ID", ID);

            Query("UPDATE projects SET [subject] = @subject, [date_start] = @date_start, [date_end] = @date_end WHERE [ID] = @ID", parameters);
            return true;
        }

        public bool Delete()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ID", ID);

            Query("DELETE FROM projects WHERE [ID] = @ID", parameters);

            ProjectMember member = new ProjectMember();
            member.project_id = ID;
            member.Delete();

            return true;
        }
    }
}
