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
    public partial class BookWindow : Form
    {
        IService server;
        int userID;
        int flightID;
        public BookWindow()
        {
            InitializeComponent();
        }
        public void setService(IService server)
        {
            this.server = server;
            BookWindow_Load();

        }
        public void setUserID(int userID)
        {
            this.userID = userID;
        }

        public void setFlightID(int flightID)
        {
            this.flightID = flightID;
        }
        public void BookWindow_Load()
        {
            numericUpDown1.Value = 0;
            numericUpDown1.Minimum = 0;
            int noAvailableSeats = server.findAvailableSeats(flightID);
            if (noAvailableSeats < 5)
            {
                numericUpDown1.Maximum = noAvailableSeats - 1;

                labelNoSeatsAvailable.Text = "Only " + noAvailableSeats.ToString() + " seats available!";
                labelNoSeatsAvailable.ForeColor = Color.Red;

            }
            else
            {
                numericUpDown1.Maximum = 5;
            }

            textBoxTourist1.Visible = false;
            textBoxTourist2.Visible = false;
            textBoxTourist3.Visible = false;
            textBoxTourist4.Visible = false;
            textBoxTourist5.Visible = false;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int nrTourists = Decimal.ToInt32(numericUpDown1.Value);
            List<TextBox> textBoxes = new List<TextBox>();
            textBoxes.Add(textBoxTourist1);
            textBoxes.Add(textBoxTourist2);
            textBoxes.Add(textBoxTourist3);
            textBoxes.Add(textBoxTourist4);
            textBoxes.Add(textBoxTourist5);
            for (int i = 0; i < textBoxes.Count; i++)
            {
                textBoxes[i].Visible = i < nrTourists;
            }
        }

        private void buttonBook_Click(object sender, EventArgs e)
        {
            List<TextBox> textBoxes = new List<TextBox>();
            textBoxes.Add(textBoxTourist1);
            textBoxes.Add(textBoxTourist2);
            textBoxes.Add(textBoxTourist3);
            textBoxes.Add(textBoxTourist4);
            textBoxes.Add(textBoxTourist5);
            if (textBoxClientName.Text.Length == 0)
            {
                MessageBox.Show("Client's Name must not be empty!");
            }
            else if (textBoxClientAddress.Text.Length == 0)
            {
                MessageBox.Show("Client's Adress must not be empty!");
            }
            else
            {
                int ok = 1;
                for (int i = 0; i < textBoxes.Count; i++)
                {
                    if (textBoxes[i].Visible && textBoxes[i].Text.Length == 0)
                    {
                        MessageBox.Show("Tourist's Name must not be empty!");
                        ok = 0;
                        break;
                    }
                }
                if (ok == 1)
                {
                    string clientName = textBoxClientName.Text.Trim();
                    string clientAdress = textBoxClientAddress.Text.Trim();
                    int purchaseID = server.savePurchase(clientName, clientAdress, userID, flightID);
                    int noTourists = 1;
                    for (int i = 0; i < textBoxes.Count; i++)
                    {
                        if (textBoxes[i].Visible)
                        {
                            server.saveTourist(textBoxes[i].Text.Trim(), purchaseID);
                            noTourists += 1;
                        }
                    }
                    server.updateFlightSeats(noTourists, flightID);
                    MessageBox.Show("Purchased approved!");
                    this.Close();
                    //form.Show();
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            //form.Show();

        }

     
    }
}
