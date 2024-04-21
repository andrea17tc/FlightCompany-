using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.service;

namespace Client
{
    public partial class Form1 : Form
    {
        private IService server;
        private SearchWindow searchWindow;
        public Form1(IService server)
        {
            this.server = server;
            InitializeComponent();
        }
        public void setSearchWindow(SearchWindow search) { 
            this.searchWindow = search;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            try
            {
                int userID = server.login(username, password, searchWindow);
                searchWindow.setUserID(userID);
                searchWindow.setService(server);
                searchWindow.Show();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
