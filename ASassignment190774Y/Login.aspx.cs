using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;


namespace ASassignment190774Y
{
    public partial class Login : System.Web.UI.Page
    {

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        string ASDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            if (ValidateCaptcha())
            {
                
                loginAccount();
            }
            else
            {
                Response.Redirect("display.aspx?Comment=" + HttpUtility.HtmlEncode(tb_logemail.Text)); //htmlencode fixes the script issue and prevents it from executing
                Response.Redirect("display.aspx?Comment=" + HttpUtility.HtmlEncode(tb_logpw.Text));
                lbl_msg.Text = "You have not fulfilled the captcha";

            }




        }

        //1. Choose the same algorithm that was used to encrypt the data.
        //2. Retrieve the key that was used.
        //3. Retrieve the IV that was used.
        //4. Retrieve the encrypted data.
        //5. Decrypt the data.
        //6. Convert the decrypted data back to its original format.
        public void loginAccount()
        {
            string pwd = tb_logpw.Text.ToString().Trim();
            string email = tb_logemail.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            
            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);
                  
                    System.Diagnostics.Debug.WriteLine(dbSalt);
                    System.Diagnostics.Debug.WriteLine(userHash);
                    System.Diagnostics.Debug.WriteLine(dbHash);
                    if (userHash.Equals(dbHash))
                    {
                        Session["email"] = email;
                        

                        //create GUID and save into the session, a unique value that is hard to guess 
                        string guid = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guid; // save to the new session variable called auth token

                        //create a new cookie with guid value
                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                        Response.Redirect("Success.aspx", false);


                    }
                    else
                    {

                        lbl_msg.Text = "User email or password is not valid. Please try again.";
                        lbl_msg.ForeColor = System.Drawing.Color.Red;

                    }
                }
                else
                {
                    
                    lbl_msg.Text = "User email or password is not valid. Please try again.";
                    lbl_msg.ForeColor = System.Drawing.Color.Red;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
        }

        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(ASDBConnectionString);
            string sql = "select pwhash FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["pwhash"] != null)
                        {
                            if (reader["pwhash"] != DBNull.Value)
                            {
                                h = reader["pwhash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(ASDBConnectionString);
            string sql = "select pwsalt FROM ACCOUNT WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["pwsalt"] != null)
                        {
                            if (reader["pwsalt"] != DBNull.Value)
                            {
                                s = reader["pwsalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        

        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter
            //captchaResponse consist of the user click pattern
            string captchaResponse = Request.Form["g-recaptcha-response"];
            

            //To send a GET request to Google along with the response and secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("https://www.google.com/recaptcha/api/siteverify?secret=1 &response=" + captchaResponse); //secret key
            try
            {
                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //To show the JSON response string for learning purpose
                        lbl_gScore.Text = jsonResponse.ToString(); //label to display the JSOPN string

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the respons e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;

            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

       
    }
}