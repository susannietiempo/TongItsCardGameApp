using System.Collections.Generic;
using System.Windows.Forms;

namespace BOLayer
{

    public class Player
    {
        #region
        /// <summary>
        /// Name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The player number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// The hand of the player.
        /// </summary>
        public Hand PlayHand { get; set; }

        /// <summary>
        /// Image of path of the player's avatar.
        /// </summary>
        public string AvatarImg { get; set; }

        /// <summary>
        ///List of the players combo card panels.This is where combo cards dropped 
        ///by the player are shown. 
        /// </summary>
        public List<Panel> ComboCardPanels { get; set; }

        /// <summary>
        /// This is panel where the cards in the player's hand is shown.
        /// </summary>
        public Panel CardsPanel { get; set; }

        /// <summary>
        /// This is the picture box that will show the player's avatar.
        /// </summary>
        public PictureBox PictureBox { get; set; }

        /// <summary>
        /// This will determine if the player is human or computer.
        /// </summary>
        public PlayerType PlayerType { get; set; }

        /// <summary>
        /// This is the alias of the player that they will set in the start of the game.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// This is the current score of the player. This will only be set when the update score method is called.
        /// </summary>
        public int Score { get; private set; }

        #endregion

        #region Constructor
        public Player(string name, string avatar, string alias, Panel cardsPanel)
        {
            Name = name;
            AvatarImg = avatar;
            CardsPanel = cardsPanel;
            Alias = alias;
        }
        public Player(string name, string avatar, Panel cardsPanel)
        {
            Name = name;
            AvatarImg = avatar;
            CardsPanel = cardsPanel;
        }

        public Player()
        {
          

        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the all the action required when it is a computer player's turn. 
        /// </summary>
        /// <param name="aDeck"></param>
        /// <param name="dumpCards"></param>
        /// <param name="dumpPic"></param>
        public void Turn(Deck aDeck, List<Card> dumpCards, Panel panelDump)
        {

            Card drawnCard = aDeck.DrawOneCard();
            PlayHand.AddCard(drawnCard);

            //Gets and display SameKind Combo
            List<Card> comboCards = PlayHand.GetSameKindCards();
            DisplayInPanelComboCards(comboCards);

            //Gets and display Straight Combo
            comboCards = PlayHand.GetStraightCards();
            DisplayInPanelComboCards(comboCards);
        }

        /// <summary>
        /// Looks for an empty panel and displays the combo cards in that panel.
        /// </summary>
        /// <param name="comboCards"></param>
        public void DisplayInPanelComboCards(List<Card> comboCards)
        {
            foreach (Panel p in ComboCardPanels)
            {
                if (p.Controls.Count == 0)
                {
                    Hand.ShowComboCards(p, comboCards);
                    break;
                }
            }
        }
        public int ScoreUpdate()
        {
            return Score = CardsEvaluator.TotalHandPoints(PlayHand);
        }
        #endregion
    }


}
