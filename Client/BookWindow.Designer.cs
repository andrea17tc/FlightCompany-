namespace Client
{
    partial class BookWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxClientName = new System.Windows.Forms.TextBox();
            this.textBoxClientAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonBook = new System.Windows.Forms.Button();
            this.textBoxTourist1 = new System.Windows.Forms.TextBox();
            this.textBoxTourist2 = new System.Windows.Forms.TextBox();
            this.textBoxTourist3 = new System.Windows.Forms.TextBox();
            this.textBoxTourist4 = new System.Windows.Forms.TextBox();
            this.textBoxTourist5 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelNoSeatsAvailable = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(304, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Book Flight";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Client\'s Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(82, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Client\'s Addres";
            // 
            // textBoxClientName
            // 
            this.textBoxClientName.Location = new System.Drawing.Point(85, 106);
            this.textBoxClientName.Name = "textBoxClientName";
            this.textBoxClientName.Size = new System.Drawing.Size(202, 22);
            this.textBoxClientName.TabIndex = 3;
            // 
            // textBoxClientAddress
            // 
            this.textBoxClientAddress.Location = new System.Drawing.Point(85, 209);
            this.textBoxClientAddress.Name = "textBoxClientAddress";
            this.textBoxClientAddress.Size = new System.Drawing.Size(202, 22);
            this.textBoxClientAddress.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(85, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Number of tourists";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(205, 289);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(35, 22);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // buttonBook
            // 
            this.buttonBook.Location = new System.Drawing.Point(124, 352);
            this.buttonBook.Name = "buttonBook";
            this.buttonBook.Size = new System.Drawing.Size(75, 23);
            this.buttonBook.TabIndex = 7;
            this.buttonBook.Text = "Book";
            this.buttonBook.UseVisualStyleBackColor = true;
            this.buttonBook.Click += new System.EventHandler(this.buttonBook_Click);
            // 
            // textBoxTourist1
            // 
            this.textBoxTourist1.Location = new System.Drawing.Point(422, 106);
            this.textBoxTourist1.Name = "textBoxTourist1";
            this.textBoxTourist1.Size = new System.Drawing.Size(167, 22);
            this.textBoxTourist1.TabIndex = 8;
            // 
            // textBoxTourist2
            // 
            this.textBoxTourist2.Location = new System.Drawing.Point(422, 170);
            this.textBoxTourist2.Name = "textBoxTourist2";
            this.textBoxTourist2.Size = new System.Drawing.Size(167, 22);
            this.textBoxTourist2.TabIndex = 9;
            // 
            // textBoxTourist3
            // 
            this.textBoxTourist3.Location = new System.Drawing.Point(422, 248);
            this.textBoxTourist3.Name = "textBoxTourist3";
            this.textBoxTourist3.Size = new System.Drawing.Size(167, 22);
            this.textBoxTourist3.TabIndex = 10;
            // 
            // textBoxTourist4
            // 
            this.textBoxTourist4.Location = new System.Drawing.Point(422, 318);
            this.textBoxTourist4.Name = "textBoxTourist4";
            this.textBoxTourist4.Size = new System.Drawing.Size(167, 22);
            this.textBoxTourist4.TabIndex = 11;
            // 
            // textBoxTourist5
            // 
            this.textBoxTourist5.Location = new System.Drawing.Point(422, 396);
            this.textBoxTourist5.Name = "textBoxTourist5";
            this.textBoxTourist5.Size = new System.Drawing.Size(167, 22);
            this.textBoxTourist5.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(48, 445);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelNoSeatsAvailable
            // 
            this.labelNoSeatsAvailable.AutoSize = true;
            this.labelNoSeatsAvailable.Location = new System.Drawing.Point(85, 263);
            this.labelNoSeatsAvailable.Name = "labelNoSeatsAvailable";
            this.labelNoSeatsAvailable.Size = new System.Drawing.Size(0, 16);
            this.labelNoSeatsAvailable.TabIndex = 14;
            // 
            // BookWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 501);
            this.Controls.Add(this.labelNoSeatsAvailable);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxTourist5);
            this.Controls.Add(this.textBoxTourist4);
            this.Controls.Add(this.textBoxTourist3);
            this.Controls.Add(this.textBoxTourist2);
            this.Controls.Add(this.textBoxTourist1);
            this.Controls.Add(this.buttonBook);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxClientAddress);
            this.Controls.Add(this.textBoxClientName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BookWindow";
            this.Text = "BookWindow";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxClientName;
        private System.Windows.Forms.TextBox textBoxClientAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button buttonBook;
        private System.Windows.Forms.TextBox textBoxTourist1;
        private System.Windows.Forms.TextBox textBoxTourist2;
        private System.Windows.Forms.TextBox textBoxTourist3;
        private System.Windows.Forms.TextBox textBoxTourist4;
        private System.Windows.Forms.TextBox textBoxTourist5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelNoSeatsAvailable;
    }
}