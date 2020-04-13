using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BOLayer
{
    public class Hand
    {
        #region Field
        private List<Card> cards = new List<Card>();
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return cards.Count();
            }
        }

        public Card this[int index]
        {
            get
            {
                return cards[index];
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// The method add a card to the hand
        /// </summary>
        /// <param name="newCard"></param>
        public void AddCard(Card newCard)
        {
            // the List<T>.Contains method cannot be used since it only checks if the same reference object exists
            if (ContainsCard(newCard))
            {
                throw new ConstraintException(newCard.FaceValue.ToString() + " of " +
                    newCard.Suit.ToString() + " already exists in the Hands");
            }

            cards.Add(newCard);
        }

        /// <summary>
        /// The method removes card from a hand.
        /// </summary>
        /// <param name="newCard"></param>
        public void RemoveCard(Card newCard)
        {
            // the List<T>.Contains method cannot be used since it only checks if the same reference object exists
            if (!ContainsCard(newCard))
            {
                throw new ConstraintException(newCard.FaceValue.ToString() + " of " +
                    newCard.Suit.ToString() + " does not exists in the Hands");
            }

            cards.Remove(newCard);
        }

        /// <summary>
        /// The method sort the card in the hand by suit and in ascending order per suit type.
        /// </summary>
        public void Sort()
        {
            int counter = 0;
            for (int i = 0; i <= 3; i++)
            {
                List<Card> suit = cards.Where(card => card.Suit == (Suit)i).ToList();
                CardsEvaluator.ArrageCardsFacevalue(suit);

                foreach (Card card in suit)
                {
                    cards.Remove(card);
                }

                for (int c = 0; c < suit.Count; c++)
                {
                    cards.Insert(counter, suit[c]);
                    counter += 1;
                }
            }

        }
        /// <summary>
        /// The method checks if the a card is in a hand.
        /// </summary>
        /// <param name="cardToCheck"></param>
        /// <returns></returns>
        private bool ContainsCard(Card cardToCheck)
        {
            return cards.Where(card => card.FaceValue == cardToCheck.FaceValue && card.Suit == cardToCheck.Suit).Count() != 0;

        }

        #region Hand Evaluator methods 
        //These set of methods are mosthly for the hand evaluation of computer players. 
        //Cards evaluation of human player is handled by card is handled by CardEvaluator class.

        /// <summary>
        /// The method arranges the cards on a hand in ascending order based on facevalue.
        /// </summary>
        public void ArrageCardsFacevalue()
        {
            cards.Sort((c1, c2) => c1.FaceValue.CompareTo(c2.FaceValue));
        }

        /// <summary>
        /// The method a list of cards which has the same facevalue
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private List<int> GroupCardsFV(List<Card> cards)
        {
            List<int> comboFV = new List<int>();

            for (int i = 0; i < 12; i++)
            {
                if (cards.Where(c => c.FaceValue == (FaceValue)i).ToList().Count >= 3)
                {
                    comboFV.Add(i);
                }
            }

            return comboFV;
        }

        /// <summary>
        /// The method returns cards with the same suit.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public List<int> GroupCardsSuit(List<Card> cards)
        {
            List<int> comboSuit = new List<int>();

            for (int i = 0; i <= 3; i++)
            {
                if (cards.Where(c => c.Suit == (Suit)i).ToList().Count >= 3)
                {
                    comboSuit.Add(i);
                }
            }

            return comboSuit;
        }

        /// <summary>
        /// The method returns the list of card in the threeofakind or fourakind combo.
        /// </summary>
        /// <returns></returns>
        public List<Card> GetSameKindCards()
        {
            List<int> comboList = GroupCardsFV(cards);
            List<Card> comboCards = new List<Card>();

            foreach (int i in comboList)
            {
                List<Card> cardsList = cards.Where(c => c.FaceValue == (FaceValue)i).ToList();

                foreach (Card card in cardsList)
                {
                    comboCards.Add(card);
                    cards.Remove(card);
                }

            }

            return comboCards;
        }
        /// <summary>
        /// The method returns the list of card in the straight combo.
        /// </summary>
        /// <returns></returns>
        public List<Card> GetStraightCards()
        {
            List<int> comboList = GroupCardsSuit(cards);
            List<Card> comboCards = new List<Card>();

            foreach (int i in comboList)
            {
                List<Card> cardsList = cards.Where(c => c.Suit == (Suit)i).ToList();

                CardsEvaluator.ArrageCardsFacevalue(cardsList);
                if (CardsEvaluator.Straight(cardsList))
                {
                    foreach (Card card in cardsList)
                    {
                        comboCards.Add(card);
                        cards.Remove(card);
                    }
                }
            }

            return comboCards;
        }



        #endregion

        /// <summary>
        /// Method selects what card to dump. This is for the computer player mostly or if the human
        /// player went "time'up".
        /// </summary>
        /// <param name="deck"></param>
        /// <returns></returns>
        public Card CardToDump(Deck deck)
        {
            Card cardToDiscard;

            List<Card> cardsToKeep = new List<Card>();
            List<Card> cardsToDiscard = new List<Card>();

            //Checks for 2 of a kind card,puts the card in the card to keep list
            for (int i = 0; i < 12; i++)
            {
                if (cards.Where(c => c.FaceValue == (FaceValue)i).ToList().Count == 2)
                {
                    cards.Where(c => c.FaceValue == (FaceValue)i).ToList().ForEach(c => cardsToKeep.Add(c));
                }
            }

            //Checks for cards that can possible form straight combo, puts the card in the card to keep list

            for (int i = 0; i < 4; i++)
            {
                List<Card> cardsStraight = new List<Card>();

                if (cards.Where(c => c.Suit == (Suit)i).ToList().Count >= 2)
                {
                    cards.Where(c => c.Suit == (Suit)i).ToList().ForEach(c => cardsStraight.Add(c));
                }

                CardsEvaluator.ArrageCardsFacevalue(cardsStraight);
                if (cardsStraight.Count > 0)
                {
                    for (int d = 0; d < cardsStraight.Count; d++)
                    {
                        if (d == 0)
                        {
                            if (cardsStraight[d].FaceValue + 2 == cardsStraight[d + 1].FaceValue)
                            {
                                cardsToKeep.Add(cardsStraight[d]);
                            }
                        }
                        else if (d == cardsStraight.Count - 1)
                        {
                            if (cardsStraight[d].FaceValue - 2 == cardsStraight[d - 1].FaceValue)
                            {
                                cardsToKeep.Add(cardsStraight[d]);
                            }
                        }
                        else
                              if (cardsStraight[d].FaceValue + 2 == cardsStraight[d + 1].FaceValue || cardsStraight[d].FaceValue - 2 == cardsStraight[d - 1].FaceValue)
                        {
                            cardsToKeep.Add(cardsStraight[d]);
                        }
                    }
                }

            }

            //puts cards in the hand that are not in the cards to keep list to the discard list
            cards.Where(c => !cardsToKeep.Contains(c)).ToList().ForEach(c => cardsToDiscard.Add(c));

            //arrange card by facevalue points, if the deck has 6 cards remaining or there is no card in the discard list , it will return the highest valued card,
            //otherwise,it will return the highest valued card in the discard list 

            CardsEvaluator.ArrageCardsFacevalue(cardsToDiscard);

            if (cardsToDiscard.Count == 0 || deck.Count <= 6)
            {
                CardsEvaluator.ArrageCardsFacevalue(cards);
                return cardToDiscard = cards.Last();
            }

            return cardToDiscard = cardsToDiscard.Last();
        }


        /// <summary>
        /// The method shows the back of the cards in a hand on the player's hand panel.
        /// </summary>
        /// <param name="thePanel"></param>
        /// <param name="theHand"></param>
        public static void ShowHandBack(Panel thePanel, Hand theHand)
        {
            thePanel.Controls.Clear();
            Card aCard;
            PictureBox aPic;
            for (int i = 0; i < theHand.Count; i++)
            {
                aCard = theHand[i];
                string imgPath = $@"images\cardback.png";
                // string imgPath = $@"images\{aCard.FaceValue.ToString()}{aCard.Suit.ToString()}.jpg";

                aPic = new PictureBox()
                {
                    Image = Image.FromFile(imgPath),
                    Text = aCard.FaceValue.ToString(),
                    Width = 71,
                    Height = 100,
                    Left = 30 * i

                };

                thePanel.Controls.Add(aPic);
            }
        }

        /// <summary>
        /// The method shows the combo cards in the avaible panel.
        /// </summary>
        /// <param name="thePanel"></param>
        /// <param name="cards"></param>
        public static void ShowComboCards(Panel thePanel, List<Card> cards)
        {

            Card aCard;
            PictureBox aPic;

            for (int i = 0; i < cards.Count; i++)
            {
                aCard = cards[i];
                string imgPath = $@"images\{aCard.FaceValue.ToString()}{aCard.Suit.ToString()}.jpg";

                aPic = new PictureBox()
                {
                    Image = Image.FromFile(imgPath),
                    Text = aCard.FaceValue.ToString(),
                    Width = 71,
                    Height = 100,
                    Left = 15 * i,
                };

                thePanel.Controls.Add(aPic);
            }
        }

        #endregion

    }
}
