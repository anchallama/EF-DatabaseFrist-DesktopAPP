using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EF_COdeFrist_DesktopAPP
{
    public partial class Form1 : Form
    {
       Customer model = new Customer(); // Creating   obj ofCustomer  1 step
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();

        }
        void Clear()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
           model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDateGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = textBox1.Text.Trim();
            model.LastName = textBox2.Text.Trim();
            model.City = textBox3.Text.Trim();
            model.Address = textBox4.Text.Trim();
            using (EFDBEntities db= new EFDBEntities())
            {
                if (model.CustomerID == 0) //insert
                    db.Customers.Add(model);

                else         //update
                    db.Entry(model).State = EntityState.Modified;

                db.SaveChanges();
            }
            Clear();
            PopulateDateGridView();
            MessageBox.Show("Submitted successfully");
        }
        void PopulateDateGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using(EFDBEntities db=new EFDBEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                using(EFDBEntities db= new EFDBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    textBox1.Text = model.FirstName;
                    textBox2.Text = model.LastName;
                    textBox3.Text = model.City;
                    textBox4.Text = model.Address;
                }
                btnSave.Text = "update";
                btnDelete.Enabled = true; 
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you  sure to Delete this Record ?", "EF CURD operation ", MessageBoxButtons.YesNo) == DialogResult.Yes) ;
            using (EFDBEntities db =new EFDBEntities())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.Customers.Attach(model);
                db.Customers.Remove(model);
                db.SaveChanges();
                PopulateDateGridView();
                Clear();
            }
        }
    }
}
