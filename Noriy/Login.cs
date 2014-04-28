using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using Microsoft.Win32;

namespace Noriy
{
    public partial class Login : Form
    {
        //Global varaibles
        public bool SuccesfulLogin = false;



        //------------
        public Login()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
                Connection.Open();

                string CheckUserName = "select count(*) from accounts where Username='" + textBox1.Text + "'";
                MySqlCommand Command = new MySqlCommand(CheckUserName, Connection);
                int count = Convert.ToInt32(Command.ExecuteScalar().ToString());

                if (count != 0) //if user exists
                {
                    string PaswordQuery = "select Password from Accounts where Username='" + textBox1.Text + "'";
                    MySqlCommand PassCheck = new MySqlCommand(PaswordQuery, Connection); ;
                    string password = PassCheck.ExecuteScalar().ToString().Replace(" ", "");

                    if (password == textBox2.Text)
                    {
                        //Set the Registry Key Value
                        RegistryKey RKey = Registry.CurrentUser.OpenSubKey("Noriy",true);
                        RKey.SetValue("username" , textBox1.Text);
                        //---------End of setting key value
                        SuccesfulLogin = true;
                        this.Close();

                    }
                    else MessageBox.Show("Password is invalid!");
                }
                else MessageBox.Show("Username is not valid!");

                Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost:59873/Basic_Registration/Register.aspx");
        }
    }
}
