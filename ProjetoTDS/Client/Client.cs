﻿using EI.SI;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Client : Form
    {
        private const int PORT = 10000;
        private byte[] key;
        private byte[] iv;
        private NetworkStream networkStream;
        private ProtocolSI protocolSI;
        private TcpClient client;
        private AesCryptoServiceProvider aes;

        public Client()
        {
            InitializeComponent();
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, PORT);
            client = new TcpClient();
            client.Connect(endpoint);
            networkStream = client.GetStream();
            protocolSI = new ProtocolSI();

            // Gera um random initialization vector (IV) e  key
            aes = new AesCryptoServiceProvider();
            iv = aes.IV;
            key = aes.Key;

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    int bytesRead = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                    switch (protocolSI.GetCmdType())
                    {
                        case ProtocolSICmdType.DATA:
                            string encryptedMessage = protocolSI.GetStringFromData();
                            string decryptedMessage = DecryptText(encryptedMessage);

                            // Use Invoke to marshal the UI update code to the UI thread
                            textBoxMensagemRecebida.Invoke((MethodInvoker)(() =>
                            {
                                textBoxMensagemRecebida.Text += decryptedMessage + Environment.NewLine;
                            }));
                            break;

                        case ProtocolSICmdType.ACK:
                            Console.WriteLine("Message sent successfully");
                            break;

                        case ProtocolSICmdType.EOT:
                            Console.WriteLine("Disconnected from server");
                            break;
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error receiving message: " + ex.Message);
            }
        }


        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message   = textBoxMessage.Text;         //Atualiza a mensagem para o texto da textbox

            string encryptedMessage = EncryptText(message);

            textBoxMessage.Clear();
            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, encryptedMessage);
            networkStream.Write(packet, 0, packet.Length);
        }

        //Encripta mensagem enviada
        private string EncryptText(string text)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(key,iv), CryptoStreamMode.Write))
                {
                    cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cs.Close();
                }
                encryptedBytes = ms.ToArray();
            }

            string encryptedText = Convert.ToBase64String(encryptedBytes);
            return encryptedText;
        }

        //Desencripta mensagem recebida
        private string DecryptText(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes;

            using (MemoryStream ms = new MemoryStream(encryptedBytes))
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    byte[] buffer = new byte[encryptedBytes.Length];
                    int bytesRead = cs.Read(buffer, 0, buffer.Length);//erro "Padding is invalid and cannot be removed." quando estão 2 ou mais clientes abertos
                    decryptedBytes = new byte[bytesRead];
                    Array.Copy(buffer, decryptedBytes, bytesRead);
                }
            }

            string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
            return decryptedText;
        }

        private void CloseClient()
        {
            byte[] eot = protocolSI.Make(ProtocolSICmdType.EOT);
            networkStream.Write(eot, 0, eot.Length);
            networkStream.Close();
            client.Close();
        }

        // Exit
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

        private void Client_Load(object sender, EventArgs e)
        {

        }

        private void textBoxMensagemEnviada_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelMensagemEnviada_Click(object sender, EventArgs e)
        {

        }

        private void labelMessage_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
