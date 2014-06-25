using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Project_Kserh_Online;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Project_Kserh_Online___Client {
    public partial class Form1 : Form {
        #region Card UI Settings
        Size CardSize = new Size(80, 111);
        Color CardBackGroundColor = Color.WhiteSmoke;
        #endregion

        #region Game
            volatile bool Playing=false;
            List<CardStr> PlayerCards;
            List<CardStr> DownCards;
            ushort oppCards = 0;
        #endregion

        #region Client
            TcpClient Client;
            NetworkStream clientStream;
            Thread ClientThread;
        #endregion

        private delegate void StartGame();

        private delegate void ServerCommands();
        private delegate void ServerCardCommands(CardStr card);

        private delegate void ServerTextCommands(String text);
        private delegate void ServerInfo(Info gInfo);

        public Form1() {
            InitializeComponent();
        }

        private void SetUpGame() {
            PlayerCards = new List<CardStr>(6);
            DownCards = new List<CardStr>();
        }

        private void AddDownCard(CardStr card) {
            DownCards.Add(card);
            Card newCard = new Card(card);
            newCard.Size = CardSize;
            newCard.BackColor = CardBackGroundColor;
            newCard.Enabled = false;
            newCard.Location = new Point((CardSize.Width+5) * (DownCards.Count-1) + 10, 170);
            newCard.Name = "Downcard_" + DownCards.Count;
            newCard.player = Players.None;
            this.Controls.Add(newCard);
            newCard.BringToFront();
            if (DownCards.Count > 6) {
                for (int c = 1; c <= DownCards.Count; c++)
                {
                    this.Controls["Downcard_" + c].Location = new Point(10 + (int)((c - 1) * ((float)CardSize.Width * (float)(6 / (float)DownCards.Count))), 170);
                }
            }
        }

        private void TakeCard(CardStr card) {
            PlayerCards.Add(card);
            Card newCard = new Card(card);
            newCard.Size = CardSize;
            newCard.BackColor = CardBackGroundColor;
            newCard.Enabled = false;
            newCard.Click += new EventHandler(newCard_Click);
            newCard.Name = "Card_" + PlayerCards.Count;
            newCard.Location = new Point((CardSize.Width + 5) * (PlayerCards.Count - 1) + 10, 30);
            newCard.player = Players.Player1;
            this.Controls.Add(newCard);
        }

        void newCard_Click(object sender, EventArgs e) {
            CloseHandlers(); 
            CommandStruct cmd;
            cmd.CommandIndex = (ushort)NetworkCommands.oppPlayed;
            cmd.ExtraDataSize = Common.CardStructSize;
            SendCommandToServer(cmd, Common.SerializeStruct<CardStr>(((Card)sender).CardStuct));
            PlayerCards.Remove(((Card)sender).CardStuct);
            this.Controls.Remove((Control)sender);
        }

        private void AllowToPlay() {
            for (ushort c = 1; c <= 6; c++) {
                if (Controls.ContainsKey("Card_" + c)) {
                    Controls["Card_" + c].Enabled = true;
                }
            }
        }
        private void newRound()
        {
            oppCards = 0;
            PlayerCards.Clear();
            MessageBox.Show("New Round");
        }
        private void CloseHandlers() {
            for (ushort c = 1; c <= 6; c++) {
                if (Controls.ContainsKey("Card_" + c)) {
                    Controls["Card_" + c].Enabled = false;
                }
            }
        }

        private void SendCommandToServer(CommandStruct cmd, byte[] extra) {
            byte[] data = Common.SerializeStruct<CommandStruct>(cmd);
            clientStream.Write(data, 0, Common.CommandStructSize);
            if (cmd.ExtraDataSize > 0) {
                clientStream.Write(extra, 0, (int)cmd.ExtraDataSize);
                Console.WriteLine("Command send : {0}", Common.ParseEnumValue<NetworkCommands>(cmd.CommandIndex).ToString());
            }
        }
       
        private void oppTakeCard() {
            oppCards++;
            Card newCard = new Card();
            newCard.Size = CardSize;
            newCard.BackColor = CardBackGroundColor;
            newCard.Enabled = false;
            newCard.Hidden = true;

            newCard.Name = "oppCard_" + oppCards;
            newCard.Location = new Point((CardSize.Width + 5) * (oppCards - 1) + 10, 290);
            newCard.player = Players.Player2;
            this.Controls.Add(newCard);
        }
        private void oppPlayed()
        {
            removeControl("oppCard_" +( oppCards));
            oppCards--;
        }
        private void RemoveDownCards()
        {
            for (int i = DownCards.Count; i > 0; i--)
            {
                removeControl("Downcard_" + i);
            }
            DownCards.Clear();
        }
        private void UpdateInfo(Info gInfo)
        {
            gInfo.turn = (gInfo.turn == Players.Player1 ? Players.Player2 : Players.Player1);
            player_turn.Text = gInfo.turn.ToString();
            deck_left.Text = gInfo.deckLeft.ToString();
            player1_points.Text = gInfo.player2points.ToString();
            player1_cards.Text = gInfo.player2cards.ToString();
            player2_points.Text = gInfo.player1points.ToString();
            player2_cards.Text = gInfo.player1cards.ToString();
        }
         private void GameOver(Info gInfo){
             player_turn.Text = gInfo.turn.ToString();
             deck_left.Text = gInfo.deckLeft.ToString();
             player1_points.Text = gInfo.player2points.ToString();
             player1_cards.Text = gInfo.player2cards.ToString();
             player2_points.Text = gInfo.player1points.ToString();
             player2_cards.Text = gInfo.player1cards.ToString();

             Playing = false;

             MessageBox.Show("Score : " + gInfo.player2points + " - " + gInfo.player1points + "\n" + (gInfo.player2points > gInfo.player1points ? "Player 1" : "Player 2") + " wins ");
                      
         }
         private void writeToChat(string text) {
             _chat.Text+=("Player2:" + text+"\n");
         }
        private void ListeningClient(){
            try
            {
                Client.Connect(address.Text, int.Parse(port.Text));
            }
            catch(Exception e)
            {
                MessageBox.Show("Cannot connect \n" + e.Message);
                return;
            }
            CommandStruct cmd;
            cmd.CommandIndex = (ushort)NetworkCommands.Start;
            cmd.ExtraDataSize=0;

            byte[] buffer = Common.SerializeStruct<CommandStruct>(cmd);

            clientStream = Client.GetStream();
            clientStream.Write(buffer, 0, buffer.Length);
            int bytesRead;
            buffer = new byte[Common.CommandStructSize];
            byte[] extra;
            CardStr newCard;
            while (Playing) {
                bytesRead = clientStream.Read(buffer, 0, Common.CommandStructSize);
                cmd = Common.DeSerializeStruct<CommandStruct>(buffer);
                Console.WriteLine("Command received : {0}", Common.ParseEnumValue<NetworkCommands>(cmd.CommandIndex).ToString());
                switch (Common.ParseEnumValue<NetworkCommands>(cmd.CommandIndex)) {
                    case NetworkCommands.Start:
                        this.Invoke(new StartGame(this.SetUpGame));
                        break;
                    case NetworkCommands.TakeCard:
                        extra = new byte[cmd.ExtraDataSize];
                        clientStream.Read(extra,0,(int)cmd.ExtraDataSize);
                        newCard= Common.DeSerializeStruct<CardStr>(extra);
                        extra = null;
                        this.Invoke(new ServerCardCommands(this.TakeCard), new object[] { newCard });
                        break;
                    case NetworkCommands.oppTakenCard:
                        this.Invoke(new ServerCommands(this.oppTakeCard));
                        break;
                    case NetworkCommands.oppPlayed:
                        this.Invoke(new ServerCommands(this.oppPlayed));
                        break;
                    case NetworkCommands.AddDownCard:
                        extra = new byte[cmd.ExtraDataSize];
                        clientStream.Read(extra, 0, (int)cmd.ExtraDataSize);
                        newCard = Common.DeSerializeStruct<CardStr>(extra);
                        extra = null;
                        this.Invoke(new ServerCardCommands(this.AddDownCard), new object[] { newCard });
                        break;
                    case NetworkCommands.Play:
                        this.Invoke(new ServerCommands(this.AllowToPlay));
                        break;
                    case NetworkCommands.ClearDownCards:
                        this.Invoke(new ServerCommands(this.RemoveDownCards));
                        break;
                    case NetworkCommands.UpdateInfo:
                        extra = new byte[cmd.ExtraDataSize];
                        clientStream.Read(extra, 0, (int)cmd.ExtraDataSize);
                        this.Invoke(new ServerInfo(this.UpdateInfo),new object[] {Common.DeSerializeStruct<Info>(extra)});
                        extra = null;
                        break;
                    case NetworkCommands.newRound:
                        this.Invoke(new ServerCommands(this.newRound));
                        break;
                    case NetworkCommands.GameOver:
                        extra = new byte[cmd.ExtraDataSize];
                        clientStream.Read(extra, 0, (int)cmd.ExtraDataSize);
                        this.Invoke(new ServerInfo(this.UpdateInfo),new object[] {Common.DeSerializeStruct<Info>(extra)});
                        extra = null;
                        break;
                    case NetworkCommands.Disconnect:
                        clientStream.Close();
                        Client.Close();
                        return;
                    case NetworkCommands.ChatText:
                        extra = new byte[cmd.ExtraDataSize];
                        clientStream.Read(extra, 0, (int)cmd.ExtraDataSize);
                        this.Invoke(new ServerTextCommands(this.writeToChat), new object[] { (System.Text.Encoding.Default.GetString(extra)) });
                        break;
                }

            }

            clientStream.Flush();
        }
        private void removeControl(string name) {
            if (Controls[name].InvokeRequired) {

                Controls[name].Invoke(new Action<string>(removeControl), name);
                return;
            }
            this.Controls.RemoveByKey(name);
        }
        private void connectToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!Playing)
            {
                _chat_text.Enabled = _chat_send.Enabled = true;
                Client = new TcpClient();
                Playing = true;
                ClientThread = new Thread(new ThreadStart(ListeningClient));
                ClientThread.Start();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Playing)
            {
               
                    CommandStruct cmd;
                    cmd.CommandIndex = (ushort)NetworkCommands.Disconnect;
                    cmd.ExtraDataSize = 0;
                    SendCommandToServer(cmd, null);
               

            }
            Playing = false;
        }

        private void _chat_send_Click(object sender, EventArgs e) {
            this._chat.AppendText("Player1:" + _chat_text.Text+"\n");
            byte[] text = System.Text.Encoding.Default.GetBytes(_chat_text.Text);
            CommandStruct cmd;
            cmd.CommandIndex = (byte)NetworkCommands.ChatText;
            cmd.ExtraDataSize = (ushort)text.Length;
            SendCommandToServer(cmd, text);
            _chat_text.Text = null;
        }

        private void _chat_text_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Enter) {
                _chat_send_Click(sender, null);
            }
        }
    }
}
