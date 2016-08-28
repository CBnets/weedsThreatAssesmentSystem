using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
//Add MySql Library
using MySql.Data.MySqlClient;

namespace WeedCropsIDSSystem
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "WeedCropIMS";
            uid = "root";
            password = "root";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }


        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //关闭连接
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //插入数据到t_weedcropsnum表中
        public void Insert(string imageName,float weedCounts,float weedsDensity,float cropsDensity,float soilsDensity,float ciws,float cics,float ciss,float aics,float tRates)
        {
            string query = "INSERT INTO t_weedcropsnum(imageNumber,weedCount,weedDensity,cropDensity,soilDensity,ciw,cic,cis,aic,tRate) VALUES(@imageNumber,@weedCount,@weedDensity,@cropDensity,@soilDensity,@ciw,@cic,@cis,@aic,@tRate)";

            MySqlParameter imageNumber = new MySqlParameter("@imageNumber", MySqlDbType.String);
            imageNumber.Value = imageName;
            MySqlParameter weedCount = new MySqlParameter("@weedCount", MySqlDbType.Int32);
            weedCount.Value = weedCounts;
            MySqlParameter weedDensity = new MySqlParameter("@weedDensity", MySqlDbType.Float);
            weedDensity.Value = weedsDensity;
            MySqlParameter cropDensity = new MySqlParameter("@cropDensity", MySqlDbType.Float);
            cropDensity.Value = cropsDensity;
            MySqlParameter soilDensity = new MySqlParameter("@soilDensity", MySqlDbType.Float);
            soilDensity.Value = soilsDensity;
            MySqlParameter ciw = new MySqlParameter("@ciw", MySqlDbType.Float);
            ciw.Value = ciws;
            MySqlParameter cic = new MySqlParameter("@cic", MySqlDbType.Float);
            cic.Value = cics;
            MySqlParameter cis = new MySqlParameter("@cis", MySqlDbType.Float);
            cis.Value = ciss;
            MySqlParameter aic = new MySqlParameter("@aic", MySqlDbType.Float);
            aic.Value = aics;
            MySqlParameter tRate = new MySqlParameter("@tRate", MySqlDbType.Float);
            tRate.Value = tRates;


            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.Add(imageNumber);
                cmd.Parameters.Add(weedCount);
                cmd.Parameters.Add(weedDensity);
                cmd.Parameters.Add(cropDensity);
                cmd.Parameters.Add(soilDensity);
                cmd.Parameters.Add(ciw);
                cmd.Parameters.Add(cic);
                cmd.Parameters.Add(cis);
                cmd.Parameters.Add(aic); 
                cmd.Parameters.Add(tRate);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //插入数据到t_threatrategps表
        public void InsertLatLongRate(string imageName, string latitudes, string longitudes, float tRates)
        {
            string query = "INSERT INTO t_threatrategps(imageNumber,latitude,longitude,tRate) VALUES(@imageNumber,@latitude,@longitude,@tRate)";

            MySqlParameter imageNumber = new MySqlParameter("@imageNumber", MySqlDbType.String);
            imageNumber.Value = imageName;

            MySqlParameter latitude = new MySqlParameter("@latitude", MySqlDbType.String);
            latitude.Value = latitudes;
            MySqlParameter longitude = new MySqlParameter("@longitude", MySqlDbType.String);
            longitude.Value = longitudes;
            MySqlParameter tRate = new MySqlParameter("@tRate", MySqlDbType.Float);
            tRate.Value = tRates;

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.Add(imageNumber);
                cmd.Parameters.Add(latitude);
                cmd.Parameters.Add(longitude);
                cmd.Parameters.Add(tRate);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update()
        {
            string query = "UPDATE t_weedcropsnum SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM t_weedcropsnum WHERE imageId='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //查询杂草作物图像原始数据
        public List<string>[] Select()
        {
            string query = "SELECT * FROM t_weedcropsnum";

            //Create a list to store the result
            List<string>[] list = new List<string>[10];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            list[8] = new List<string>();
            list[9] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["imageNumber"] + "");
                    list[1].Add(dataReader["weedCount"] + "");
                    list[2].Add(dataReader["ciw"] + "");
                    list[3].Add(dataReader["cic"] + "");
                    list[4].Add(dataReader["cis"] + "");
                    list[5].Add(dataReader["aic"] + "");
                    list[6].Add(dataReader["weedDensity"] + "");
                    list[7].Add(dataReader["cropDensity"] + "");
                    list[8].Add(dataReader["soilDensity"] + "");
                    list[9].Add(dataReader["tRate"] + "");
                   
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //统计杂草作物图像原始数据个数
        public int Count()
        {
            string query = "SELECT Count(*) FROM t_weedcropsnum";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }


        //杂草威胁率与GPS坐标数据
        public List<string>[] SelectWeeddThreatRate()
        {
            string query = "SELECT * FROM t_threatrategps";

            //Create a list to store the result
            List<string>[] listRate = new List<string>[4];
            listRate[0] = new List<string>();
            listRate[1] = new List<string>();
            listRate[2] = new List<string>();
            listRate[3] = new List<string>();
          

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    listRate[0].Add(dataReader["imageNumber"] + "");
                    listRate[1].Add(dataReader["latitude"] + "");
                    listRate[2].Add(dataReader["longitude"] + "");
                    listRate[3].Add(dataReader["tRate"] + "");

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return listRate;
            }
            else
            {
                return listRate;
            }
        }


        //杂草威胁率与GPS坐标数据
        public List<string>[] SelectImageNameRate()
        {
            string query = "SELECT imageNumber,tRate FROM t_threatrategps";

            //Create a list to store the result
            List<string>[] listImageNameRate = new List<string>[2];
            listImageNameRate[0] = new List<string>();
            listImageNameRate[1] = new List<string>();
          

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    listImageNameRate[0].Add(dataReader["imageNumber"] + "");
                    listImageNameRate[1].Add(dataReader["tRate"] + "");

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return listImageNameRate;
            }
            else
            {
                return listImageNameRate;
            }
        }

        //统计杂草威胁率数据个数
        public float AverageRate()
        {
             string query = "SELECT tRate FROM t_threatrategps";

            //Create a list to store the result
             List<float> listRate = new List<float>();
             float averageRate = 0;

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    float rate = float.Parse(dataReader["tRate"]+"");
                    listRate.Add(rate);

                }

                //求总体杂草威胁率
                averageRate = listRate.Sum() / listRate.Count;

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return averageRate;
            }
            else
            {
                return averageRate;
            }
        }

        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "F:\\" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to backup!");
            }
        }

        //Restore
        public void Restore()
        {
            try
            {
                //Read file from F:\
                string path;
                path = "F:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to Restore!");
            }
        }
    }
}
