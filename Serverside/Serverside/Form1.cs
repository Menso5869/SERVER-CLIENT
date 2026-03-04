using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serverside
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            disconnectServer();
        }

        private void UI_is_closing(object sender, FormClosingEventArgs e)
        {
            UIavilable = false;
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
        //server functions
        private TcpListener server = null;
        private List<TcpClient> clients = new List<TcpClient> { };
        bool keepConnections = true;
        private void ListenConnections()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 10001);
                server.Start();
                keepConnections = true;
                disableButtonFromSubprocess(button1);
                // Check if there's a pending connection before accepting
                while (keepConnections)
                {
                    if (server.Pending())
                    {
                        TcpClient client = server.AcceptTcpClient();
                        Thread clientTask = new Thread(new ParameterizedThreadStart(HandleMessages));
                        clients.Add(client);
                        clientTask.Start(client);
                    }
                    else
                    {
                        Thread.Sleep(100); // Sleep briefly to avoid high CPU usage
                    }
                }
            }
            catch (Exception ex)
            {
                write2TextboxFromSubprocess(richTextBox1, "Connection ERROR: " + ex.Message);
            }
            //finally
            try
            {
                Thread.Sleep(1000);//allow disconnections
                clients.Clear();
                if (server != null) server.Stop();
                enableButtonFromSubprocess(button1);
            }
            catch (Exception ex)
            {
                write2TextboxFromSubprocess(richTextBox1, "Disconnect ERROR:" + ex.Message);
            }
        }
        private void HandleMessages(object obj)
        {
            write2TextboxFromSubprocess(richTextBox1, "New client");
            // Cast object back to TcpClient
            TcpClient client = (TcpClient)obj;
            // Read data from the client
            NetworkStream stream = client.GetStream();
            try
            {
                while (keepConnections && client != null && client.Connected)  // Keep reading messages
                {
                    if (!stream.DataAvailable) // Avoid blocking if no data is present
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; //disconnected

                    string message = "From Server: " + Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    byte[] bytes2send = Encoding.UTF8.GetBytes(message);
                    stream.Write(bytes2send);
                }
                if (client != null && client.Connected)
                {
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                write2TextboxFromSubprocess(richTextBox1, "Client ERROR: " + ex.Message);
            }

        }
        private void disconnectServer()
        {
            keepConnections = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread subprocess1 = new Thread(ListenConnections);
            subprocess1.Start();
        }
    }
}
