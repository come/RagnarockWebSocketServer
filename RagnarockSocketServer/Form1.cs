using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WatsonWebsocket;

namespace RagnarockSocketServer
{
    public partial class Form1 : Form
    {
        private List<ClientMetadata> clientList = new List<ClientMetadata>();
        private WatsonWsServer server;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ClientConnected(object sender, ConnectionEventArgs args)
        {
          //  Form1 form1 = (Form1)sender;
           // this.LogTextBox.Text += ("\n");
           // LogTextBox.Text += "Client Connected " + args.Client.ToString();
            Console.WriteLine("Client connected: " + args.Client.ToString());
            clientList.Add(args.Client);
        }

        private void ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
           // Form1 form1 = (Form1)sender;
           // this.LogTextBox.Text += ("\n");
           // this.LogTextBox.Text += "Client disconnected: " + args.Client.ToString();
            Console.WriteLine("Client disconnected: " + args.Client.ToString());
        }

        private void WriteTextbox(string Message) {
            LogTextBox.Text += "\n";
            LogTextBox.Text += Message;
        }


        private void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            this.Invoke((Action)(() => WriteTextbox("Message received from " + args.Client.ToString() + ": " + Encoding.UTF8.GetString(args.Data.Array))));
            Console.WriteLine("Message received from " + args.Client.ToString() + ": " + Encoding.UTF8.GetString(args.Data.Array));

            WatsonWsServer s = (WatsonWsServer)sender;
            clientList.ForEach((Action<ClientMetadata>)((c) => {
                server.SendAsync(c.Guid, args.Data);
            } ));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogTextBox.Text += ("\n");
            LogTextBox.Text += "Starting ...  " ;
            server = new WatsonWsServer("127.0.0.1", int.Parse(TB_port.Text), false);

            server.ClientConnected += ClientConnected;
            server.ClientDisconnected += ClientDisconnected;
            server.MessageReceived += MessageReceived;
            server.Start();
            LogTextBox.Text += ("\n");
            LogTextBox.Text += "Started !  " + (server.IsListening ? "Listening" : "error");
        }
    }
}
