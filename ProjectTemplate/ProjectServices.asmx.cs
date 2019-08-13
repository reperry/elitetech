using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Script.Serialization;
using System.Diagnostics;


namespace ProjectTemplate
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class ProjectServices : System.Web.Services.WebService
    {
        private string dbID = "elitetech";
        private string dbPass = "!!Cis440";
        private string dbName = "elitetech";


        private string getConString()
        {
            return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass;
        }
        ////////////////////////////////////////////////////////////////////////


        // tests database connection
        [WebMethod(EnableSession = true)]
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

        // checks if the logged in user is an admin. Used for hiding elements from non-admin
        // returns 1 if admin, 0 if not, and -1 if user is not logged in
        [WebMethod(EnableSession = true)]
        public int IsAdmin() {

            try
            {
                string query = $"select IsAdmin from Users where UserID = {Session["UserId"]}";

                MySqlConnection con = new MySqlConnection(getConString());
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                if (Convert.ToInt32(table.Rows[0][0]) == 1)
                {
                    Debug.WriteLine(1);
                    return 1;
                }

                else
                {
                    Debug.WriteLine(0);
                    return 0;
                }
            }
            catch (Exception e) {
                return -1;
            }
           
        }
        
        // adds user to database
        // currently not implemented into webpage
        [WebMethod(EnableSession = true)]
        public string AddUser(string uname, string pass)
        {
            try
            {
                string query = $"insert into elitetech.Users(Username, Password) values ('{uname}', '{pass}')";

                MySqlConnection con = new MySqlConnection(getConString());

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

        // adds post with user input to database
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

        // sets is reported in database to be true for given postid
        [WebMethod(EnableSession = true)]
        public string ReportPost(string postid)
        {
            try
            {
                string query = $"UPDATE Posts SET IsReported = 1 WHERE Postid = {postid}";

                MySqlConnection con = new MySqlConnection(getConString());

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

        // sets is reported in database to be false for given postid
        [WebMethod(EnableSession = true)]
        public string ClearReport(string postid)
        {
            try
            {
                string query = $"UPDATE Posts SET IsReported = 0 WHERE Postid = {postid}";

                MySqlConnection con = new MySqlConnection(getConString());

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

        // increments likes for given postid
        [WebMethod(EnableSession = true)]
        public string LikePost(string postid)
        {
            try
            {
                string query = $"UPDATE Posts SET Likes = Likes + 1 WHERE Postid = {postid}";

                MySqlConnection con = new MySqlConnection(getConString());

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

        // removes user with given userid from database
        [WebMethod(EnableSession = true)]
        public string DeleteUser(string userid)
        {
            try
            {
                string query = $"DELETE FROM Users WHERE UserID = {userid}";

                MySqlConnection con = new MySqlConnection(getConString());

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

        // removes post with given postid from database
        [WebMethod(EnableSession = true)]
        public string DeletePost(string postid)
        {
            try
            {
                string query = $"DELETE FROM Posts WHERE PostID = {postid}";

                MySqlConnection con = new MySqlConnection(getConString());

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

        // checks database for user credentials and creates session if they exist
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

        // closes session
        [WebMethod(EnableSession = true)]
        public bool LogOff()
        {
            Session.Abandon();
            return true;
        }

        // returns array of posts for specified query
        [WebMethod]
        public Post[] GetPosts(string keyword, string category, string sortby)
        {
            try
            {
                string query = $"select Posts.PostID, Posts.Content, Posts.Likes, Posts.TimeStamp, Users.Username from Posts inner join Users on Posts.UserID = Users.UserID";

                if (keyword != "")
                {
                    query += $" where Posts.Content like '%{keyword}%'";

                    if (category != "")
                    {
                        query += $" and Posts.Category = '{category}'";
                    }
                }
                else {
                    if (category != "")
                    {
                        query += $" where Posts.Category = '{category}'";
                    }
                }

                if (sortby != "") {
                    sortby = sortby.Replace("%20", " ");
                    query += $" order by {sortby}";
                }

                Debug.WriteLine(query);

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(query, con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable("posts");
                adapter.Fill(table);

                return PackagePosts(table).ToArray();
            }
            catch (Exception e)
            {
                return new Post[0];
            }

        }

        // takes data table and combines into list of posts
        public List<Post> PackagePosts(DataTable table) {

            List<Post> posts = new List<Post>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                posts.Add(new Post
                {
                    postid = table.Rows[i][0].ToString(),
                    content = table.Rows[i][1].ToString(),
                    likes = Convert.ToInt32(table.Rows[i][2]),
                    timestamp = DateTime.Parse(table.Rows[i][3].ToString()).ToString("MMMM dd, yyyy"),
                    user = table.Rows[i][4].ToString()
                });
            }
            return posts;
        }

        // returns list of all reported posts
        [WebMethod]
        public Post[] GetReports()
        {
            try
            {
                string query = $"select Posts.PostID, Posts.Content, Posts.Likes, Posts.TimeStamp, Users.Username from Posts inner join Users on Posts.UserID = Users.UserID where IsReported ='1'";

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(query, con);


                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable("posts");
                adapter.Fill(table);

                return PackagePosts(table).ToArray();
            }


            catch (Exception e)
            {
                return new Post[0];
            }

        }

        // returns list of all users
        [WebMethod]
        public User[] GetUsers()
        {
            try
            {
                string query = $"select UserID, Username, IsAdmin from Users order by UserID";

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

        // changes isadmin to true for given user id in database
        [WebMethod(EnableSession = true)]
        public string MakeAdmin(string userid)
        {
            try
            {
                string query = $"UPDATE Users SET IsAdmin = 1 WHERE UserID = {userid}";

                MySqlConnection con = new MySqlConnection(getConString());

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
////// NOTE: We had so issues with useing the @ and parameters to add variables to queries.
////// I made sure that any post that has direct user input uses that notation
////// Otherwise, all variables are generated by functions and should give proper, preselected input
