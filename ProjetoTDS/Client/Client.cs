using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Client : Form
    {
        private const int PORT = 10000;
        private byte[] key;
        private byte[] iv;
        NetworkStream networkStream;
        ProtocolSI protocolSI;
        TcpClient client;
        AesCryptoServiceProvider aes;

        public Client()
        {
            InitializeComponent();
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, PORT);
            client = new TcpClient();
            client.Connect(endpoint);
            networkStream = client.GetStream();
            protocolSI = new ProtocolSI();
        }

        private string GerarChavePrivada(string pass)
        {
            byte[] salt = new byte[] { 0, 1, 0, 8, 3, 9, 9, 7 };

            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(pass, salt, 1000);

            byte[] key = pwdGen.GetBytes(16);
            string keyB64 = Convert.ToBase64String(key);
            return keyB64;
        }

        private string GerarIV(string pass)
        {
            byte[] salt = new byte[] { 1, 9, 3, 4, 1, 0, 5, 8 };
            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(pass, salt, 1000);

            byte[] iv = pwdGen.GetBytes(16);
            string iv64 = Convert.ToBase64String(iv);
            return iv64;
        }

        private string CifrarTexto(string txt)
        {
            byte[] textoDecifrado = Encoding.UTF8.GetBytes(txt);
            byte[] textoCifrado;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(textoDecifrado, 0, textoDecifrado.Length);
            cs.Close();
            textoCifrado = ms.ToArray();
            string textoCifradoB64 = Convert.ToBase64String(textoCifrado);
            return textoCifradoB64;
        }

        private string DefifrarTexto(string textoCifradoB64)
        {
            byte[] texoCifrado = Convert.FromBase64String(textoCifradoB64);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            byte[] textoDecifrado = new byte[ms.Length];
            int bytesLidos = 0;
            bytesLidos = cs.Read(textoDecifrado, 0, textoDecifrado.Length);
            cs.Close();
            string txtDecifrado = Encoding.UTF8.GetString(textoDecifrado, 0, textoDecifrado.Length);
            return txtDecifrado;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string msg = textBoxMessage.Text;
            textBoxMessage.Clear();
            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);
            networkStream.Write(packet, 0, packet.Length);
            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }
        }
        
        private void CloseClient()
        {
            byte[] eot = protocolSI.Make(ProtocolSICmdType.EOT);
            networkStream.Write(eot, 0, eot.Length);
            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            networkStream.Close();
            client.Close();
        }
                        //Exit
        private void buttonQuit_Click(object sender, EventArgs e)
        {
            CloseClient();
            this.Close();
        }


        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseClient();
        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
