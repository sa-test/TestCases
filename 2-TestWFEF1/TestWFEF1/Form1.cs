using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWFEF1
{
    public partial class Form1 : Form
    {

        TestDB1Entities db;


        public Form1()
        {
            InitializeComponent();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void ConnectDB()
        {
            db = new TestDB1Entities();
            db.Users.Load();
            dataGridView1.DataSource = db.Users.Local.ToBindingList();
            dataGridView1.Columns["id"].ReadOnly = true;

            textBoxLog.AppendText("Загрузили данные из базы в Модель. Показали данные в таблице (связали модель и таблицу) \n");

        }

        private void DisconnectDB()
        {
            db.Dispose();
            dataGridView1.DataSource = null;

            textBoxLog.AppendText("Отключились от базы (Контекст.Dispose()) \n");

        }


        private void AddNewRow()
        {
            db.Users.Add(new User());
            dataGridView1.Refresh();

            textBoxLog.AppendText("Добавили строку в датаГрид. Новая строка БУДЕТ добавлена в базу при нажатии кнопки Сохранить \n");
        }

        private void DeleteRow()
        {
            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {
                User us = db.Users.Find(row.Cells[0].Value);
                db.Users.Remove(us);
            }
            textBoxLog.AppendText("Удалили выбранные строки. Необходимо Сохранить изменения.\n");

            //db.SaveChanges();
        }

        private void UpdateDB()
        {
            try
            {
                db.SaveChanges();
                textBoxLog.AppendText("Сохранили изменения \n");
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
