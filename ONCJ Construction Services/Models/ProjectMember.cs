namespace ONCJ_Construction_Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProjectMember : Database<ProjectMember>
    {
        public int ID { get; set; }
        public int project_id { get; set; }
        public int employee_id { get; set; }
        public double rate { get; set; }

        public bool Create()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("project_id", project_id);
            parameters.Add("employee_id", employee_id);
            parameters.Add("rate", rate);

            Query("INSERT INTO project_members ([project_id], [employee_id], [rate]) VALUES (@project_id, @employee_id, @rate)", parameters);

            return true;
        }

        public List<ProjectMember> Read()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("project_id", project_id);

            return Query("SELECT * FROM project_members WHERE [project_id] = @project_id", parameters);
        }

        public bool Delete()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("project_id", project_id);

            Query("DELETE FROM project_members WHERE [project_id] = @project_id", parameters);
            return true;
        }
    }
}
