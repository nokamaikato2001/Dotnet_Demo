using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;
using Microsoft.VisualBasic.Devices;
using System.Text.RegularExpressions;

namespace Dotnet_CRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load1();
            timer1.Start();
        }

        SqlConnection con = new SqlConnection("Data Source=LAPTOP-6GRL9DB8\\SQLEXPRESS; Initial Catalog=Dotnet_CRUD; User Id=pawan; Password = pawan0804");
        SqlCommand cmd;
        SqlDataReader read;
        SqlDataAdapter drr;
        string id;
        bool Mode = true;
        string sql;


        public void Load1()
        {
            try
            {
                sql = "select * from Dotnet_CRUD";
                cmd = new SqlCommand(sql, con);
                con.Open();
                read = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();

                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3], read[4], read[5]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        //Select data from database
        public void getID(String id)
        {
            sql = "select * from Dotnet_CRUD where id = '" + id + "'  ";
            cmd = new SqlCommand(sql, con);
            con.Open();
            read = cmd.ExecuteReader();

            while (read.Read())
            {
                name.Text = read[1].ToString();
                dob.Text = read[2].ToString();
                age.Text = read[3].ToString();
                num1.Text = read[4].ToString();
                email.Text = read[5].ToString();
            }
            con.Close();
        }

        //Save Button
        private void button1_Click(object sender, EventArgs e)

        {
            string name1 = name.Text;
            string dob1 = dob.Text;
            string age1 = age.Text;
            string mobile = num1.Text;
            string email1 = email.Text;


            if (String.IsNullOrEmpty(name.Text))
            {
                //label8.Visible = true;
                MessageBox.Show("Please Enter Name.");
            }
            else if (String.IsNullOrEmpty(age.Text))
            {
                //label9.Visible = true;
                MessageBox.Show("Please Enter Age.");
            }
            else if (String.IsNullOrEmpty(num1.Text))
            {
                //label10.Visible = true;
                MessageBox.Show("Please Enter Mobile no.");
            }
            else if (String.IsNullOrEmpty(email.Text))
            {
                //label11.Visible = true;
                MessageBox.Show("Please Enter Email.");
            }

            else
            {

                if (num1.Text.Length == 10)
                {
                    string pattern = "^([0-9a-zA-Z]([-\\.\\w])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
                    if (Regex.IsMatch(email.Text, pattern))
                    {
                        errorProvider1.Clear();
                    }
                    else
                    {
                        errorProvider1.SetError(this.email, "Please Enter Valid Email.");
                        MessageBox.Show("Please Enter Valid Email.");
                        return;
                    }



                    if (Mode == true)
                    {

                        cmd = new SqlCommand("SELECT * from Dotnet_CRUD where number = '" + num1.Text + "'", con);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Mobile No. already exists.");

                        }
                            cmd = new SqlCommand("SELECT email from Dotnet_CRUD where email = '" + email.Text + "'", con);
                            SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);
                            DataTable dt1 = new DataTable();
                            adapter1.Fill(dt1);
                            if (dt1.Rows.Count > 0)
                            {
                                MessageBox.Show("Email already exists.");
                            }
                        
                        else
                        {
                            sql = "insert into Dotnet_CRUD(name,dob,age,number,email) values(@name,@dob,@age,@number,@email)";
                            con.Open();
                            cmd = new SqlCommand(sql, con);
                            cmd.Parameters.AddWithValue("@name", name1);
                            cmd.Parameters.AddWithValue("@dob", dob1);
                            cmd.Parameters.AddWithValue("@age", age1);
                            cmd.Parameters.AddWithValue("@number", mobile);
                            cmd.Parameters.AddWithValue("@email", email1);
                            MessageBox.Show("Record Added");
                            cmd.ExecuteNonQuery();

                            name.Clear();
                            age.Clear();
                            num1.Clear();
                            email.Clear();
                            name.Focus();
                            label8.Visible = false;
                            label9.Visible = false;
                            label10.Visible = false;
                            label11.Visible = false;
                        }
                    }

                    else
                    {
                        id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        sql = "update Dotnet_CRUD set name = @name, dob= @dob,age = @age,number = @number,email = @email where id = @id";
                        con.Open();
                        cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@name", name1);
                        cmd.Parameters.AddWithValue("@dob", dob1);
                        cmd.Parameters.AddWithValue("@age", age1);
                        cmd.Parameters.AddWithValue("@number", mobile);
                        cmd.Parameters.AddWithValue("@email", email1);
                        cmd.Parameters.AddWithValue("@id", id);
                        MessageBox.Show("Record Updated");
                        cmd.ExecuteNonQuery();

                        name.Clear();
                        age.Clear();
                        num1.Clear();
                        email.Clear();
                        name.Focus();
                        button1.Text = "Save";
                        Mode = true;
                    }
                }

                else
                {
                    MessageBox.Show("Mobile number should be 10 Digit.");
                }
            }
            con.Close();
                   
        }

        //Refresh Button
        private void button3_Click(object sender, EventArgs e)
        {
            Load1();
        }

        //Clear Button
        private void button2_Click(object sender, EventArgs e)
        {
            name.Clear();
            age.Clear();
            num1.Clear();
            email.Clear();
            name.Focus();
            button1.Text = "Save";
            Mode = true;

            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label7.Text = DateTime.Now.ToString("yyyy-MM-dd").ToString();
        }

        private void num1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Only Allow Digit");
            }
        }

        private void email_Leave(object sender, EventArgs e)
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (Regex.IsMatch(email.Text,pattern))
            {
                errorProvider1.Clear();
            }
            else
            {
                errorProvider1.SetError(this.email, "Please Enter Valid Email.");
                return;
            }
        }

        private void dob_Leave(object sender, EventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;

                int Age1= today.Year - dob.Value.Year;
                age.Text = Age1.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        //DataGrid CellContent Click
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                button1.Text = "Edit";

            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from Dotnet_CRUD where id  = @id ";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id ", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Delete");
                con.Close();
            }
        }
    }
}