using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace ServerEX3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void StartServer(object sender, EventArgs e)
        {
            Thread subprocess1 = new Thread(ListenConnections);
            subprocess1.Start();
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
        // Map each connected client to the list of topics they subscribed to
        private Dictionary<TcpClient, List<string>> clientSubscriptions = new Dictionary<TcpClient, List<string>>();
        bool keepConnections = true;
        private void ListenConnections()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 10005);
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
            // Cast object back to TcpClient
            TcpClient client = (TcpClient)obj;
            // Read data from the client
            NetworkStream stream = client.GetStream();
            try
            {
                //First message is the name
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
               
                string Name = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                write2TextboxFromSubprocess(richTextBox1, "New Client: "+ Name);

                // Initialize subscription list for this client
                lock (clientSubscriptions)
                {
                    if (!clientSubscriptions.ContainsKey(client))
                        clientSubscriptions[client] = new List<string>();
                }

                string[] topics = new string[] { "#Sports", "#Trains", "#Autism", "#Chiikawa", "#Food", "#Videogames" };
                string subscriptions = string.Join("\n", topics);
                NetworkStream stream3 = client.GetStream();
                byte[] byte2send = Encoding.UTF8.GetBytes("Topics:\n"+subscriptions);
                stream3.Write(byte2send);

                while (keepConnections && client != null && client.Connected)  // Keep reading messages
                {
                    if (!stream.DataAvailable) // Avoid blocking if no data is present
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; //disconnected
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (message[0] == '0')
                    {
                        //Mensaje
                        foreach (var eachClient in clients)
                        {
                            try
                            {
                                NetworkStream stream2 = eachClient.GetStream();
                                byte[] byte2send1 = Encoding.UTF8.GetBytes(Name + ": " + message.Substring(2));
                                stream2.Write(byte2send1);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else if (message[0] == '1')
                    {
                        // Subscribe
                        string subscribe_topic = message.Substring(2).Trim();
                        if (topics.Contains(subscribe_topic))
                        {
                            lock (clientSubscriptions)
                            {
                                if (!clientSubscriptions.TryGetValue(client, out var list))
                                {
                                    list = new List<string>();
                                    clientSubscriptions[client] = list;
                                }
                                if (!list.Any(t => t.Trim() == subscribe_topic))
                                {
                                    list.Add(subscribe_topic);
                                    write2TextboxFromSubprocess(richTextBox1, Name + " subscribed to " + subscribe_topic);
                                    // send confirmation to client
                                    try
                                    {
                                        byte[] conf = Encoding.UTF8.GetBytes("Subscribed to: " + subscribe_topic);
                                        stream.Write(conf, 0, conf.Length);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                byte[] err = Encoding.UTF8.GetBytes("Unknown topic: " + subscribe_topic);
                                stream.Write(err, 0, err.Length);
                            }
                            catch { }
                        }

                    }
                    else if (message[0] == '2')
                    {
                        // Unsubscribe
                        string unsubscribe_topic = message.Substring(2).Trim();
                        if (topics.Contains(unsubscribe_topic))
                        {
                            lock (clientSubscriptions)
                            {
                                if (clientSubscriptions.TryGetValue(client, out var list) && list.Any(t => t.Trim() == unsubscribe_topic))
                                {
                                    // remove any entries that match when trimmed
                                    list.RemoveAll(t => t.Trim() == unsubscribe_topic);
                                    write2TextboxFromSubprocess(richTextBox1, Name + " unsubscribed from " + unsubscribe_topic);
                                    try
                                    {
                                        byte[] conf = Encoding.UTF8.GetBytes("Unsubscribed from: " + unsubscribe_topic);
                                        stream.Write(conf, 0, conf.Length);
                                    }
                                    catch { }
                                }
                                else
                                {
                                    try
                                    {
                                        byte[] err = Encoding.UTF8.GetBytes("Not subscribed to: " + unsubscribe_topic);
                                        stream.Write(err, 0, err.Length);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                byte[] err = Encoding.UTF8.GetBytes("Unknown topic: " + unsubscribe_topic);
                                stream.Write(err, 0, err.Length);
                            }
                            catch { }
                        }
                    }

                    else if (message[0] == '3')
                    {
                        // Topic message
                        string[] parts = message.Substring(2).Split(new char[] { ':' }, 2);
                        if (parts.Length == 2)
                        {
                            string topic = parts[0].Trim();
                            string topicMessage = parts[1].Trim();
                            if (topics.Contains(topic))
                            {
                                // Require that the sender is subscribed to the topic before publishing
                                bool senderSubscribed = false;
                                lock (clientSubscriptions)
                                {
                                    if (clientSubscriptions.TryGetValue(client, out var senderList))
                                    {
                                        senderSubscribed = senderList.Any(t => t.Trim() == topic);
                                    }
                                }

                                if (!senderSubscribed)
                                {
                                    try
                                    {
                                        byte[] err = Encoding.UTF8.GetBytes("Not subscribed to topic, cannot publish: " + topic);
                                        stream.Write(err, 0, err.Length);
                                    }
                                    catch { }
                                    continue; // skip publishing
                                }

                                // Send to clients subscribed to this topic
                                lock (clientSubscriptions)
                                {
                                    foreach (var kvp in clientSubscriptions)
                                    {
                                        try
                                        {
                                            // compare trimmed subscription entries
                                            if (kvp.Key != null && kvp.Key.Connected && kvp.Value.Any(t => t.Trim() == topic))
                                            {
                                                NetworkStream stream2 = kvp.Key.GetStream();
                                                byte[] byte2send1 = Encoding.UTF8.GetBytes("[" + topic + "] " + Name + ": " + topicMessage);
                                                stream2.Write(byte2send1, 0, byte2send1.Length);
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    byte[] err = Encoding.UTF8.GetBytes("Unknown topic: " + topic);
                                    stream.Write(err, 0, err.Length);
                                }
                                catch { }
                            }
                        }
                    }
                    // Log any other messages to UI
                    write2TextboxFromSubprocess(richTextBox2, Name + ": " + message);
                    


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

        private void StopServer(object sender, EventArgs e)
        {
            disconnectServer();
        }
    }
}
