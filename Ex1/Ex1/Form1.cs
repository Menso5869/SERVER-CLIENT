using System.Net.Sockets;
using System.Text;

namespace Ex1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void send_Click(object sender, EventArgs e)
        {
            sendMessage(richTextBox2);
        }

        private void Connect_click(object sender, EventArgs e)
        {
            Thread subprocess = new Thread(Connect2Server);
            subprocess.Start();
        }

        private void Disconnect_click(object sender, EventArgs e)
        {
            disconnectClient();
        }

        // UI handler functions
        bool UIavilable = true;
        private void clearTextboxFromSubprocess(RichTextBox textBox)
        {
            if (UIavilable)
            {
                Invoke(new MethodInvoker(() =>
                {
                    textBox.Text = "";
                }));
            }
        }
        private void write2TextboxFromSubprocess(RichTextBox textBox, string text)
        {
            if (UIavilable)
            {
                Invoke(new MethodInvoker(() =>
                {
                    if (UIavilable) textBox.Text += "\n" + text;
                }));
            }
        }
        private void enableButtonFromSubprocess(Button button)
        {
            if (UIavilable)
            {
                Invoke(new MethodInvoker(() =>
                {
                    button.Enabled = true;
                }));
            }
        }
        private void disableButtonFromSubprocess(Button button)
        {
            if (UIavilable)
            {
                Invoke(new MethodInvoker(() =>
                {
                    button.Enabled = false;
                }));
            }
        }

        private void UI_is_closing(object sender, FormClosingEventArgs e)
        {
            UIavilable = false;
        }

        //client functions
        TcpClient client = null;
        bool keepConnection = false;
        private void Connect2Server()
        {
            try
            {
                disableButtonFromSubprocess(button1);
                client = new TcpClient("10.48.184.202", 10001);
                if (client.Connected)
                {
                    keepConnection = true;
                    NetworkStream stream = client.GetStream();
                    int bytesRead = 0;
                    byte[] buffer = new byte[1024];
                    while (keepConnection && client.Connected)
                    {
                        if (!stream.DataAvailable)
                        {
                            bytesRead = 0;
                        }
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        else
                        {
                            string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            write2TextboxFromSubprocess(richTextBox1, text);
                        }
                    }
                }
                else
                {
                    write2TextboxFromSubprocess(richTextBox1, "connection failed");
                }
            }
            catch (Exception ex)
            {
                write2TextboxFromSubprocess(richTextBox1, ex.ToString());
            }
            finally
            {
                if (client != null && client.Connected)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Close();
                    client.Close();
                }
                enableButtonFromSubprocess(button1);
            }
        }
        private void sendMessage(RichTextBox textBox)
        {
            if (UIavilable && client != null && client.Connected)
            {
                string message = textBox.Text;
                clearTextboxFromSubprocess(textBox);

                NetworkStream stream = client.GetStream();
                byte[] bytes2send = Encoding.UTF8.GetBytes(message);
                stream.Write(bytes2send);
            }
        }
        private void disconnectClient()
        {
            keepConnection = false;
        }
    }
}
