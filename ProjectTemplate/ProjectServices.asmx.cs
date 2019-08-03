using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProjectTemplate
{
    /////////////////////////////HEAD
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class ProjectServices : System.Web.Services.WebService
    {
        ////////////////////////////////////////////////////////////////////////
        ///replace the values of these variables with your database credentials
        ////////////////////////////////////////////////////////////////////////
        private string dbID = "elitetech";
        private string dbPass = "!!Cis440";
        private string dbName = "elitetech";
        ////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////
        ///call this method anywhere that you need the connection string!
        ////////////////////////////////////////////////////////////////////////
        private string getConString()
        {
            return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass;
        }
        ////////////////////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////////////////////
        //don't forget to include this decoration above each method that you want
        //to be exposed as a web service!
        [WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        public string TestConnection()
        {
            try
            {
                string testQuery = "select * from test";

                ////////////////////////////////////////////////////////////////////////
                ///here's an example of using the getConString method!
                ////////////////////////////////////////////////////////////////////////
                MySqlConnection con = new MySqlConnection(getConString());
                ////////////////////////////////////////////////////////////////////////

                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return "Success!";
            }
            catch (Exception e)
            {
                return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
            }
        }

        [WebService(Namespace = "http://tempuri.org/")]
        [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
        [System.ComponentModel.ToolboxItem(false)]
        [System.Web.Script.Services.ScriptService]

        public class ProjectServices : System.Web.Services.WebService
        {
            ////////////////////////////////////////////////////////////////////////
            ///replace the values of these variables with your database credentials
            ////////////////////////////////////////////////////////////////////////
            private string dbID = "elitetech";
            private string dbPass = "!!Cis440";
            private string dbName = "elitetech";
            ////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////
            ///call this method anywhere that you need the connection string!
            ////////////////////////////////////////////////////////////////////////
            private string getConString()
            {
                return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass;
            }
            ////////////////////////////////////////////////////////////////////////



            /////////////////////////////////////////////////////////////////////////
            //don't forget to include this decoration above each method that you want
            //to be exposed as a web service!
            [WebMethod(EnableSession = true)]
            /////////////////////////////////////////////////////////////////////////
            public string TestConnection()
            {
                try
                {
                    string testQuery = "select * from Users";

                    ////////////////////////////////////////////////////////////////////////
                    ///here's an example of using the getConString method!
                    ////////////////////////////////////////////////////////////////////////
                    MySqlConnection con = new MySqlConnection(getConString());
                    ////////////////////////////////////////////////////////////////////////

                    MySqlCommand cmd = new MySqlCommand(testQuery, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return "Success!";
                }
                catch (Exception e)
                {
                    return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
                }
            }

            [WebMethod]
            public string AddUser(string uname, string pass)
            {
                try
                {
                    string query = $"insert into elitetech.Users(Username, Password) values ('{uname}', '{pass}')";

                    ////////////////////////////////////////////////////////////////////////
                    ///here's an example of using the getConString method!
                    ////////////////////////////////////////////////////////////////////////
                    MySqlConnection con = new MySqlConnection(getConString());
                    ////////////////////////////////////////////////////////////////////////

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return "Success!";
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }

            [WebMethod]
            public string AddPost(string content, string category, string uid)
            {
                try
                {
                    string query = $"insert into elitetech.Posts(UserID, Content, Category, TimeStamp) values ('{uid}', '{content}', '{category}', CURRENT_TIMESTAMP())";

                    ////////////////////////////////////////////////////////////////////////
                    ///here's an example of using the getConString method!
                    ////////////////////////////////////////////////////////////////////////
                    MySqlConnection con = new MySqlConnection(getConString());
                    ////////////////////////////////////////////////////////////////////////

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return "Success!";
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }

            [WebMethod]
            public string ReportPost(string postid)
            {
                try
                {
                    string query = $"UPDATE Posts SET IsReported = 1 WHERE Postid = {postid}";

                    ////////////////////////////////////////////////////////////////////////
                    ///here's an example of using the getConString method!
                    ////////////////////////////////////////////////////////////////////////
                    MySqlConnection con = new MySqlConnection(getConString());
                    ////////////////////////////////////////////////////////////////////////

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return "Success!";
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }

            [WebMethod]
            public string LikePost(string postid)
            {
                try
                {
                    string query = $"UPDATE Posts SET Likes = Likes + 1 WHERE Postid = {postid}";

                    ////////////////////////////////////////////////////////////////////////
                    ///here's an example of using the getConString method!
                    ////////////////////////////////////////////////////////////////////////
                    MySqlConnection con = new MySqlConnection(getConString());
                    ////////////////////////////////////////////////////////////////////////

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return "Success!";
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }


            [WebMethod]
            public string DeleteUser(string userid)
            {
                try
                {
                    string query = $"DELETE FROM Users WHERE UserID = {userid}";

                    ////////////////////////////////////////////////////////////////////////
                    ///here's an example of using the getConString method!
                    ////////////////////////////////////////////////////////////////////////
                    MySqlConnection con = new MySqlConnection(getConString());
                    ////////////////////////////////////////////////////////////////////////

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return "Success!";
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }

            [WebMethod]
            public string DeletePost(string postid)
            {
                try
                {
                    string query = $"DELETE FROM Posts WHERE PostID = {postid}";

                    ////////////////////////////////////////////////////////////////////////
                    ///here's an example of using the getConString method!
                    ////////////////////////////////////////////////////////////////////////
                    MySqlConnection con = new MySqlConnection(getConString());
                    ////////////////////////////////////////////////////////////////////////

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return "Success!";
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }
        }
    }
}
