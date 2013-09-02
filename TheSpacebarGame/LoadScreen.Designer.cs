namespace TheSpacebarGame
{
    partial class LoadScreen
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.LobbyPicture = new System.Windows.Forms.PictureBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.chatWindow = new System.Windows.Forms.PictureBox();
            this.chatWindowTB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.LobbyPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chatWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 282);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(97, 20);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start the Game";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Visible = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // LobbyPicture
            // 
            this.LobbyPicture.Location = new System.Drawing.Point(13, 12);
            this.LobbyPicture.Name = "LobbyPicture";
            this.LobbyPicture.Size = new System.Drawing.Size(669, 264);
            this.LobbyPicture.TabIndex = 1;
            this.LobbyPicture.TabStop = false;
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(970, 282);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(65, 20);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.chatButton_Click);
            // 
            // chatWindow
            // 
            this.chatWindow.Location = new System.Drawing.Point(689, 13);
            this.chatWindow.Name = "chatWindow";
            this.chatWindow.Size = new System.Drawing.Size(346, 263);
            this.chatWindow.TabIndex = 5;
            this.chatWindow.TabStop = false;
            this.chatWindow.Click += new System.EventHandler(this.chatWindow_Click);
            // 
            // chatWindowTB
            // 
            this.chatWindowTB.Location = new System.Drawing.Point(689, 282);
            this.chatWindowTB.Name = "chatWindowTB";
            this.chatWindowTB.Size = new System.Drawing.Size(275, 20);
            this.chatWindowTB.TabIndex = 6;
            this.chatWindowTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatWindowTB_KeyDown);
            // 
            // LoadScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 309);
            this.Controls.Add(this.chatWindowTB);
            this.Controls.Add(this.chatWindow);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.LobbyPicture);
            this.Controls.Add(this.buttonStart);
            this.Name = "LoadScreen";
            this.Text = "LoadScreen";
            this.Load += new System.EventHandler(this.LoadScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LobbyPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chatWindow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox LobbyPicture;
        public System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.PictureBox chatWindow;
        private System.Windows.Forms.TextBox chatWindowTB;
    }
}