using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.model;
using Utils.service;

namespace Client
{
    public partial class SearchWindow : Form, IObserver
    {
        private IService server;
        private int userID;
        public SearchWindow()
        {
            InitializeComponent();
        }

        public void setService(IService server)
        {
            this.server = server;
            SearchWindow_Load();
        }

        public void setUserID(int userID)
        {
            this.userID = userID;
        }

        private void SearchWindow_Load()
        {
            List<string> destinations = server.findAllFlightDestinations().ToList();
            comboBoxDestination.Text = "Destination";
            comboBoxDestination.DataSource = destinations;
            comboBoxDestination.SelectedIndex = 0;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            List<Flight> flights = server.findAllAvailableFlights().ToList();
            dataGridView1.DataSource = flights;
            dataGridView1.Columns[4].Visible = false;

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            DateTime d = dateTimePicker1.Value.Date;
            string destination = comboBoxDestination.Text.Trim();

            List<Flight> flights = server.findAllFlightsByDestinationAndDate(destination, d).ToList();
            if (flights.Count == 0)
            {
                dataGridView1.Visible = false;
                
                labelAllFlights.Text = "No Flights Available For Your Date";
                labelAllFlights.Font = new Font("Calibri", 18);
                labelAllFlights.ForeColor = Color.Red;
                labelAllFlights.Padding = new Padding(2);

            }
            else
            {
                dataGridView1.DataSource = flights;
                dataGridView1.Columns[4].Visible = false;
            }

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void buttonBook_Click(object sender, EventArgs e)
        {
            string stringId = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            int id = int.Parse(stringId);
            if (id == 0)
            {
                MessageBox.Show("Select flight first!");
            }
            else
            {
                BookWindow form = new BookWindow();
                form.setFlightID(id);
                form.setUserID(userID);
                form.setService(server);
                form.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.logout(userID);
            this.Close();
            Environment.Exit(0);
        }

        public void update()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                SearchWindow_Load();
            });
        }
    }
}
