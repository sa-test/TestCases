using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SQLtestWinForm1
{
    public partial class Form1 : Form
    {

        DataSet ds;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        SqlConnection connection;
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB1;Integrated Security=True";
        string sql = "SELECT * FROM Users";


        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;

            try
            {
                connection.Open();
                textBoxLog.AppendText("Подключение к базе: " + connection.DataSource + "\\" + connection.Database + "\n");
                adapter = new SqlDataAdapter(sql, connection);

                ds = new DataSet();
                adapter.Fill(ds);
                textBoxLog.AppendText("Считали данные в датасет. \n");

                dataGridView1.DataSource = ds.Tables[0];
                textBoxLog.AppendText("Показали датасет в таблице. \n");

                // делаем недоступным столбец id для изменения
                dataGridView1.Columns["Id"].ReadOnly = true;

            } catch (Exception ex)
            {
                textBoxLog.AppendText("Ошибка подключения к базе: " + ex.Message + "\n");

            } finally
            {
                connection.Close();
                textBoxLog.AppendText("Отключились от базы. \n");

            }

        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Close();
                textBoxLog.AppendText("Отключились от базы. \n");

                //connection.Dispose();
                ds.Clear();
                textBoxLog.AppendText("Очистили датасет \n");

            }
            catch (Exception ex)
            {
                textBoxLog.AppendText("Ошибка отключения от базы: " + ex.Message + "\n");

            }


        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ds.Tables[0].NewRow(); // добавляем новую строку в DataTable
                ds.Tables[0].Rows.Add(row);
                textBoxLog.AppendText("Добавили строку в датасет. Новая строка БУДЕТ добавлена в базу при нажатии кнопки Сохранить \n");
            }
            catch (Exception ex)
            {
                textBoxLog.AppendText("Ошибка добавления строк: " + ex.Message + "\n");

            }

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
                textBoxLog.AppendText("Выбраные строки удалены из датасета. Выбранные строки БУДУТ удалены из базы при нажатии кнопки Сохранить \n");
            }
            catch (Exception ex)
            {
                textBoxLog.AppendText("Ошибка удаления строк: " + ex.Message + "\n");

            }


        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                textBoxLog.AppendText("Подключение к базе... \n");

                adapter = new SqlDataAdapter(sql, connection);
                commandBuilder = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = new SqlCommand("sp_CreateUser", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NChar, 10, "Name"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@descr", SqlDbType.Text, 0, "Description"));

                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 0, "id");
                parameter.Direction = ParameterDirection.Output;

                adapter.Update(ds);
                textBoxLog.AppendText("Изменения приняты, база обновлена \n");

            }
            textBoxLog.AppendText("Отключение от базы. (Статус="+ connection.State.ToString()+ ") \n");

        }
    }
}
