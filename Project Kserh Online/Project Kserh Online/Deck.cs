using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Kserh_Online {
   public class Deck {

        public CardStr[] Cards;
        public ushort _index;
        public ushort Index{get {return _index;}}


        public Deck() {
            _index = 0;
            Cards = new CardStr[52];
        }

        public void Setup() {
            _index = 0;
            ushort i = 0;

            foreach (Suit S in Enum.GetValues(typeof(Suit))) {
                foreach (Ranks R in Enum.GetValues(typeof(Ranks))) {
                    Cards[i].rank = R;
                    Cards[i].suit = S;
                    i++;
                }
            }
        }

        public void Shuffle() {
            Random Rnd = new Random();
            ushort Steps = (ushort)Rnd.Next(50, 120);
            for (ushort step = 0; step < Steps; step++) {
                ushort indexA = (ushort)Rnd.Next(0, 52);
                ushort indexB;
                do {
                    indexB = (ushort)Rnd.Next(0, 52);
                } while (indexA == indexB);
                CardStr temp = Cards[indexA];
                Cards[indexA] = Cards[indexB];
                Cards[indexB] = temp;
            }
        }
        public CardStr Pop() {
            _index++;
            return Cards[Index - 1];
        }
    }
}
