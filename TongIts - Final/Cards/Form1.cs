using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BOLayer;

namespace Cards
{
    public partial class Form1 : Form
    {
        #region Fields
        private Deck aDeck;
        private int timeLeft = 25;
        private Random r = new Random();
        private List<Card> groupCards = new List<Card>();
        private List<Card> dumpCards = new List<Card>();
        private int player1Wins = 0;
        private SortedDictionary<int, Hero> heroes = new SortedDictionary<int, Hero>();

        private Player player1 = new Player();
        private Player player2 = new Player();
        private Player player3 = new Player();
        private Game game = new Game();
        #endregion

        #region Main Methods
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When form loads, the heroes are created and sorted dictionary list for heroes is made. Heroes are added to the sorted dictionary.
        /// </summary>
        private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {
                SetUp();

                //Create heroes
                Hero captainBarbel = new Hero
                {
                    Name = "Captain Barbel",
                    AvatarImg = $@"images\captainbarbel.png"
                };

                Hero darna = new Hero
                {
                    Name = "Darna",
                    AvatarImg = $@"images\darna.png"
                };

                Hero lastikMan = new Hero
                {
                    Name = "Lastik Man",
                    AvatarImg = $@"images\lastikman.png"
                };

                Hero panday = new Hero
                {
                    Name = "Ang Panday",
                    AvatarImg = $@"images\panday.png"
                };

                heroes.Add(1, captainBarbel);
                heroes.Add(2, darna);
                heroes.Add(3, lastikMan);
                heroes.Add(4, panday);

                //Add heroes to selection
                cmbHeroes.Items.Add(captainBarbel.Name);
                cmbHeroes.Items.Add(darna.Name);
                cmbHeroes.Items.Add(lastikMan.Name);
                cmbHeroes.Items.Add(panday.Name);



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }


