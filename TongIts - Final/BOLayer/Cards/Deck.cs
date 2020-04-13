using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLayer
{
    public class Deck
    {
        private List<Card> deck = new List<Card>();

        public int Count
        {
            get
            {
                return deck.Count();
            }
        }


        public Deck()
        {
            MakeDeck();
        }

        private void MakeDeck()
        {
            // there are 4 suits
            foreach (int s in Enum.GetValues(typeof(Suit)))
            {
                // there are 13 cards per suit
                foreach (int v in Enum.GetValues(typeof(FaceValue)))
                {
                    // create a card for the current suit and value
                    Card newCard = new Card((Suit)s, (FaceValue)v);

                    // add the card to the deck
                    deck.Add(newCard);
                }
            }
        }

        public void Shuffle()
        {
            List<Card> newDeck = new List<Card>();
            Random rGen = new Random();

            while (deck.Count > 0)
            {
                int removeIndex = rGen.Next(0, deck.Count);
                Card cardToRemove = deck[removeIndex];
                deck.RemoveAt(removeIndex);
                newDeck.Add(cardToRemove);
            }

            // replace the old deck with the next deck
            deck = newDeck;
        }

        public Hand DealHand(int number)
        {
            if (deck.Count == 0)
            {
                throw new ConstraintException("There are no cards left in the deck.  Redeal.");
            }

            // create a hand
            Hand hand = new Hand();

            // determine how many cards to deal
            int numCardsToDeal = number;
            // if there are not enough cards left in the deck to deal the number of 
            // cards passed into the method, then just deal what is left in the deck
            if (deck.Count < numCardsToDeal)
            {
                numCardsToDeal = deck.Count;
            }

            for (int i = 0; i < numCardsToDeal; i++)
            {
                hand.AddCard(DrawOneCard());
            }

            return hand;
        }

        public Card DrawOneCard()
        {
            Card topCard;
            if (deck.Count > 0)
            {
                topCard = deck[0];
                deck.RemoveAt(0);

                return topCard;
            }
            else
            {
                throw new ArgumentException("There are no cards in the deck - deal again.");
            }
        }
    }

}
