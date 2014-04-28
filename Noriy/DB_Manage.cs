using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using Microsoft.Win32;

namespace Noriy
{
    public class DB_Manage
    {

        #region GetGuid
        public static string GetGuid()
        {
            string Guid = "";
            try
            {
                using (MySqlConnection Connection = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]))
                {
                    Connection.Open();
                    string Query = "SELECT * FROM accounts WHERE Username = '" + Registry_Manage.GetUsername() + "'";
                    MySqlCommand Command = new MySqlCommand(Query, Connection);
                    MySqlDataReader Reader = Command.ExecuteReader();
                    while (Reader.Read())
                    {
                        Guid = Reader.GetString(0);
                    }

                    return Guid;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "null";
            }
        }

        #endregion
        #region Insertion
        public static void InsertUrl(string url, bool Accepted)
        {
  
            if (Registry_Manage.GetUsername() != "null")
            {
                try
                {
                    if (url.Length < 32) //The maximum length of the url
                    {
                        using (MySqlConnection Connection = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]))
                        {
                            Connection.Open();
                            string InsertTraffic = "";


                            if (Accepted == true) //Checks if the url has been accepted or rejected
                                InsertTraffic = "INSERT INTO Traffic (Url,Time,Accepted) VALUES('" + url + "','" + DateTime.Now.ToString() + "','True');";
                            else InsertTraffic = "INSERT INTO Traffic (Url,Time,Accepted) VALUES('" + url + "','" + DateTime.Now.ToString() + "','False');";


                            using (MySqlCommand Command = new MySqlCommand(InsertTraffic, Connection))
                            {
                                Command.ExecuteNonQuery();
                            }

                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }
        #endregion

        #region Checking
        private static bool CheckDomain(string url)
        {
            try
            {
                using (MySqlConnection Connection = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]))
                {

                    RegistryKey reg = Registry.CurrentUser.OpenSubKey("Noriy");
                    string username = reg.GetValue("username").ToString();
                    //Get the Category List
                    Connection.Open();
                    MySqlDataAdapter Adapter = new MySqlDataAdapter("SELECT * FROM category_links WHERE User_ID = '" + Form1.Guid + "';", Connection);
                    DataTable Dt = new DataTable();
                    Adapter.Fill(Dt);

                    //Check each category
                    //----------------------------------------------------------------------Check Blacklist Domain-------------------------------------------------- 


                    foreach (DataRow Row in Dt.Rows)
                    {

                        int size1 = url.IndexOf('/');

                        string domain1 = "";

                        string domain2 = url;

                        if (url.Substring(0, 4) == "www.")
                        {
                            domain2 = url.Substring(4, domain2.Length - 4);
                        }

                        int size2 = domain2.IndexOf('/');

                        if (size1 < 1)
                        {
                            domain1 = url;
                        }
                        else
                        {
                            domain1 = url.Substring(0, size1);
                        }

                        if (size2 > 1)
                        {
                            domain2 = domain2.Substring(0, size2);
                        }

                        /*if (domain1.Substring(domain1.Length - 4, 4) == ":443") //checks if it is https (port 443)
                        {
                            domain1 = domain1.Substring(0, domain1.Length - 4);
                            MessageBox.Show("https");
                        }

                        if (domain2.Substring(domain2.Length - 4, 4) == ":443") //checks if it is https (port 443)
                        {
                            domain2 = domain2.Substring(0, domain2.Length - 4);
                        }
                        */

                        if (domain1 != domain2)
                        {


                            string searchQuery1 = "select count(*) from blacklist_domain_" + Row["Category_ID"].ToString() + " where domain like '%" + domain1 + "%'";
                            string searchQuery2 = "select count(*) from blacklist_domain_" + Row["Category_ID"].ToString() + " where domain like '%" + domain2 + "%'";

                            MySqlCommand Command1 = new MySqlCommand(searchQuery1, Connection);
                            MySqlCommand Command2 = new MySqlCommand(searchQuery2, Connection);

                            int count1 = Convert.ToInt32(Command1.ExecuteScalar().ToString());
                            int count2 = Convert.ToInt32(Command2.ExecuteScalar().ToString());



                            if (count1 > 0 || count2 > 0)
                            {
                                return false;
                            }




                        }
                        else
                        {

                            string searchQuery2 = "select count(*) from blacklist_domain_" + Row["Category_ID"].ToString() + " where domain like '%" + domain2 + "%'";
                            MySqlCommand Command2 = new MySqlCommand(searchQuery2, Connection);
                            int count2 = Convert.ToInt32(Command2.ExecuteScalar().ToString());


                            if (count2 > 0)
                            {
                                return false;
                            }






                        }



                    }
                    //-----------------------------------------------------------------------Check Blacklist Url-------------------------------------------------------------------
                    foreach (DataRow Row in Dt.Rows)
                    {
                        string searchQuery = "select count(*) from blacklist_url_" + Row["Category_ID"].ToString() + " where Url = '" + url + "'";
                        using (MySqlCommand Command = new MySqlCommand(searchQuery, Connection))
                        {
                            int count = Convert.ToInt32(Command.ExecuteScalar().ToString());
                            if (count > 0)
                            {
                                return false;
                            }
                        }
                    }



                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            return true;

        }
            



        #endregion


    }
}
