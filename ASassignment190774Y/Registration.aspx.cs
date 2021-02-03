using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions; // for Regular expression
using System.Drawing; // for change of color

using System.Security.Cryptography;//hashing pw and cc
using System.Text;
using System.Data;
using System.Data.SqlClient;//using database
using System.Globalization;

namespace ASassignment190774Y
{
    public partial class Registration : System.Web.UI.Page
    {

        string ASDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateInput()
        {
            bool result;
            lbl_success.Text = String.Empty;

            if (tb_fname.Text == "")
            {
                
                lbl_success.Text += "First name is required" + "<br/>";
            }
            if (tb_lname.Text == "")
            {
                lbl_success.Text += "Last name is required" + "<br/>";
            }
            if (tb_cc.Text == "")
            {
                lbl_success.Text += "Credit Card Info is required" + "<br/>";
            }
            if (tb_expiry.Text == "")
            {
                lbl_success.Text += "Credit Card Expiry Info is required" + "<br/>";
            }
            if (tb_cvv.Text == "")
            {
                lbl_success.Text += "CVV is required" + "<br/>";
            }
            if (tb_email.Text == "")
            {
                lbl_success.Text += "Email is required" + "<br/>";
                //string email = tb_email.Text;
                //if (Regex.IsMatch(email, "[/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i]"))
                //    {

                //}
            }
            if (tb_pw.Text != "")
            {
                
               
                if(checkPw(tb_pw.Text) <= 4)
                { 
                    lbl_success.Text += "Please put a stronger pw" + "<br/>";
                }
                
            }
            else
            {
                lbl_success.Text = "Password is required" + "<br/>";
            }
            if (tb_dob.Text == "")
            {
                lbl_success.Text += "Date of birth is required" + "<br/>";
            }

            if (String.IsNullOrEmpty(lbl_success.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btn_checkpw(object sender, EventArgs e)
        {
            int scores = checkPw(tb_pw.Text);// Extract data from textbox
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            lbl_pwchecker.Text = "Status : " + status;
            if (scores < 4) //any score below 4 will show red
            {
                lbl_pwchecker.ForeColor = Color.Red;
                return;
            }
            lbl_pwchecker.ForeColor = Color.Green;
        }

        protected int checkPw(string password) //server side validation for password
        {
            int score = 0;

            // score 1 very weak
            // if length of password is less than 8 chars
            if(password.Length <8 )
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            // score 2 weak
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++; //++ is used to increment the value of its operand
            }                    
            // score 3 medium
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score ++;
            }
            // score 4 strong
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            // score 5 excellent
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }
            return score;



        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (ValidateInput() == true)
            {
                //string pwd = get value from your Textbox
                string pwd = tb_pw.Text.ToString().Trim(); ;
                //Generate random "salt"
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];
                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);
                SHA512Managed hashing = new SHA512Managed();
                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                finalHash = Convert.ToBase64String(hashWithSalt);
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
                createAccount();
                lbl_success.Text = "Sucessfully Registered";
                lbl_success.ForeColor = Color.Green;
                lbl_success.Visible = true;
                btn_login.Visible = true;
            }
            else
            {
                lbl_success.Visible = true;
            };
            

        }


        public void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ASDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@email, @fname, @lname, @cc, @expiry, @cvv, @dob,@pwsalt,@pwhash,@IV,@Key)")) //inserting the values into db
                {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@fname", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@lname", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@cc", Convert.ToBase64String(encryptData(tb_cc.Text.Trim())));
                            //cmd.Parameters.AddWithValue("@expiry", tb_expiry.Text.DateTime.Now.ToString("m/yyyy"));
                            //cmd.Parameters.AddWithValue("@expiry", Convert.ToDateTime(tb_expiry.Text));
                            string expiry = tb_expiry.Text.Trim(); // assigning it to a string will change it to yyyy-mm-dd which matches the db
                            cmd.Parameters.AddWithValue("@expiry", Convert.ToBase64String(encryptData(expiry)));
                            cmd.Parameters.AddWithValue("@cvv", Convert.ToBase64String(encryptData(tb_cvv.Text.Trim())));
                            //cmd.Parameters.AddWithValue("@dob", tb_dob.Text.Trim());
                            string dob = tb_dob.Text.Trim();
                            cmd.Parameters.AddWithValue("@dob", dob);
                            cmd.Parameters.AddWithValue("@pwsalt", salt);
                            cmd.Parameters.AddWithValue("@pwhash", finalHash);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            //cmd.Parameters.AddWithValue("@Status", "Open");
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex) //catches any error to protect the database from sql injection
            {
                throw new Exception(ex.ToString());
            }
        }

        //Steps:
        //1. Choose an algorithm.
        //2. Create or retrieve a key.
        //3. Generate the IV.
        //4. Convert the clear text data to an array of bytes.
        //5. Encrypt the clear text byte array.
        //6. Return the cipherText.

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }
    }
}