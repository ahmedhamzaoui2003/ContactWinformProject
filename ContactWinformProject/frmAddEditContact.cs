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
    public partial class frmAddEditContact : Form
    {
        enum enMode { AddNew,Update}
        int _ContactID;
        enMode _Mode;
        clsContact _Contact;
        public frmAddEditContact(int ContactID)
        {
            InitializeComponent();

            _ContactID = ContactID;

            if (_ContactID == -1)
            {
                _Mode = enMode.AddNew;
                lnkRemoveImage.Visible = false;
            }
            else
                _Mode = enMode.Update;
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();
            foreach(DataRow row in dtCountries.Rows)
            {
                cbCountries.Items.Add(row["CountryName"]);
            }
        }

        private void _LoadData()
        {
            _FillCountriesInComboBox();
            cbCountries.SelectedIndex = 0;
            //this is for the Add New Mode :
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Contact";
                _Contact = new clsContact();
                return;
            }
            // this is for the Update Mode :
            _Contact = clsContact.Find(_ContactID);

            lblTitle.Text = "Edit Contact With ID = " + _ContactID;
            lblContactID.Text = _Contact.ID.ToString();
            txtFirstName.Text = _Contact.FirstName;
            txtLastName.Text = _Contact.LastName;
            txtEmail.Text = _Contact.Email;
            txtPhone.Text = _Contact.Phone;
            dtpDateOfBirth.Value = _Contact.DateOfBirth;
            txtAddress.Text = _Contact.Address;
            // Dealing with picturebox :
            if(_Contact.ImagePath != null)
            {
                pbContactImage.ImageLocation = _Contact.ImagePath;
            }

            lnkRemoveImage.Visible = (_Contact.ImagePath != null);

            // Dealing with CountryName in ComboBox : we need to show the specific CountryName of the selected contact directly :
            string CountryName = clsCountry.Find(_Contact.CountryID).CountryName;
            cbCountries.SelectedIndex = cbCountries.FindStringExact(CountryName);


        }

        private void frmAddEditContact_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnkSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.png;*.jpeg;*.jpg;*.bmp;";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                pbContactImage.ImageLocation = path;
                lnkRemoveImage.Visible = true;
            }
        }

        private void lnkRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbContactImage.ImageLocation = null;
            lnkRemoveImage.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Find the CountryID from the comboBox (selected text ) 
            int CountryID = clsCountry.Find(cbCountries.Text).CountryID;

            // Load Data in the contact object :
            _Contact.FirstName = txtFirstName.Text;
            _Contact.LastName = txtLastName.Text;
            _Contact.Email = txtEmail.Text;
            _Contact.Phone = txtPhone.Text;
            _Contact.DateOfBirth = dtpDateOfBirth.Value;
            _Contact.CountryID = CountryID;
            _Contact.Address = txtAddress.Text;

            if (pbContactImage.ImageLocation != null)
            {
                _Contact.ImagePath = pbContactImage.ImageLocation;
            }
            else
                _Contact.ImagePath = null;

            if (_Contact.Save())
            {
                if (_Mode == enMode.AddNew)
                {
                    lblTitle.Text = "Edit Contact With ID = " + _Contact.ID;
                    lblContactID.Text = _Contact.ID.ToString();
                    _Mode = enMode.Update;
                }
                else
                    MessageBox.Show($"Contact [{_Contact.ID}] was updated successfully !");
            }
        }
    }
}
