using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WcfClient.ServiceReference1;

namespace WcfClient
{
    public partial class Form1 : Form
    {


        TestDBServiceClient DBClient;
        List<int> changedDataGridRowsId;

        public Form1()
        {
            InitializeComponent();
            changedDataGridRowsId = new List<int>();
            dataGridView1.CellBeginEdit += DataGridView1_CellBeginEdit;
        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int index = dataGridView1.Columns["Id"].Index;
            //int id = Int32.Parse(dataGridView1.CurrentRow.Cells[index].Value.ToString());
            int id = dataGridView1.CurrentRow.Index;
            if (!changedDataGridRowsId.Exists(i => i == id))
            {
                changedDataGridRowsId.Add(id);
            }
        }

        private void ConnectDB()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;

            try
            {
                textBoxLog.AppendText("Подключение к сервису... \n");

                DBClient = new TestDBServiceClient();

                dataGridView1.DataSource = DBClient.GetData();
                dataGridView1.Columns["Id"].ReadOnly = true;
                dataGridView1.Columns["Id"].DisplayIndex = 0;
                textBoxLog.AppendText("Подключились к сервису - ОК \n");
                textBoxLog.AppendText("Запросили данные из базы - ОК \n");

            }
            catch (Exception ex)
            {
                textBoxLog.AppendText("Ошибка подключения к сервису: " + ex.Message + "\n");

            }

        }

        private void DisconnectDB()
        {
            try
            {
                DBClient.Close();
                dataGridView1.DataSource = null;
                textBoxLog.AppendText("Отключились от базы. \n");


            }
            catch (Exception ex)
            {
                textBoxLog.AppendText("Ошибка отключения от базы: " + ex.Message + "\n");

            }

        }


        private void AddNewRow()
        {
            //db.Users.Add(new User());
            DBClient.AddUser("", "");
            dataGridView1.DataSource = DBClient.GetData();
            dataGridView1.Columns["Id"].DisplayIndex = 0;
            dataGridView1.Refresh();

            textBoxLog.AppendText("Добавление новой строки - ОК \n");
        }

        private void DeleteRow()
        {
            int resultDel = 0; 
            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int index = dataGridView1.Columns["Id"].Index;
                    int id = Int32.Parse(row.Cells[index].Value.ToString());
                    resultDel = DBClient.DeleteUser(id);
                    if (resultDel == 1)
                    {
                        textBoxLog.AppendText("Удаление строки id=" +id.ToString()+ " (ОК)\n");

                    }
                    else
                    {
                        textBoxLog.AppendText("Удаление строки id=" + id.ToString() + " ОШИБКА\n");
                    }
                }
                dataGridView1.DataSource = DBClient.GetData();
                dataGridView1.Columns["Id"].DisplayIndex = 0;
                dataGridView1.Refresh();


            }
            catch (Exception ex)
            {
                textBoxLog.AppendText("Удаление строки - ОШИБКА: " + ex.ToString() + "\n");

            }

        }

        private void UpdateDB()
        {
            try
            {
                if (changedDataGridRowsId != null && 
                    changedDataGridRowsId.Count > 0)
                {
                    int indName = dataGridView1.Columns["Name"].Index;
                    int indDescr = dataGridView1.Columns["Description"].Index;
                    int indId = dataGridView1.Columns["Id"].Index;


                    foreach (int id in changedDataGridRowsId)
                    {
                        int userId = Int32.Parse(dataGridView1.Rows[id].Cells[indId].Value.ToString());
                        string name = dataGridView1.Rows[id].Cells[indName].Value.ToString();
                        string description = dataGridView1.Rows[id].Cells[indDescr].Value.ToString();

                        int res = DBClient.UpdateUser(userId, name, description);
                        if (res == 1)
                        {
                            textBoxLog.AppendText("Сохранили изменения id=" + userId.ToString() + "\n");
                        } else
                        {
                            textBoxLog.AppendText("Ошибка изменения id=" + userId.ToString() + "\n");
                        }
                        

                    }

                    changedDataGridRowsId.Clear();
                } else
                {
                    textBoxLog.AppendText("Сохранять нечего (ничего не меняли) \n");
                }
            }
            catch (Exception ex)
            {
                textBoxLog.AppendText("Ошибка сохранения: " + ex.ToString() + " \n");
            }
            dataGridView1.Refresh();

        }



        private void buttonConnect_Click(object sender, EventArgs e)
        {
            ConnectDB();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectDB();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteRow();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            UpdateDB();
        }
    }
}
