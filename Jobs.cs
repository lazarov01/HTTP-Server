using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace HTTP_sever
{
    class Jobs
    {
        public String[] jobnames = null;
        public String OutputAll = "";
        public String OutputNew = "";
        public String index1 = "";
        public static Byte[] d;
        
        private Jobs(String[] jobnames)
        {
            this.jobnames = jobnames;
        }
        private Jobs(String OutputAll, String OutputNew)
        {
            this.OutputAll = OutputAll;
            this.OutputNew = OutputNew;
            String index1 = "";
            JSONiser2 j = new JSONiser2();
            String jSon = j.all2();
            //Console.WriteLine(jSon);
            index1 += "<!DOCTYPE html><html lang='en'>" +
                "<title>List of Jobs from jobs.bg</title>" +
                "<meta charset='UTF - 8'>" +
                "<meta name='viewport' content='width = device - width, initial - scale = 1'>" +
                "<link rel='stylesheet' href='https://www.w3schools.com/w3css/4/w3.css'>" +
                "<link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Lato'>" +
                "<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css'>" +
                "<style>body {font-family: 'Lato', sans-serif}.mySlides {display: none}</style>" +
                "<body><div class='w3-content' style='max-width:2000px;margin-top:46px'>" +
                "<div class='w3-container w3-content w3-center w3-padding-64' style='max-width:800px' id='band'>" +
                "<h2 class='w3-wide'>All jobs</h2>";
            index1 = index1 + OutputAll;
            index1 += "</div></div>" +
                "<div class='w3-container w3-content w3-center w3-padding-64' style='max-width:800px' id='band'>" +
                "<h2 class='w3-wide'>New jobs</h2>";
            index1 = index1 + OutputNew;
            index1 += "</div>" +
                "</div>" +
                "</body>" +
                "<script>console.log('" + jSon + "');</script>" +
                "</html>";
            Byte[] b = Encoding.UTF8.GetBytes(index1);
            d = b;
        }
        public static Jobs GetJobs()
        {
            int i = 0;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://www.jobs.bg/front_job_search.php?add_sh=1&from_hp=1&categories%5B%5D=15");
            HtmlNodeCollection jobs = doc.DocumentNode.SelectNodes("//a[@class='card__title mdc-typography mdc-typography--headline6 text-overflow']");
            String[] jobName = new String[jobs.Count];
            foreach (HtmlNode item in jobs)
            {
                jobName[i] = item.InnerText;
                i++;
            }
            return new Jobs(jobName);
        }
        public static Jobs SaveJobs(String[] jobNames)
        {
            MySqlConnection cnn;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            string connStr = "server=localhost;user=root;database=jobs;port=3306;password=maznislav";
            cnn = new MySqlConnection(connStr);
            cnn.Open();
            foreach(String s in jobNames)
            {
                String sql = "INSERT INTO jobnames(jobname, jobseen) " +
                             "SELECT '" + s + "', 1 FROM DUAL " +
                             "WHERE NOT EXISTS(SELECT * FROM jobnames " +
                             "WHERE jobname = '" + s + "' AND (jobseen = 1 OR jobseen = 2))";
                MySqlCommand comd = new MySqlCommand(sql, cnn);
                adapter.InsertCommand = new MySqlCommand(sql, cnn);
                adapter.InsertCommand.ExecuteNonQuery();
                comd.Dispose();
            }
            cnn.Close();
            return null;
        }
        public void loadJobNames()
        {
            String OutputAll = "";
            String OutputNew = "";
            int i = 0;
            MySqlConnection cnn;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            string connStr = "server=localhost;user=root;database=jobs;port=3306;password=maznislav";
            cnn = new MySqlConnection(connStr);
            cnn.Open();

            String sql = "SELECT jobname FROM jobnames LIMIT 15";
            String sql1 = "SELECT jobname FROM jobnames WHERE jobseen = 1";
            MySqlCommand comd = new MySqlCommand(sql, cnn);

            adapter.InsertCommand = new MySqlCommand(sql, cnn);
            adapter.InsertCommand.ExecuteNonQuery();
            MySqlDataReader dataReader = comd.ExecuteReader();
            while (dataReader.Read())
            {
                //loadNames[i] = dataReader.GetValue(0).ToString();
                i++;
                OutputAll = OutputAll + "<p class='w3 - justify'>" + dataReader.GetValue(0) + "</p>" + "\n";
            }
            cnn.Close();

            cnn.Open();
            comd = new MySqlCommand(sql1, cnn);
            adapter = new MySqlDataAdapter();
            adapter.InsertCommand = new MySqlCommand(sql1, cnn);
            adapter.InsertCommand.ExecuteNonQuery();
            MySqlDataReader dataReader1 = comd.ExecuteReader();
            while (dataReader1.Read())
            {
                
                OutputNew = OutputNew + "<p class='w3 - justify'>" + dataReader1.GetValue(0) + "</p>" + "\n";
            }
            cnn.Close();
            new Jobs(OutputAll, OutputNew);
            OutputAll = "";
            OutputNew = "";
        }
        public static Jobs Update()
        {
            MySqlConnection conn;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            string connStr = "server=localhost;user=root;database=jobs;port=3306;password=maznislav";
            conn = new MySqlConnection(connStr);
            conn.Open();
            String sql1 = "UPDATE jobnames SET jobseen = 2 WHERE jobseen = 1;";
            MySqlCommand comd = new MySqlCommand(sql1, conn);
            adapter.InsertCommand = new MySqlCommand(sql1, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            comd.Dispose();
            conn.Close();
            return null;
            
        }
        
    }
}
