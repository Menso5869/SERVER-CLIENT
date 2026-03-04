namespace ServerEX3
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
            button1 = new Button();
            button2 = new Button();
            richTextBox1 = new RichTextBox();
            richTextBox2 = new RichTextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(128, 255, 128);
            button1.Location = new Point(140, 49);
            button1.Name = "button1";
            button1.Size = new Size(173, 40);
            button1.TabIndex = 0;
            button1.Text = "Start Server";
            button1.UseVisualStyleBackColor = false;
            button1.Click += StartServer;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 128, 128);
            button2.Location = new Point(590, 49);
            button2.Name = "button2";
            button2.Size = new Size(170, 40);
            button2.TabIndex = 1;
            button2.Text = "Stop Server";
            button2.UseVisualStyleBackColor = false;
            button2.Click += StopServer;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(192, 192, 255);
            richTextBox1.Location = new Point(45, 125);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(799, 316);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = Color.FromArgb(255, 224, 192);
            richTextBox2.Location = new Point(45, 489);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(799, 308);
            richTextBox2.TabIndex = 3;
            richTextBox2.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(879, 834);
            Controls.Add(richTextBox2);
            Controls.Add(richTextBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
    }
}
