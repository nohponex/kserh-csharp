using System;

using System.Collections.Generic;

using System.Drawing;

using System.Windows.Forms;

using System.IO;
using System.Net;
using System.Net.Sockets;
using Project_Kserh_Online;
using System.Threading;
namespace Project_Kserh_Online___Server {
    public partial class Form1 : Form {
        #region Card UI Settings
        Size CardSize = new Size(80, 111);
        Color CardBackGroundColor  = Color.WhiteSmoke;
        Info gInfo;
        #endregion

        # region Game

        List<CardStr> PlayerCards;
        Deck Deck; //Server Only
        //CardStr[,] PlayersCards; // Server Only
        List<CardStr> DownCards;
        ushort oppCards = 0;
        volatile Boolean Playing = false;
        Players PlayerTurn;
        ushort[] Scores;
        ushort[] CardCount;
        #endregion
        
        #region Server
        TcpListener Server;
        Thread ServerThread;
        TcpClient Client;
        NetworkStream clientStream;
        Thread clientThread;
        #endregion



        private delegate void ServerCommands();
        private delegate void ServerTextCommands(String text);
        public Form1() {
            InitializeComponent();
        }

        private void SetUpGame() {
            if (Deck == null) {
                Deck = new Deck();
            }
            Deck.Setup();

            PlayerCards = new List<CardStr>(6);

            //PlayersCards= new CardStr[2,6];
            DownCards= new List<CardStr>();

            PlayerTurn = Players.Player1;
            Scores = new ushort[2];
            Scores[0] = Scores[1] = 0;
            SetupRound();
        }

        private void CreateServer(object sender, EventArgs e) {
            if (!Playing)
            {
                _chat_text.Enabled = _chat_send.Enabled = true;
                Playing = true;
                this.Text += " {listeing at port 1821}";
                Server = new TcpListener(IPAddress.Any, 1821);
                ServerThread = new Thread(new ThreadStart(ListenForClient));
                ServerThread.Start();
            }
        }

        private void ListenForClient() {
            Server.Start();
            Client = Server.AcceptTcpClient();

            clientThread = new Thread(new ThreadStart(HandleClient));
            clientThread.Start();
        }

        private void AddDownCard(CardStr card) {
            DownCards.Add(card);
            Card newCard = new Card(card);
            newCard.Size = CardSize;
            newCard.BackColor = CardBackGroundColor;
            newCard.Enabled = false;
            newCard.Location = new Point((CardSize.Width + 5) * (DownCards.Count - 1) + 10, 170);
            newCard.Name = "Downcard_" + DownCards.Count;
            newCard.player = Players.None;
            addControl(newCard);
            tofrontControl("Downcard_" + DownCards.Count);
            if (DownCards.Count > 6) {
                for (int c = 1; c <= DownCards.Count; c++) {
                    relocateControl("Downcard_" + c, new Point(10 + (int)((c - 1) * ((float)CardSize.Width * (float)(6 / (float)DownCards.Count))), 170));
                }
            }
        }

        private void TakeCard(CardStr card) {
            PlayerCards.Add(card);
            Card newCard = new Card(card);
            newCard.Size = CardSize;
            newCard.BackColor = CardBackGroundColor;
            newCard.Enabled = false;
            newCard.Name = "Card_" + PlayerCards.Count;
            newCard.Index = (ushort)(PlayerCards.Count - 1);
            newCard.Click += new EventHandler(newCard_Click);
            newCard.Location = new Point((CardSize.Width + 5) * (PlayerCards.Count - 1) + 10, 30);
            newCard.player = Players.Player1;
            addControl(newCard);
        }

        void newCard_Click(object sender, EventArgs e) {
            CloseHandlers();
            PlayerCards.Remove(((Card)sender).CardStuct);
            CardPlayed(PlayerTurn, ((Card)sender).CardStuct);
            this.Controls.Remove((Control)sender);
        }

