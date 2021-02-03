using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASassignment190774Y
{
    public partial class Success : System.Web.UI.Page
    {
        string ASDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        //byte[] nric = null;
        string email = null;
        protected void Page_Load(object sender, EventArgs e) // The method checks for a valid session else it would redirect the user back to the login page
        {
            if (Session["email"] != null)
            {
                email = (string)Session["email"];

            //    displayUserProfile(email);
            }
            if (Session["email"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null) //checks for normal session, the new session "AuthToken" and new cookie 
            {
                //String authToken = Session["AuthToken"].ToString();
                //String cookie = Request.Cookies["AuthToken"].Value;
                //if (!authToken.Equals(cookie))
                //comes here when the 3 conditions above is not null and checks if they match
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else 
                {
                    lbl_msg.Text = "Sucessfully logged in";
                    lbl_msg.ForeColor = System.Drawing.Color.Green;
                    btn_logout.Visible = true;
                    displayUserProfile(email);
                }
                

                
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            //Read the decrypted bytes from the decrypting stream
                            // and place them in a string
                            plainText = srDecrypt.ReadToEnd();
                        }

                    }

                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }
        protected void displayUserProfile(string email)
        {
            SqlConnection connection = new SqlConnection(ASDBConnectionString);
            string sql = "SELECT * FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["email"] != DBNull.Value)
                        {
                            lbl_email.Text = reader["email"].ToString();
                        }
                    }
                    //lbl_nric.Text = decryptData(nric);
                }
            }//try
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // removes all the variables stored in session and if user try to browse your site same sessionID will be used which was previously assigned to him
            Session.Abandon(); //removes all the variables stored in session, fire session_end event and if user try to browse your site a new sessionID will be assigned to him.
            Session.RemoveAll(); //removing books from the bookshelf whereas Session.Abandon() is like throwing the bookshelf itself.

            Response.Redirect("logout.Aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null) // making sure the cookie is removed from the browser 
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null) //then expire the authtoken cookie as well
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
    }
}