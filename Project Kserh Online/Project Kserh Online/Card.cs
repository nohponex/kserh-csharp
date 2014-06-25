using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Project_Kserh_Online {


    public class Card : System.Windows.Forms.Control {
        public static readonly string[] SuitChars = { "♣", "♦", "♥", "♠" };
        public static readonly string[] RankChars = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        public static readonly float SizeRatio = 0.715909F;

        private short SuitIndex = 0, RankIndex = 0;
        public Players player = Players.None;
        private bool _hidden = false;

        private CardStr _card;

        private bool mouseOver = false;

        public ushort Index;
        
        public Suit Suit { get { return _card.suit; } set { _card.suit = value; SetSuitIndex(); this.Invalidate(); } }
        public Ranks Rank { get { return _card.rank; } set { _card.rank = value; SetRankIndex(); this.Invalidate(); } }

        public CardStr CardStuct { get { return _card; } }
        public bool Hidden { get { return _hidden; } set { _hidden = value; this.Invalidate(); } }

        public Card() {
            this.Size = new System.Drawing.Size(120, 167);
        }

        public Card(CardStr CardDetails) {
            this.Size = new System.Drawing.Size(120, 167);
            _card = CardDetails;
            SetSuitIndex(); SetSuitIndex();
            SetRankIndex();
            this.Invalidate();
        }

        private void SetSuitIndex() {
            switch (_card.suit) {
                case Suit.Clubs:
                    SuitIndex = 0;
                    break;
                case Suit.Diamonds:
                    SuitIndex = 1;
                    break;
                case Suit.Hearts:
                    SuitIndex = 2;
                    break;
                case Suit.Spades:
                    SuitIndex = 3;
                    break;
            }
        }

        private void SetRankIndex() {
            switch (_card.rank) {
                case Ranks.Ace:
                    RankIndex = 0;
                    break;
                case Ranks.two:
                    RankIndex = 1;
                    break;
                case Ranks.three:
                    RankIndex = 2;
                    break;
                case Ranks.four:
                    RankIndex = 3;
                    break;
                case Ranks.five:
                    RankIndex = 4;
                    break;
                case Ranks.six:
                    RankIndex = 5;
                    break;
                case Ranks.seven:
                    RankIndex = 6;
                    break;
                case Ranks.eight:
                    RankIndex = 7;
                    break;
                case Ranks.nine:
                    RankIndex = 8;
                    break;
                case Ranks.ten:
                    RankIndex = 9;
                    break;
                case Ranks.Jack:
                    RankIndex = 10;
                    break;
                case Ranks.Queen:
                    RankIndex = 11;
                    break;
                case Ranks.King:
                    RankIndex = 12;
                    break;
            }
        }


        protected override void OnMouseEnter(EventArgs e) {
            mouseOver = true;
            Invalidate();
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e) {
            mouseOver = false;
            Invalidate();
            base.OnMouseLeave(e);
        }
        protected override void OnResize(EventArgs e) {
            this.Size = new Size(this.Width, (int)(Width / SizeRatio));
            base.OnResize(e);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
            System.Drawing.Graphics g = e.Graphics;

            if (!Hidden) {

                if (mouseOver) {
                    //g.DrawImage(global::Kserh_Project.Properties.Resources.CardOver, new Point(0, 0));
                    g.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(0, 0, this.Size.Width, this.Size.Height));
                    g.DrawRectangle(new Pen(Color.White, 10), new Rectangle(0, 0, this.Size.Width, this.Size.Height));
                } else {
                    // g.DrawImage(global::Kserh_Project.Properties.Resources.Card, new Point(0, 0));
                    g.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, this.Size.Width, this.Size.Height));
                    g.DrawRectangle(new Pen(Color.Gray, 3), new Rectangle(0, 0, this.Size.Width - 2, this.Size.Height - 2));
                }
                //g.DrawLine(System.Drawing.Pens.Aqua, new System.Drawing.Point(10, 20), new System.Drawing.Point(20, 40));
                Brush myBrush;
                if (_card.suit == Suit.Diamonds || _card.suit == Suit.Hearts) {
                    myBrush = Brushes.Red;
                } else {
                    myBrush = Brushes.Black;
                }
                g.DrawString(RankChars[RankIndex], new Font(this.Font.Name, 14, FontStyle.Regular), myBrush, new PointF(7, 7));
                g.DrawString(SuitChars[SuitIndex], new Font(this.Font.Name, 18, FontStyle.Bold), myBrush, 5, 25);

                g.DrawString(SuitChars[SuitIndex], new Font(this.Font.Name, 55, FontStyle.Bold), myBrush, this.Width - 70, this.Height - 75);


            } else {
                g.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);
                g.DrawRectangle(new Pen(Color.Gray, 6), new Rectangle(3, 3, this.Size.Width - 6, this.Size.Height - 6));


                g.DrawString(SuitChars[0], new Font(this.Font.Name, 20, FontStyle.Regular), Brushes.WhiteSmoke, new Point((this.Width / 2) - 13, this.Height / 2 - 50));
                g.DrawString(SuitChars[1], new Font(this.Font.Name, 20, FontStyle.Regular), Brushes.WhiteSmoke, new Point((this.Width / 2) - 13, this.Height / 2 - 27));
                g.DrawString(SuitChars[2], new Font(this.Font.Name, 20, FontStyle.Regular), Brushes.WhiteSmoke, new Point((this.Width / 2) - 13, this.Height / 2 - 2));
                g.DrawString(SuitChars[3], new Font(this.Font.Name, 20, FontStyle.Regular), Brushes.WhiteSmoke, new Point((this.Width / 2) - 13, this.Height / 2 + 20));

            }
        }
    }
}
