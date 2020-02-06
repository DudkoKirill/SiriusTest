using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using SiriusSqlTest1;
using System.Data.Common;

namespace SiriusSqlTest
{
    public partial class Form1 : Form
    {
        private SqlConnection conn;
        public Form1()
        {
            InitializeComponent();
        }

       
        

        private void button1_Click(object sender, EventArgs e)
        {
            
            // Получить объект Connection подключенный к DB.
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ФИО");
                dt.Columns.Add("Статус");
                dt.Columns.Add("Отдел");
                dt.Columns.Add("Должность");
                dt.Columns.Add("Дата приема");
                dt.Columns.Add("Дата увольнения");

                SqlCommand cmd = new SqlCommand("exec Persons_list", conn);



                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            
                            DataRow r = dt.NewRow();
                            string name = reader.GetString(0);
                            string post = reader.GetString(1);
                            string dep = reader.GetString(2);
                            string stat = reader.GetString(3);
                            DateTime start = reader.GetDateTime(4);
                            r["ФИО"] = name;
                            r["Статус"] = stat;
                            r["Отдел"] = dep;
                            r["Должность"] = post;
                            r["Дата приема"] = start;
                            DateTime end;
                            if (!reader.IsDBNull(5))
                            {
                                end = reader.GetDateTime(5);
                                r["Дата увольнения"] = end;
                            }
                            dt.Rows.Add(r);
                        }
                    }
                }
                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;
            }
            catch (Exception err)
            {
                MessageBox.Show(
                    err.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                conn.Close();
                // Разрушить объект, освободить ресурс.
                conn.Dispose();
            }
            catch (Exception err)
            {
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                string sql = "";
                if (radioButton2.Checked)
                    sql = "person_with_stat_with_emp";
                else if (radioButton1.Checked)
                    sql = "person_with_stat_with_unep";
                SqlCommand cmd = new SqlCommand(sql, conn); ;
                cmd.CommandType = CommandType.StoredProcedure;
                DateTime date_start;
                DateTime date_end;
                string stat;
                date_start = dateTimePicker1.Value;
                date_end = dateTimePicker2.Value;
                stat = listBox1.SelectedItem.ToString();

                cmd.Parameters.Add("@date_start", SqlDbType.DateTime).Value = date_start;
                cmd.Parameters.Add("@date_end", SqlDbType.DateTime).Value = date_end;
                cmd.Parameters.Add("@stat", SqlDbType.VarChar).Value = stat;

                DataTable dt = new DataTable();
                dt.Columns.Add("Дата");
                dt.Columns.Add("Количество");


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            DataRow r = dt.NewRow();
                            DateTime date = reader.GetDateTime(0);
                            Int32 count = reader.GetInt32(1);
                            r["Дата"] = date;
                            r["Количество"] = count;
                            dt.Rows.Add(r);
                        }
                    }
                }
                dataGridView2.DataSource = dt;
                dataGridView2.AllowUserToAddRows = false;
            }
            catch (Exception err)
            {
                MessageBox.Show(
                    err.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filename;
            string setting_string;
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                try
                {
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        setting_string=sr.ReadToEnd();
                        conn = DBUtils.GetDBConnection(setting_string);
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("exec stat_list", conn);
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string stat = reader.GetString(0);
                                    listBox1.Items.Add(stat);
                                    listBox1.SetSelected(0, true);
                                }
                            }
                        }
                    }
                    button1.Enabled = true;
                    button2.Enabled = true;
                }
                catch (Exception err)
                {
                    MessageBox.Show(
                        err.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int row=0;
            if (radioButton3.Checked)
                row = 0;
            if (radioButton4.Checked)
                row = 1;
            if (radioButton5.Checked)
                row = 2;
            if (radioButton6.Checked)
                row = 3;
            dataGridView1.CurrentCell = null;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                if (row!=0)
                    dataGridView1.Rows[i].Visible = dataGridView1.Rows[i].Visible && dataGridView1[row, i].Value.ToString() == textBox1.Text;
                else
                    dataGridView1.Rows[i].Visible = dataGridView1.Rows[i].Visible && dataGridView1[row, i].Value.ToString().Contains(textBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = null;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Visible = true;
        }
    }
}
