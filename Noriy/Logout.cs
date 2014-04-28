using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.Configuration;

namespace Noriy
{
    public partial class Logout : Form
    {
        private RegistryKey Reg = Registry.CurrentUser.OpenSubKey("Noriy", true);

        public Logout()
        {
            InitializeComponent();
            label3.Text = Reg.GetValue("username").ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0)
            {
                RegistryKey Reg = Registry.CurrentUser.OpenSubKey("Noriy",true);
                using (MySqlConnection Connection = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]))
                {
                    
                    try
                    {
                        Connection.Open();

                        string PaswordQuery = "select Password from Accounts where Username='" + Reg.GetValue("username") + "'";
                        MySqlCommand PassCheck = new MySqlCommand(PaswordQuery, Connection); ;
                        string password = PassCheck.ExecuteScalar().ToString().Replace(" ", "");

                        if (password == textBox1.Text)
                        {
                            //Set the Registry Key Value of username to null
                            Reg.SetValue("username", "null");
                    
                            //---------End of setting key value
                            this.Close();

                        }
                        else MessageBox.Show("Password is invalid!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter the password!");
            }

        }
    }
}
