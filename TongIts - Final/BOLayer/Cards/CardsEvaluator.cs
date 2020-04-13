using System;
using System.Collections.Generic;
using System.Linq;

namespace BOLayer
{
    /// <summary>
    /// This class evaluates the list of card combination to see it has a valid combination. This is a 
    /// static class and is mostly used for the human player. Hand evaluation of computer players will
    /// be done using the Hand Class.
    /// </summary>
    public static class CardsEvaluator
    {
    
        #region Methods

        /// <summary>
        /// Method evaluates if a list of card is a valid combination. 
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool EvaluateCard(List<Card> cards)
        {
            if (cards.Count < 3)
            {
                throw new ArgumentException("This is an invalid card combination.");
            }

            if (ThreeOrFourfAKind(cards) || Straight(cards))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculated the total points of a player's hand. The player with lowest number of points at the end of the game winds.
        /// </summary>
        /// <param name="playerHand"></param>
        /// <returns></returns>
        public static int TotalHandPoints(Hand playerHand)
        {
            int points = 0;
            for (int i = 0; i < playerHand.Count; i++)
            {
                switch (playerHand[i].FaceValue)
                {
                    case (FaceValue.King):
                        {
                            points += 10;
                            break;
                        }

                    case (FaceValue.Queen):
                        {
                            points += 10;
                            break;
                        }
                    case (FaceValue.Jack):
                        {
                            points += 10;
                            break;
                        }
                    case (FaceValue.Ten):
                        {
                            points += 10;
                            break;
                        }
                    case (FaceValue.Nine):
                        {
                            points += 9;
                            break;
                        }
                    case (FaceValue.Eight):
                        {
                            points += 8;
                            break;
                        }
                    case (FaceValue.Seven):
                        {
                            points += 10;
                            break;
                        }
                    case (FaceValue.Six):
                        {
                            points += 6;
                            break;
                        }
                    case (FaceValue.Five):
                        {
                            points += 5;
                            break;
                        }
                    case (FaceValue.Four):
                        {
                            points += 4;
                            break;
                        }
                    case (FaceValue.Three):
                        {
                            points += 3;
                            break;
                        }
                    case (FaceValue.Two):
                        {
                            points += 2;
                            break;
                        }
                    case (FaceValue.Ace):
                        {
                            points += 1;
                            break;
                        }

                    default:
                        break;
                }

            }

            return points;
        }

        /// <summary>
        /// The method sorts a list of card by facevalue in ascending order.
        /// </summary>
        /// <param name="cards"></param>
        internal static void ArrageCardsFacevalue(List<Card> cards)
        {
            cards.Sort((c1,c2) => c1.FaceValue.CompareTo(c2.FaceValue));
            
        }

        /// <summary>
        /// Evaluate if a list of card is a three or four of a kind combination. 
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static bool ThreeOrFourfAKind(List<Card> cards)
        {
            return (!(cards.Any(c => c.FaceValue != cards[0].FaceValue)));
            //return (cards[0].FaceValue == cards[1].FaceValue && cards[1].FaceValue == cards[2].FaceValue);
        }

        /// <summary>
        /// Evaulate if a list of card is a straight. 
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        internal static bool Straight(List<Card> cards)
        {
            if (cards.Any(c => c.Suit != cards[0].Suit))
            {
                return false;
            }

            ArrageCardsFacevalue(cards);

            FaceValue prev = cards[0].FaceValue;

            for (int i = 1; i < cards.Count; i++)
            {
            
                if ((prev + 1) != cards[i].FaceValue)
                {
                    return false;
                }

                prev = cards[i].FaceValue;
            }

            return true;
        }

        #endregion

    }
}
