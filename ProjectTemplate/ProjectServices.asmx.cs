using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Script.Serialization;

namespace ProjectTemplate
{
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

        [WebMethod(EnableSession = true)]
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

        [WebMethod(EnableSession = true)]
        public string AddPost(string content, string category, bool isAnon)
        {

            try
            {
                string uid;
                if (content == "") {
                    content = "Other";
                }
                if (isAnon)
                {
                    //anonymous userid in the database
                    uid = "6";
                }
                else
                {
                    uid = Session["UserId"].ToString();
                }

                string query = $"insert into elitetech.Posts(UserID, Content, Category, TimeStamp) values (@uid, @content, @category, CURRENT_TIMESTAMP())";

                MySqlConnection con = new MySqlConnection(getConString());
                MySqlCommand cmd = new MySqlCommand(query, con);

                cmd.Parameters.AddWithValue("@uid", HttpUtility.UrlDecode(uid));
                cmd.Parameters.AddWithValue("@content", HttpUtility.UrlDecode(content));
                cmd.Parameters.AddWithValue("@category", HttpUtility.UrlDecode(category));

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

        [WebMethod(EnableSession = true)]
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

        [WebMethod(EnableSession = true)]
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

        [WebMethod(EnableSession = true)]
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

        [WebMethod(EnableSession = true)]
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

        [WebMethod(EnableSession = true)]
        public bool LogOn(string un, string pass)
        {
            //we return this flag to tell them if they logged in or not
            bool success = false;
   
            string sqlSelect = "SELECT UserID, IsAdmin FROM elitetech.Users WHERE Username=@UsernameValue AND Password=@PasswordValue";

            MySqlConnection sqlConnection = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@UsernameValue", HttpUtility.UrlDecode(un));
            sqlCommand.Parameters.AddWithValue("@PasswordValue", HttpUtility.UrlDecode(pass));

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);
            if (sqlDt.Rows.Count > 0)
            {
                //if we found an account, store the id and admin status in the session
                //so we can check those values later on other method calls to see if they 
                //are 1) logged in at all, and 2) and admin or not
                Session["UserId"] = sqlDt.Rows[0]["UserId"];
                Session["IsAdmin"] = sqlDt.Rows[0]["IsAdmin"];
                success = true;
            }
            //return the result!
            return success;
        }

        [WebMethod(EnableSession = true)]
        public bool LogOff()
        {
            Session.Abandon();
            return true;
        }

        [WebMethod]
        public Post[] GetPosts()
        {
            try
            {
                string query = $"select Posts.PostID, Posts.Content, Posts.Likes, Posts.TimeStamp, Users.Username from Posts inner join Users on Posts.UserID = Users.UserID";

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable("posts");
                adapter.Fill(table);

                List<Post> posts = new List<Post>();
                for (int i = 0; i < table.Rows.Count; i++) {
                    posts.Add(new Post
                    {
                        postid = table.Rows[i][0].ToString(),
                        content = table.Rows[i][1].ToString(),
                        likes = Convert.ToInt32(table.Rows[i][2]),
                        timestamp = DateTime.Parse(table.Rows[i][3].ToString()).ToString("MMMM dd, yyyy"),
                        user = table.Rows[i][4].ToString()
                    });
                }
              
                return posts.ToArray();
            }
            catch (Exception e)
            {
                return new Post[0];
            }

        }

        [WebMethod]
        public User[] GetUsers()
        {
            try
            {
                string query = $"select UserID, Username, IsAdmin from Users";

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable("users");
                adapter.Fill(table);

                List<User> users = new List<User>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    users.Add(new User
                    {
                        userid = table.Rows[i][0].ToString(),
                        username = table.Rows[i][1].ToString(),
                        isadmin = Convert.ToBoolean(table.Rows[i][2])
                    });
                }

                return users.ToArray();
            }
            catch (Exception e)
            {
                return new User[0];
            }

        }

        [WebMethod(EnableSession = true)]
        public string MakeAdmin(string userid)
        {
            try
            {
                string query = $"UPDATE Users SET IsAdmin = 1 WHERE UserID = {userid}";

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
