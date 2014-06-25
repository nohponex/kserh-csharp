namespace Project_Kserh_Online___Server {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.player2_cards = new System.Windows.Forms.Label();
            this.player2_points = new System.Windows.Forms.Label();
            this.player1_cards = new System.Windows.Forms.Label();
            this.player1_points = new System.Windows.Forms.Label();
            this.deck_left = new System.Windows.Forms.Label();
            this.player_turn = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._chat_send = new System.Windows.Forms.Button();
            this._chat_text = new System.Windows.Forms.TextBox();
            this._chat = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createServerToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(764, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // createServerToolStripMenuItem
            // 
            this.createServerToolStripMenuItem.Name = "createServerToolStripMenuItem";
            this.createServerToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.createServerToolStripMenuItem.Text = "Create Server";
            this.createServerToolStripMenuItem.Click += new System.EventHandler(this.CreateServer);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.player2_cards);
            this.groupBox1.Controls.Add(this.player2_points);
            this.groupBox1.Controls.Add(this.player1_cards);
            this.groupBox1.Controls.Add(this.player1_points);
            this.groupBox1.Controls.Add(this.deck_left);
            this.groupBox1.Controls.Add(this.player_turn);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(550, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 143);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // player2_cards
            // 
            this.player2_cards.AutoSize = true;
            this.player2_cards.Location = new System.Drawing.Point(63, 123);
            this.player2_cards.Name = "player2_cards";
            this.player2_cards.Size = new System.Drawing.Size(13, 13);
            this.player2_cards.TabIndex = 13;
            this.player2_cards.Text = "0";
            // 
            // player2_points
            // 
            this.player2_points.AutoSize = true;
            this.player2_points.Location = new System.Drawing.Point(63, 110);
            this.player2_points.Name = "player2_points";
            this.player2_points.Size = new System.Drawing.Size(13, 13);
            this.player2_points.TabIndex = 12;
            this.player2_points.Text = "0";
            // 
            // player1_cards
            // 
            this.player1_cards.AutoSize = true;
            this.player1_cards.Location = new System.Drawing.Point(63, 80);
            this.player1_cards.Name = "player1_cards";
            this.player1_cards.Size = new System.Drawing.Size(13, 13);
            this.player1_cards.TabIndex = 11;
            this.player1_cards.Text = "0";
            // 
            // player1_points
            // 
            this.player1_points.AutoSize = true;
            this.player1_points.Location = new System.Drawing.Point(63, 67);
            this.player1_points.Name = "player1_points";
            this.player1_points.Size = new System.Drawing.Size(13, 13);
            this.player1_points.TabIndex = 10;
            this.player1_points.Text = "0";
            // 
            // deck_left
            // 
            this.deck_left.AutoSize = true;
            this.deck_left.Location = new System.Drawing.Point(64, 33);
            this.deck_left.Name = "deck_left";
            this.deck_left.Size = new System.Drawing.Size(13, 13);
            this.deck_left.TabIndex = 9;
            this.deck_left.Text = "0";
            // 
            // player_turn
            // 
            this.player_turn.AutoSize = true;
            this.player_turn.Location = new System.Drawing.Point(48, 20);
            this.player_turn.Name = "player_turn";
            this.player_turn.Size = new System.Drawing.Size(13, 13);
            this.player_turn.TabIndex = 8;
            this.player_turn.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Cards left";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Cards";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Points";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Player 2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Cards";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Points";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Player 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Turn :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._chat);
            this.groupBox2.Controls.Add(this._chat_text);
            this.groupBox2.Controls.Add(this._chat_send);
            this.groupBox2.Location = new System.Drawing.Point(550, 179);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(210, 231);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chat";
            // 
            // _chat_send
            // 
            this._chat_send.Enabled = false;
            this._chat_send.Location = new System.Drawing.Point(129, 202);
            this._chat_send.Name = "_chat_send";
            this._chat_send.Size = new System.Drawing.Size(75, 23);
            this._chat_send.TabIndex = 4;
            this._chat_send.Text = "send";
            this._chat_send.UseVisualStyleBackColor = true;
            this._chat_send.Click += new System.EventHandler(this._chat_send_Click);
            // 
            // _chat_text
            // 
            this._chat_text.Enabled = false;
            this._chat_text.Location = new System.Drawing.Point(6, 176);
            this._chat_text.Name = "_chat_text";
            this._chat_text.Size = new System.Drawing.Size(198, 20);
            this._chat_text.TabIndex = 5;
            this._chat_text.KeyDown += new System.Windows.Forms.KeyEventHandler(this._chat_text_KeyDown);
            // 
            // _chat
            // 
            this._chat.BackColor = System.Drawing.SystemColors.Window;
            this._chat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._chat.Location = new System.Drawing.Point(6, 19);
            this._chat.Name = "_chat";
            this._chat.ReadOnly = true;
            this._chat.Size = new System.Drawing.Size(198, 155);
            this._chat.TabIndex = 7;
            this._chat.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 422);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Project Kserh Online - Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createServerToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label player2_cards;
        private System.Windows.Forms.Label player2_points;
        private System.Windows.Forms.Label player1_cards;
        private System.Windows.Forms.Label player1_points;
        private System.Windows.Forms.Label deck_left;
        private System.Windows.Forms.Label player_turn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox _chat_text;
        private System.Windows.Forms.Button _chat_send;
        private System.Windows.Forms.RichTextBox _chat;
    }
}

