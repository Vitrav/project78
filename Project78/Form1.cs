using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Project78
{
    public partial class Form1 : Form
    {
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();

            connectAndPrepare();
            fillChart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //fillChart method  
        private void connectAndPrepare()
        {

            string connstring = String.Format("Server=localhost;Port=5432;" +
                "User Id=postgres;Password=;Database=project78;");

            // Connectie maken met de connstring
            NpgsqlConnection conn = new NpgsqlConnection(connstring);


            try
            {
                // PostgeSQL-style connection string

                conn.Open();
                MessageBox.Show("Connectie gelukt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            string sql = "SELECT * FROM voorraadbeheer";

            // data adapter making request from our connection
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

            string sql2 = "insert into voorraadbeheer ( voorraad , schap) values (" + getDateVanAfstandsensoor() + "   ,  '1' ) ";

            NpgsqlCommand dc1 = new NpgsqlCommand(sql2, conn);
            dc1.ExecuteNonQuery();
            
            ds.Reset();
            // Data vullen uit de adapter
            da.Fill(ds);
            // Eerste table kiezen
            dt = ds.Tables[0];
            
            chart1.DataSource = dt;




        }
        public void fillChart() {
            
            
            // Loopt over elke row in de DataSet tables
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int voorraadInt = Convert.ToInt32(row["voorraad"]);
                String schapNaam = row["schap"].ToString();

                // Voor elke row pakt hij de "schap" en "voorraad" en zet deze in de chart
                this.chart1.Series["Voorraad"].Points.AddXY(row["schap"].ToString(), row["voorraad"].ToString());
                if (voorraadInt <= 6)
                {
                    MessageBox.Show("BIJNA LEEG SCHAP:   " + schapNaam);
                }
            }
}









#region toolStripMenu
private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Project 7-8\r\nGemaakt door groep 3\r\nvoor de Hogeschool Rotterdam en Quistor");
        }
#endregion

        private void button1_Click(object sender, EventArgs e)
        {
            connectAndPrepare();
            this.chart1.Update();
            this.chart1.
            

        }


        private int getDateVanAfstandsensoor()
        {
        SerialPort sp = new SerialPort("COM4");

        sp.Open();
            

        int a =  Convert.ToInt32(sp.ReadLine());

         sp.Close();

            return a;
        
        }


        private void insertDataInDb()
        {

        }
    }
}
