namespace Client
{
    partial class Client
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelMessage = new System.Windows.Forms.Label();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.labelMensagemEnviada = new System.Windows.Forms.Label();
            this.textBoxMensagemEnviada = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(84, 240);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(151, 13);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Texto a enviar para o servidor:";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(87, 256);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(287, 62);
            this.textBoxMessage.TabIndex = 1;
            this.textBoxMessage.TextChanged += new System.EventHandler(this.textBoxMessage_TextChanged);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(414, 256);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Enviar";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(414, 295);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(75, 23);
            this.buttonQuit.TabIndex = 3;
            this.buttonQuit.Text = "Sair";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // labelMensagemEnviada
            // 
            this.labelMensagemEnviada.AutoSize = true;
            this.labelMensagemEnviada.Location = new System.Drawing.Point(84, 70);
            this.labelMensagemEnviada.Name = "labelMensagemEnviada";
            this.labelMensagemEnviada.Size = new System.Drawing.Size(151, 13);
            this.labelMensagemEnviada.TabIndex = 4;
            this.labelMensagemEnviada.Text = "Texto enviado para o servidor:";
            // 
            // textBoxMensagemEnviada
            // 
            this.textBoxMensagemEnviada.Location = new System.Drawing.Point(87, 86);
            this.textBoxMensagemEnviada.Multiline = true;
            this.textBoxMensagemEnviada.Name = "textBoxMensagemEnviada";
            this.textBoxMensagemEnviada.ReadOnly = true;
            this.textBoxMensagemEnviada.Size = new System.Drawing.Size(287, 133);
            this.textBoxMensagemEnviada.TabIndex = 5;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 363);
            this.Controls.Add(this.textBoxMensagemEnviada);
            this.Controls.Add(this.labelMensagemEnviada);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.labelMessage);
            this.Name = "Client";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Label labelMensagemEnviada;
        private System.Windows.Forms.TextBox textBoxMensagemEnviada;
    }
}