        private void CardPlayed(Players player, CardStr card) {
            AddDownCard(card);
            
            if (player == Players.Player1) {
                CommandStruct cmd;
                cmd.CommandIndex = (ushort)NetworkCommands.oppPlayed;
                cmd.ExtraDataSize = 0;
                SendCommandToClient(cmd, null);

                cmd.CommandIndex = (ushort)NetworkCommands.AddDownCard;
                cmd.ExtraDataSize = Common.CardStructSize;
                SendCommandToClient(cmd, Common.SerializeStruct<CardStr>(card));
            } else {
                removeControl("oppCard_" + oppCards);
                oppCards--;

                CommandStruct cmd;
                cmd.CommandIndex = (ushort)NetworkCommands.AddDownCard;
                cmd.ExtraDataSize = Common.CardStructSize;
                SendCommandToClient(cmd, Common.SerializeStruct<CardStr>(card));
            }

            AfterPlayedCalculations(player, card);
        }
            private void removeControl(string name){
                  if (Controls[name].InvokeRequired){
        
              Controls[name].Invoke(new Action<string>(removeControl), name);
                return;
            }      
                this.Controls.RemoveByKey(name);
            }
            private void relocateControl(string name,Point pt)
            {
                if (Controls[name].InvokeRequired)
                {

                    Controls[name].Invoke(new Action<string, Point>(relocateControl), name, pt);
                    return;
                }
                this.Controls[name].Location = pt;
            }
            private void tofrontControl(string name)
            {
                if (Controls[name].InvokeRequired)
                {

                    Controls[name].Invoke(new Action<string>(tofrontControl), name);
                    return;
                }
                this.Controls[name].BringToFront();
            }
            private void addControl(Control c)
            {
                if (this.InvokeRequired)
                {

                    this.Invoke(new Action<Control>(addControl), c);
                    return;
                }
                this.Controls.Add(c);
            }
        private void RemoveDownCards() {
            for (int i = DownCards.Count ; i > 0; i--) {
                removeControl("Downcard_" + i);
            }
            DownCards.Clear();
        }
        private void AfterPlayedCalculations(Players player, CardStr card) {
            CommandStruct cmd;
            if (DownCards.Count > 1) {
                if (DownCards[DownCards.Count - 2].rank == card.rank) {
                    if (DownCards.Count == 2) {
                        if (card.rank == Ranks.Jack) {
                            AddScore(player, 20, DownCards.Count);
                        } else {
                            AddScore(player, 10, DownCards.Count);
                        }
                    } else {
                        AddScore(player, CalculateDownCardPoints(), DownCards.Count);
                    }
                    RemoveDownCards();
                    cmd.CommandIndex = (ushort)NetworkCommands.ClearDownCards;
                    cmd.ExtraDataSize = 0;
                    SendCommandToClient(cmd, null);
                } else {
                    if (card.rank == Ranks.Jack) {
                        AddScore(player, CalculateDownCardPoints(), DownCards.Count);
                        RemoveDownCards();
                        cmd.CommandIndex = (ushort)NetworkCommands.ClearDownCards;
                        cmd.ExtraDataSize = 0;
                        SendCommandToClient(cmd, null);
                    }
                }
            }
            NextPlayer();
        }
        private ushort CalculateDownCardPoints() {
            ushort Sum = 0;
            foreach (CardStr c in DownCards) {
                if (c.rank == Ranks.ten || c.rank == Ranks.Jack || c.rank == Ranks.King || c.rank == Ranks.Queen || c.rank == Ranks.Ace) {
                    Sum += 1;
                }
                if (c.rank == Ranks.ten && c.suit == Suit.Diamonds) {
                    Sum += 1;
                }
                if (c.rank == Ranks.two && c.suit == Suit.Clubs) {
                    Sum += 1;
                }
            }
            return Sum;
        }

