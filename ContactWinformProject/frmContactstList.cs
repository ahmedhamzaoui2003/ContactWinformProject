using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using clsBusinessLayer;
using System.Data;


namespace ContactWinformProject
{
    public partial class frmContactstList : Form
    {
        public frmContactstList()
        {
            InitializeComponent();
        }

        
        private void _FillGridViewWithContacts()
        {
            dgvContactsList.DataSource = clsContact.GetAllContacts();
        }
        private void frmContactstList_Load(object sender, EventArgs e)
        {
            _FillGridViewWithContacts();
        }

        private void btnAddNewContact_Click(object sender, EventArgs e)
        {
            var frmAddNewContact = new frmAddEditContact(-1);
            frmAddNewContact.ShowDialog();

            // Refresh Data :
            _FillGridViewWithContacts();
        }

        private void tsmEdit_Click(object sender, EventArgs e)
        {
            var frmEditContact = new frmAddEditContact((int)dgvContactsList.CurrentRow.Cells[0].Value);
            frmEditContact.ShowDialog();

            // Refresh Data :
            _FillGridViewWithContacts();
        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
            int ContactID = (int)dgvContactsList.CurrentRow.Cells[0].Value;
            if (MessageBox.Show($"Are you sure to delete contact [ {ContactID} ] ?","Question",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsContact.IsContactExist(ContactID))
                {
                    if (clsContact.DeleteContact(ContactID))
                    {
                        MessageBox.Show($"Contact [ {ContactID} ] was deleted successfully ", "Success");
                        return;
                    }
                    else
                        MessageBox.Show($"Faild to delete contact [ {ContactID} ]");
                }
                else
                    MessageBox.Show($"Contact [ {ContactID} ] not exist");
            }

            // Refresh Data :
            _FillGridViewWithContacts();
        }
    }
}