        private void btnNewGame_Click(object sender, EventArgs e)
        {
            try
            {

                if (game.GameNumber == 1)
                {

                    if (cmbHeroes.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please select your hero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (String.IsNullOrEmpty(txtPlayerAlias.Text))
                    {
                        MessageBox.Show("Please put in an alias", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Hero hero = new Hero();
                    hero = heroes[cmbHeroes.SelectedIndex + 1];

                    //Set up player 1
                    player1.Name = hero.Name;
                    player1.AvatarImg = hero.AvatarImg;
                    player1.Number = 1;
                    player1.PictureBox = picP1;
                    player1.CardsPanel = PanelHandP1;
                    player1.ComboCardPanels = new List<Panel>() {
                        PanelCmbP1A,
                        PanelCmbP1B,
                        PanelCmbP1C,
                        PanelCmbP1D
                    };
                    player1.PlayerType = PlayerType.HumanPlayer;
                    picP1.Image = Image.FromFile(player1.AvatarImg);
                    lbl1.Text = player1.Name;
                    lblAlias.Text = $"Alyas \"{txtPlayerAlias.Text}\"";

                    //Set up player 2

                    hero = heroes[r.Next(1, 4)];
                    while (hero.Name == player1.Name)
                    {
                        hero = heroes[r.Next(1, 4)];
                    }

                    player2.Name = hero.Name;
                    player2.AvatarImg = hero.AvatarImg;
                    player2.Number = 1;
                    player2.PictureBox = picP1;
                    player2.CardsPanel = PanelHandP2;
                    player2.ComboCardPanels = new List<Panel>() {
                        PanelCmbP2A,
                        PanelCmbP2B,
                        PanelCmbP2C,
                        PanelCmbP2D
                    };
                    player2.PlayerType = PlayerType.ComputerPlayer;
                    picP2.Image = Image.FromFile(player2.AvatarImg);
                    lblP2.Text = player2.Name;

                    //Set up player 3

                    hero = heroes[r.Next(1, 4)];
                    while (hero.Name == player1.Name || hero.Name == player2.Name)
                    {
                        hero = heroes[r.Next(1, 4)];
                    }

                    player3.Name = hero.Name;
                    player3.AvatarImg = hero.AvatarImg;
                    player3.Number = 1;
                    player3.PictureBox = picP1;
                    player3.CardsPanel = PanelHandP3;
                    player3.ComboCardPanels = new List<Panel>() {
                        PanelCmbP3A,
                        PanelCmbP3B,
                        PanelCmbP3C,
                        PanelCmbP3D
                    };
                    player3.PlayerType = PlayerType.ComputerPlayer;
                    picP3.Image = Image.FromFile(player3.AvatarImg);
                    lblP3.Text = player3.Name;
                    List<Player> players = new List<Player>() {
                        player1,
                        player2,
                        player3
                    };
                    game.Players = players;
                }

                SetUp();

                //Setup timer info and game status info
                timeLeft = 25;
                timerP1.Start();
                game.IsActive = true;

                //Set up stat label
                lblGameNum.Text = game.GameNumber.ToString();
                lblP1Wins.Text = player1Wins.ToString();

                //Clear  combo card panels and dump card panel
                player1.ComboCardPanels.ForEach(c => c.Controls.Clear());
                player2.ComboCardPanels.ForEach(c => c.Controls.Clear());
                player3.ComboCardPanels.ForEach(c => c.Controls.Clear());
                PanelDump.Controls.Clear();

                //Set list of cards to default

                groupCards = new List<Card>();
                dumpCards = new List<Card>();

                //Make controls visible
                foreach (Control c in this.Controls)
                {
                    c.Visible = true;
                }

                PanelSelection.Visible = false;

                //Deal card to players
                player1.PlayHand = aDeck.DealHand(13);
                player2.PlayHand = aDeck.DealHand(13);
                player3.PlayHand = aDeck.DealHand(13);

                //Sort player's card to arrange cards with same suit and ascending facevalue
                player1.PlayHand.Sort();
                player2.PlayHand.Sort();
                player3.PlayHand.Sort();

                //Show cards of players in the panel
                ShowHandFront(player1.CardsPanel, player1.PlayHand);
                Hand.ShowHandBack(player2.CardsPanel, player2.PlayHand);
                Hand.ShowHandBack(player3.CardsPanel, player3.PlayHand);

                // Show player 1 score
                UpdatePlayer1Score();

                //Set up stock area
                lblStockCount.Text = $"({aDeck.Count.ToString()})";
                PictureBox stockPic = new PictureBox()
                {
                    Image = Image.FromFile($@"images\cardback.png"),
                    Width = 71,
                    Height = 100,
                    Cursor = System.Windows.Forms.Cursors.Hand
                };
                stockPic.Click += Stock_Click;
                panelStock.Controls.Add(stockPic);

                // Prompt Player 1 turn
                picP1.BackColor = Color.FromArgb(255, 255, 128);
                lblTurnName.Text = $"{player1.Name}'s";
                timerP1.Enabled = true;
                game.Turn = player1;
                //timerP1.Start();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void CardPictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (game.Turn != player1)
                {
                    MessageBox.Show("It is not your turn!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PictureBox picClicked = (PictureBox)sender;
                int cardPos = PanelHandP1.Controls.IndexOf(picClicked);

                // Changes the position of the clicked card to top and puts it back on the second click
                if (picClicked.Top == 15)
                {
                    picClicked.Top = 0;
                    groupCards.Add(player1.PlayHand[cardPos]);
                }
                else
                {
                    picClicked.Top = 15;
                    groupCards.Remove(player1.PlayHand[cardPos]);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void Stock_Click(object sender, EventArgs e)
        {
            try
            {

                if (game.Turn != player1)
                {
                    MessageBox.Show("Please wait for your turn before drawing a card.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Card aCard;
                aCard = aDeck.DrawOneCard();
                UpdateStockCount();

                player1.PlayHand.AddCard(aCard);
                player1.PlayHand.Sort();
                ShowHandFront(player1.CardsPanel, player1.PlayHand);

                groupCards = new List<Card>();

                //Enables drop and dump
                //Player 1 can't drop or dump until card is drawn from stock
                PanelHandP1.Enabled = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method fired when the dump cards is click
        private void Dump_Click(object sender, EventArgs e)
        {
            try
            {
                if (game.Turn != player1)
                {
                    MessageBox.Show("Please wait for your turn before drawing a card.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Card dumpCard = dumpCards[0];

                player1.PlayHand.AddCard(dumpCard);
                player1.PlayHand.Sort();

                ShowHandFront(player1.CardsPanel, player1.PlayHand);
                UpdatePlayer1Score();
                PanelDump.Controls.Clear();

                groupCards = new List<Card>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oh No!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDump_Click(object sender, EventArgs e)
        {
            try
            {
                if (PanelHandP1.Enabled == false)
                {
                    MessageBox.Show("Please draw a card from stock before dumping a card!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (game.Turn != player1)
                {
                    MessageBox.Show("Please wait for your turn before dumping a card.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (groupCards.Count != 1)
                {
                    MessageBox.Show("You can only drop ONE card.");
                }
                else
                {
                    Card card = groupCards[0];

                    player1.PlayHand.RemoveCard(card);
                    player1.PlayHand.Sort();

                    ShowHandFront(player1.CardsPanel, player1.PlayHand);
                    UpdatePlayer1Score();

                    dumpCards.Insert(0, card);
                    groupCards = new List<Card>();

                    //Set dump pic
                    PanelDump.Controls.Clear();
                    string imgPath = $@"images\{card.FaceValue.ToString()}{card.Suit.ToString()}.jpg";
                    PictureBox dumpPic = new PictureBox()
                    {
                        Image = Image.FromFile(imgPath),
                        Width = 71,
                        Height = 96,
                        Cursor = System.Windows.Forms.Cursors.Hand
                    };

                    dumpPic.Click += Dump_Click;
                    PanelDump.Controls.Add(dumpPic);

                    //Check Game Status
                    CheckGameStatus();

                    //Calls for player2 method to start turn
                    Player2_Turn();
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oh oh!!", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

        }

        private void btnDrop_Click(object sender, EventArgs e)
        {
            try
            {
                if (PanelHandP1.Enabled == false)
                {
                    MessageBox.Show("Please draw a card from stock before dropping!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (game.Turn != player1)
                {
                    MessageBox.Show("Please wait for your turn before dropping cards.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (CardsEvaluator.EvaluateCard(groupCards))
                {
                    player1.DisplayInPanelComboCards(groupCards);

                    for (int i = 0; i < groupCards.Count; i++)
                    {
                        player1.PlayHand.RemoveCard(groupCards[i]);
                    }

                    player1.PlayHand.Sort();
                    ShowHandFront(player1.CardsPanel, player1.PlayHand);
                    UpdatePlayer1Score();

                    groupCards = new List<Card>();
                }

                else
                {
                    MessageBox.Show("This is an invalid card combination.", "Sorry!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void cmbHeroes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Hero hero1 = heroes[cmbHeroes.SelectedIndex + 1];
                picAvatar.Image = Image.FromFile(hero1.AvatarImg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }

        private void timerP1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (timeLeft > 0)
                {
                    timeLeft -= 1;
                    lblTimeP1.Text = timeLeft.ToString();
                }

                if (timeLeft == 0)
                {
                    timerP1.Stop();
                    MessageBox.Show("Sorry, Times up!", "Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //Auto selects card for player to dump
                    //Dump card with highest value
                    //Player1 loses ability to chose what card to dump 
                    DumpCard(player1);
                    ShowHandFront(player1.CardsPanel, player1.PlayHand);

                    //Calls player 2's turn
                    Player2_Turn();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you really want to exit the game?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                   System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }
        #endregion

        #region Turn Methods for Computer Players
        /// <summary>
        /// Method  that handles all activities for player 2.
        /// </summary>
        private void Player2_Turn()
        {
            try
            {
                if (game.IsActive)
                {
                    //Reset timer info
                    timeLeft = 25;
                    timerP1.Start();

                    game.Turn = player2;
                    picP1.BackColor = Color.FromArgb(0, 128, 55);
                    picP2.BackColor = Color.FromArgb(255, 255, 128);
                    lblTurnName.Text = $"{player2.Name}'s";

                    //Pauses the turn for the player between 1-5 seconds,otherwise it will turns will almost
                    //become instantenous
                    PauseForMilliSeconds(r.Next(1000, 5000));
                    player2.Turn(aDeck, dumpCards, PanelDump);

                    UpdateStockCount();

                    //Pauses the turn for the player between 5-15 seconds,otherwise it will turns will almost
                    //become instantenous
                    PauseForMilliSeconds(r.Next(5000, 7000));
                    // Handles dump card for player 2
                    DumpCard(player2);

                    player2.PlayHand.Sort();
                    Hand.ShowHandBack(player2.CardsPanel, player2.PlayHand);

                    //Check game status
                    CheckGameStatus();

                    //Calls for Player 3's turn
                    Player3_Turn();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
            }

        }

        /// <summary>
        /// Method  that handles all activities for player 3 and resets configuration for player 1.
        /// </summary>
        private void Player3_Turn()
        {
            try
            {
                if (game.IsActive)
                {

                    timeLeft = 25;

                    game.Turn = player3;
                    picP2.BackColor = Color.FromArgb(0, 128, 55);
                    picP3.BackColor = Color.FromArgb(255, 255, 128);
                    lblTurnName.Text = $"{player3.Name}'s";

                    //Pauses the turn for the player between 1-5 seconds,otherwise it will turns will almost
                    //become instantenous
                    PauseForMilliSeconds(r.Next(1000, 5000));
                    player3.Turn(aDeck, dumpCards, PanelDump);
                    UpdateStockCount();

                    // Handles dump card for player 3
                    PauseForMilliSeconds(r.Next(5000, 7000));
                    DumpCard(player3);

                    player3.PlayHand.Sort();
                    Hand.ShowHandBack(player3.CardsPanel, player3.PlayHand);
                    player3.ScoreUpdate();

                    //Checkgame status
                    CheckGameStatus();

                    //reset to P1's Turn
                    picP3.BackColor = Color.FromArgb(0, 128, 55);
                    picP1.BackColor = Color.FromArgb(255, 255, 128);
                    PanelHandP1.Enabled = false;
                    lblTurnName.Text = $"{player1.Name}'s";
                    timeLeft = 25;
                    game.Turn = player1;

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

        }

        #endregion

        #region Helper Methods
        private void SetUp()
        {
            try
            {
                aDeck = new Deck();
                aDeck.Shuffle();
                groupCards = new List<Card>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public void ShowHandFront(Panel thePanel, Hand theHand)
        {
            try
            {
                thePanel.Controls.Clear();
                Card aCard;
                PictureBox aPic;
                for (int i = 0; i < theHand.Count; i++)
                {
                    aCard = theHand[i];
                    string imgPath = $@"images\{aCard.FaceValue.ToString()}{aCard.Suit.ToString()}.jpg";

                    aPic = new PictureBox()
                    {
                        Image = Image.FromFile(imgPath),
                        Text = aCard.FaceValue.ToString(),
                        Width = 71,
                        Height = 100,
                        Left = 71 * i,
                        Top = 15,
                        Tag = aCard,
                        Cursor = System.Windows.Forms.Cursors.Hand
                    };

                    aPic.Click += CardPictureBox_Click;

                    thePanel.Controls.Add(aPic);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

        }

        /// <summary>
        /// The method add the dump card to the dumpcard list and sets the image of the dumbPic with the dump card and add it to the PanelDump. 
        /// </summary>
        /// <param name="player"></param>
        private void DumpCard(Player player)
        {
            try
            {
                //Get the card to dump using CardToDump Method
                Card cardToDump = player.PlayHand.CardToDump(aDeck);

                player.PlayHand.RemoveCard(cardToDump);
                dumpCards.Insert(0, cardToDump);

                //Set the dumpPic with the image of the dumpcard and the event handler for dumpPic click event
                PanelDump.Controls.Clear();
                string imgPath = $@"images\{cardToDump.FaceValue.ToString()}{cardToDump.Suit.ToString()}.jpg";
                PictureBox dumpPic = new PictureBox()
                {
                    Image = Image.FromFile(imgPath),
                    Width = 71,
                    Height = 100,

                };
                dumpPic.Click += Dump_Click;
                PanelDump.Controls.Add(dumpPic);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

        }

        private void UpdateStockCount()
        {
            try
            {
                lblStockCount.Text = $"({aDeck.Count.ToString()})";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }
        private void UpdatePlayer1Score()
        {
            try
            {
                player1.ScoreUpdate();
                lblP1Score.Text = player1.Score.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }


        //Code copied from StackOverflow
        //Source:https://stackoverflow.com/questions/14625427/c-sharp-time-delay-on-application
        public static DateTime PauseForMilliSeconds(int MilliSecondsToPauseFor)
        {
            System.DateTime ThisMoment = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(0, 0, 0, 0, MilliSecondsToPauseFor);
            System.DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = System.DateTime.Now;
            }

            return System.DateTime.Now;
        }

        /// <summary>
        /// This method checks the game status. If it is end of game, the selection panel is shown with the players' scores and winners. Human player will have option to play another game.
        /// </summary>

        private void CheckGameStatus()
        {

            try
            {
                if (aDeck.Count == 0 || player1.PlayHand.Count == 0 || player2.PlayHand.Count == 0 || player2.PlayHand.Count == 0)
                {
                    timerP1.Stop();

                    //Make all controls invisible except selection panel and score elements
                    foreach (Control c in this.Controls)
                    {
                        c.Visible = false;
                    }

                    PanelSelection.Visible = true;
                    lblPutInYourAlias.Visible = false;
                    lblSelectHero.Visible = false;
                    lblStatNewGame.Visible = false;
                    cmbHeroes.Visible = false;
                    txtPlayerAlias.Visible = false;
                    picAvatar.Visible = false;
                    PanelEndGame.Visible = true;
                    picExitStart.Visible = true;
                    lblStatNewGame.Visible= true;
                    lblStatNewGame.Text = "Play Again";

                    game.GameEnd();

                    lblPlayerScoreFinal.Text = game.DisplayScore();
                    game.GameNumber += 1;

                    //update player one win score
                    if (game.Winner.Contains(player1))
                    {
                        player1Wins += 1;
                        lblWinner.Text = $"AWESOME! YOU WIN!";
                    }
                    else
                    {
                        if (game.Winner.Count == 1)
                        {
                            lblWinner.Text = $"SORRY, {game.Winner[0].Name} WINS!";
                        }
                        else
                        {
                            lblWinner.Text = $"SORRY, Better luck next time!";
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }
        #endregion
    }
}