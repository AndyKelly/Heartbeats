namespace HeartbeatsClient
{
    partial class HeartbeatsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeartbeatsForm));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SubmitAddress = new System.Windows.Forms.Button();
            this.clientAddressTextBox = new System.Windows.Forms.TextBox();
            this.xmlLocationTextBox = new System.Windows.Forms.TextBox();
            this.DebugTextBox = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Xml file Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Client address";
            // 
            // SubmitAddress
            // 
            this.SubmitAddress.Location = new System.Drawing.Point(293, 129);
            this.SubmitAddress.Name = "SubmitAddress";
            this.SubmitAddress.Size = new System.Drawing.Size(77, 20);
            this.SubmitAddress.TabIndex = 8;
            this.SubmitAddress.Text = "Submit";
            this.SubmitAddress.UseVisualStyleBackColor = true;
            this.SubmitAddress.Click += new System.EventHandler(this.SubmitAddress_Click_1);
            // 
            // clientAddressTextBox
            // 
            this.clientAddressTextBox.Location = new System.Drawing.Point(93, 77);
            this.clientAddressTextBox.Name = "clientAddressTextBox";
            this.clientAddressTextBox.Size = new System.Drawing.Size(277, 20);
            this.clientAddressTextBox.TabIndex = 9;
            // 
            // xmlLocationTextBox
            // 
            this.xmlLocationTextBox.Location = new System.Drawing.Point(93, 103);
            this.xmlLocationTextBox.Name = "xmlLocationTextBox";
            this.xmlLocationTextBox.Size = new System.Drawing.Size(277, 20);
            this.xmlLocationTextBox.TabIndex = 10;
            // 
            // DebugTextBox
            // 
            this.DebugTextBox.AutoSize = true;
            this.DebugTextBox.Location = new System.Drawing.Point(3, 152);
            this.DebugTextBox.Name = "DebugTextBox";
            this.DebugTextBox.Size = new System.Drawing.Size(42, 13);
            this.DebugTextBox.TabIndex = 11;
            this.DebugTextBox.Text = "Debug:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(358, 50);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // HeartbeatsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(385, 170);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.DebugTextBox);
            this.Controls.Add(this.xmlLocationTextBox);
            this.Controls.Add(this.clientAddressTextBox);
            this.Controls.Add(this.SubmitAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "HeartbeatsForm";
            this.Text = "Heartbeats";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SubmitAddress;
        private System.Windows.Forms.TextBox clientAddressTextBox;
        private System.Windows.Forms.TextBox xmlLocationTextBox;
        private System.Windows.Forms.Label DebugTextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;

    }
}

