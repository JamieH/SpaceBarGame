namespace TheSpacebarGame
{
    partial class Game
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
            this.scoreLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.countdownLabel = new System.Windows.Forms.Label();
            this.LeaderBoard = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 120F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.Location = new System.Drawing.Point(351, 9);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(166, 181);
            this.scoreLabel.TabIndex = 2;
            this.scoreLabel.Text = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 173);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(809, 107);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // countdownLabel
            // 
            this.countdownLabel.AutoSize = true;
            this.countdownLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countdownLabel.ForeColor = System.Drawing.Color.Lime;
            this.countdownLabel.Location = new System.Drawing.Point(380, 46);
            this.countdownLabel.Name = "countdownLabel";
            this.countdownLabel.Size = new System.Drawing.Size(98, 108);
            this.countdownLabel.TabIndex = 4;
            this.countdownLabel.Text = "3";
            // 
            // LeaderBoard
            // 
            this.LeaderBoard.BackColor = System.Drawing.SystemColors.Control;
            this.LeaderBoard.FormattingEnabled = true;
            this.LeaderBoard.Location = new System.Drawing.Point(13, 9);
            this.LeaderBoard.Name = "LeaderBoard";
            this.LeaderBoard.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.LeaderBoard.Size = new System.Drawing.Size(129, 160);
            this.LeaderBoard.TabIndex = 99999;
            this.LeaderBoard.TabStop = false;
            this.LeaderBoard.UseTabStops = false;
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 292);
            this.Controls.Add(this.LeaderBoard);
            this.Controls.Add(this.countdownLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.scoreLabel);
            this.Name = "Game";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Game_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label countdownLabel;
        private System.Windows.Forms.ListBox LeaderBoard;
    }
}

