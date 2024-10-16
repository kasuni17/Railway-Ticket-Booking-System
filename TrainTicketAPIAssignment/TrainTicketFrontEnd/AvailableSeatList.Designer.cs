namespace TrainTicketFrontEnd
{
    partial class AvailableSeatList
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
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            NICTextBox = new TextBox();
            SeatNumbersTextBox = new TextBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(283, 29);
            label1.Name = "label1";
            label1.Size = new Size(224, 34);
            label1.TabIndex = 0;
            label1.Text = "Available Seats";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(296, 107);
            label2.Name = "label2";
            label2.Size = new Size(62, 21);
            label2.TabIndex = 1;
            label2.Text = "label2";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.InfoText;
            button1.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(272, 364);
            button1.Name = "button1";
            button1.Size = new Size(218, 59);
            button1.TabIndex = 2;
            button1.Text = "Book Tickets";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // NICTextBox
            // 
            NICTextBox.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NICTextBox.Location = new Point(404, 220);
            NICTextBox.Name = "NICTextBox";
            NICTextBox.Size = new Size(245, 36);
            NICTextBox.TabIndex = 3;
            // 
            // SeatNumbersTextBox
            // 
            SeatNumbersTextBox.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SeatNumbersTextBox.Location = new Point(403, 276);
            SeatNumbersTextBox.Name = "SeatNumbersTextBox";
            SeatNumbersTextBox.Size = new Size(245, 36);
            SeatNumbersTextBox.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(258, 223);
            label3.Name = "label3";
            label3.Size = new Size(54, 27);
            label3.TabIndex = 5;
            label3.Text = "NIC";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(144, 279);
            label4.Name = "label4";
            label4.Size = new Size(168, 27);
            label4.TabIndex = 6;
            label4.Text = "Seat Numbers";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(194, 167);
            label5.Name = "label5";
            label5.Size = new Size(118, 27);
            label5.TabIndex = 7;
            label5.Text = "Schedule";
            label5.Click += label5_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(404, 167);
            label6.Name = "label6";
            label6.Size = new Size(118, 27);
            label6.TabIndex = 8;
            label6.Text = "Schedule";
            label6.Click += label6_Click;
            // 
            // AvailableSeatList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(SeatNumbersTextBox);
            Controls.Add(NICTextBox);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "AvailableSeatList";
            Text = "AvailableSeatList";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button button1;
        private TextBox NICTextBox;
        private TextBox SeatNumbersTextBox;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
    }
}