        private void AddScore(Players player, ushort score, int cards) {
            if (player == Players.Player1) {
                Scores[0] += score;
                CardCount[0] += (ushort)cards;
            } else if (player == Players.Player2) {
                Scores[1] += score;
                CardCount[1] += (ushort)cards;
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
            addControl(newCard);
        }

        private void SetupRound() {
            CardCount=new ushort[2];
            CardCount[0]=CardCount[1]=0;
            Deck.Shuffle();
            PlayerCards.Clear();
            oppCards = 0;
            for (ushort c = 0; c < 4; c++) {
                CardStr newCard = Deck.Pop();
                AddDownCard(newCard);
                CommandStruct cmd;
                cmd.CommandIndex = (ushort)NetworkCommands.AddDownCard;
                cmd.ExtraDataSize = Common.CardStructSize;
                SendCommandToClient(cmd, Common.SerializeStruct<CardStr>(newCard));
            }
            Shuffle();
            CloseHandlers();
            NextPlayer();
        }
        private void Shuffle() {
            for (ushort c = 0; c < 6; c++) {
                for (ushort p = 0; p < 2; p++) {
                    if (Deck.Index == 52) break;
                    CardStr newCard = Deck.Pop();
                    //PlayersCards[p, c] = newCard;
                    if (p == 0) {
                        TakeCard(newCard);
                        CommandStruct cmd;
                        cmd.CommandIndex = (ushort)NetworkCommands.oppTakenCard;
                        cmd.ExtraDataSize = 0;
                        SendCommandToClient(cmd, null);
                    }else{
                        oppTakeCard();
                        CommandStruct cmd;
                        cmd.CommandIndex = (ushort)NetworkCommands.TakeCard;
                        cmd.ExtraDataSize = Common.CardStructSize;
                        SendCommandToClient(cmd, Common.SerializeStruct<CardStr>(newCard));
                    }
                }
            }
        }
        private void SendCommandToClient(CommandStruct cmd,byte[] extra) {
            byte[] data = Common.SerializeStruct<CommandStruct>(cmd);
            clientStream.Write(data, 0, Common.CommandStructSize);
            if (cmd.ExtraDataSize > 0) {
                clientStream.Write(extra, 0, (int)cmd.ExtraDataSize);
                Console.WriteLine("Command send : {0}", Common.ParseEnumValue<NetworkCommands>(cmd.CommandIndex).ToString());
            }
        }
        private void ChangeText(Control c,string t)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new Action<Control, string>(ChangeText), c, t);
                return;
            }
            c.Text = t;
        }
        private void UpdateInfo()
        {
            ChangeText(player_turn, gInfo.turn.ToString());

            ChangeText(deck_left, gInfo.deckLeft.ToString());

            ChangeText(player1_points, gInfo.player1points.ToString());

            ChangeText(player1_cards, gInfo.player1cards.ToString());

            ChangeText(player2_points, gInfo.player2points.ToString());

            ChangeText(player2_cards, gInfo.player2cards.ToString());
        }
        private void NextPlayer() {

            CommandStruct cmd;
            Console.WriteLine("opp = {0}   pl = {1}", oppCards, PlayerCards.Count);
            if (oppCards == 0 && PlayerCards.Count == 0)
            {
                if (Deck.Index < 52)
                {
                    Shuffle();
                }
                else
                {
                    if (CardCount[0] >= CardCount[1])
                    {
                        Scores[0] += 3;
                    }
                    else
                    {
                        Scores[1] += 3;
                    }
                    AddScore(PlayerTurn, CalculateDownCardPoints(), DownCards.Count);
                    RemoveDownCards();

                    if (Scores[0] >= 51 || Scores[1] >= 51)
                    {
                        cmd.CommandIndex = (ushort)NetworkCommands.GameOver;
                        cmd.ExtraDataSize = Common.InfoStructSize;
                        ;
                        gInfo.deckLeft = (ushort)(52 - Deck.Index);
                        gInfo.turn = PlayerTurn;
                        gInfo.player1points = Scores[0];
                        gInfo.player1cards = CardCount[0];
                        gInfo.player2points = Scores[1];
                        gInfo.player2cards = CardCount[1];

                        SendCommandToClient(cmd, Common.SerializeStruct<Info>(gInfo));

                        MessageBox.Show("Score : " + Scores[0] + " - " + Scores[1] + "\n" + (Scores[0] > Scores[1] ? "Player 1" : "Player 2") + " wins ");
                        Playing = false;
                        return;
                    }
                    else
                    {
                        cmd.CommandIndex = (ushort)NetworkCommands.newRound;
                        cmd.ExtraDataSize = 0;
                        SendCommandToClient(cmd, null);
                        Deck.Setup();
                        Deck.Shuffle();
                        SetupRound();
                    }
                }
            }
            PlayerTurn = (PlayerTurn == Players.Player1 ? Players.Player2 : Players.Player1);
            gInfo.deckLeft = (ushort)(52 - Deck.Index);
            gInfo.turn = PlayerTurn;
            gInfo.player1points = Scores[0];
            gInfo.player1cards = CardCount[0];
            gInfo.player2points = Scores[1];
            gInfo.player2cards = CardCount[1];
            UpdateInfo();
            cmd.CommandIndex = (ushort)NetworkCommands.UpdateInfo;
            cmd.ExtraDataSize = Common.InfoStructSize;
            SendCommandToClient(cmd, Common.SerializeStruct<Info>(gInfo));
            if (PlayerTurn == Players.Player1) {
                AllowToPlay();
            } else {
                CloseHandlers();
                cmd.CommandIndex = (ushort)NetworkCommands.Play;
                cmd.ExtraDataSize = 0;
                SendCommandToClient(cmd, null);
            }
        }
        private void SetEnabled(string name, bool value) {
            if (Controls[name].InvokeRequired) {

                Controls[name].Invoke(new Action<string,bool>(SetEnabled), name, value);
                return;
            }
            Controls[name].Enabled = value;
        }

        private void AllowToPlay(){
            for (ushort c = 1; c <= 6; c++) {
                if (Controls.ContainsKey("Card_" + c)) {
                    SetEnabled("Card_" + c, true);
                }
            }
        }
        private void CloseHandlers() {
            for (ushort c = 1; c <= 6; c++) {
                if (Controls.ContainsKey("Card_" + c)) {
                    SetEnabled("Card_" + c, false);
                }
            }
        }
        private void writeToChat(string text) {
            _chat.AppendText("Player2:" + text+"\n");
        }
        private void HandleClient() {
            //TcpClient tcpClient = (TcpClient)client;
            clientStream = Client.GetStream();

            byte[] data = new byte[Common.CommandStructSize];
            int bytesRead;
            byte[] extra;
            while (Playing) {
                bytesRead = 0;

                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(data, 0, Common.CommandStructSize);
                    //String t = System.Text.ASCIIEncoding.Default.GetString(message, 0, bytesRead);
                
                    CommandStruct cmd = Common.DeSerializeStruct<CommandStruct>(data);
                    Console.WriteLine("Command received : {0}",Common.ParseEnumValue<NetworkCommands>(cmd.CommandIndex).ToString());
                    switch (Common.ParseEnumValue<NetworkCommands>(cmd.CommandIndex)) {
                        case NetworkCommands.Start:
                            clientStream.Write(data, 0, Common.CommandStructSize);
                            this.Invoke(new ServerCommands(this.SetUpGame));
                            //this.Invoke(new ServerCommands(NextPlayer));
                            break;
                        case NetworkCommands.oppPlayed:
                            extra = new byte[cmd.ExtraDataSize];
                            clientStream.Read(extra, 0, (int)cmd.ExtraDataSize);
                            CardPlayed(Players.Player2, Common.DeSerializeStruct<CardStr>(extra));
                            extra = null;
                            break;
                        case NetworkCommands.Disconnect:
                            Playing = false;
                            return;
                        case NetworkCommands.ChatText:
                            extra = new byte[cmd.ExtraDataSize];
                            clientStream.Read(extra, 0, (int)cmd.ExtraDataSize);
                            this.Invoke(new ServerTextCommands(this.writeToChat), new object[] {(System.Text.Encoding.Default.GetString(extra))});
                            break;
                    }

                   // this.Invoke(new UpdateLogCall(this.msgO), new object[] { message });

                //message has successfully been received
                //ASCIIEncoding encoder = new ASCIIEncoding();
                //System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
            }

            Client.Close();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Playing)
            {
                CommandStruct cmd;
                cmd.CommandIndex = (byte)NetworkCommands.Disconnect;
                cmd.ExtraDataSize = 0;
                SendCommandToClient(cmd, null);
                
                //Server.Stop();
                //clientThread.Abort();
                //ServerThread.Abort();
            }
            Playing = false;
        }

        private void _chat_send_Click(object sender, EventArgs e) {
            this._chat.AppendText("Player1:" + _chat_text.Text+"\n");
            byte[] text = System.Text.Encoding.Default.GetBytes(_chat_text.Text);
            CommandStruct cmd;
            cmd.CommandIndex = (byte)NetworkCommands.ChatText;
            cmd.ExtraDataSize = (ushort)text.Length;
            SendCommandToClient(cmd, text);
            _chat_text.Text = null;
        }

        private void _chat_text_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Enter) {
                _chat_send_Click(sender, null);
            }
        }
    }
}
