namespace Ex1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            richTextBox2 = new RichTextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(76, 131);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(1042, 308);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(76, 463);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(1042, 92);
            richTextBox2.TabIndex = 2;
            richTextBox2.Text = "";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(192, 192, 255);
            button1.Location = new Point(915, 75);
            button1.Name = "button1";
            button1.Size = new Size(203, 40);
            button1.TabIndex = 3;
            button1.Text = "Connect to Server";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Connect_click;
            // 
            // button2
            // 
            button2.Location = new Point(987, 613);
            button2.Name = "button2";
            button2.Size = new Size(131, 40);
            button2.TabIndex = 4;
            button2.Text = "Send";
            button2.UseVisualStyleBackColor = true;
            button2.Click += send_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(255, 128, 128);
            button3.Location = new Point(715, 75);
            button3.Name = "button3";
            button3.Size = new Size(131, 40);
            button3.TabIndex = 5;
            button3.Text = "Disconnect";
            button3.UseVisualStyleBackColor = false;
            button3.Click += Disconnect_click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1242, 693);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(richTextBox2);
            Controls.Add(richTextBox1);
            Name = "Form1";
            Text = "Form1";
            FormClosing += UI_is_closing;
            ResumeLayout(false);
        }

        #endregion
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}
