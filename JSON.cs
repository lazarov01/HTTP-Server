using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_sever
{
    public class PagesJSON
    {
        public string template;
        public List<PageRowDTO> pages;

        public PagesJSON()
        {
            template = "";
            pages = new List<PageRowDTO>();
        }
    }
    public class PageRowDTO
    {
        public int jid;
        public string jobname;
        public int jobseen;

        public PageRowDTO()
        {
            jid = 0;
            jobname = "";
            jobseen = 0;
        }
    }
    class JSONiser2
    {
        public JSONiser2()
        {
            all2();
        }
        public String all2()
        {

            string query = "SELECT * FROM jobnames LIMIT 15;";
            string config = "server=localhost;user=root;database=jobs;port=3306;password=maznislav";

            MySqlConnection connection = new MySqlConnection(config);
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader Reader = command.ExecuteReader();

            PagesJSON pj = new PagesJSON();
            pj.template = "template.html";

            while (Reader.Read())
            {
                PageRowDTO pr = new PageRowDTO();
                pr.jid = Int32.Parse(Reader[0].ToString());
                pr.jobname = Reader[1].ToString();
                pr.jobseen = Int32.Parse(Reader[2].ToString());

                pj.pages.Add(pr);

            }
            connection.Close();
            return JsonConvert.SerializeObject(pj);
        }
    }
}
