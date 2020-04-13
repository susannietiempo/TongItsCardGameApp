using System.Collections.Generic;

namespace BOLayer
{
    public class Game
    {
        #region Parameters
        /// <summary>
        ///  The number of games played.
        /// </summary>
        public int GameNumber { get; set; } = 1;
        /// <summary>
        /// If the game is active or end.
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Winner of the current game.
        /// </summary>
        public List<Player> Winner = new List<Player>();
        /// <summary>
        /// The player of the current turn.
        /// </summary>
        public Player Turn { get; set; }
        /// <summary>
        /// List of players in the current game.
        /// </summary>
        public List<Player> Players = new List<Player>();
        #endregion

        #region Methods
        /// <summary>
        /// Evaluates the winner when game ends and sets game to inactive
        /// </summary>
        public void GameEnd()
        {
            //Sets game to inactive
            IsActive = false;

            //Update players' scores
            Players.ForEach(p => p.ScoreUpdate());

            //Determine winner

            // Winner = Players.Where(p => p.Score == Players.Max(c=>c.Score));

            if (Players[0].Score < Players[1].Score && Players[0].Score < Players[2].Score)
            {
                Winner.Add(Players[0]);
            }
            else if (Players[1].Score < Players[0].Score && Players[1].Score < Players[2].Score)
            {
                Winner.Add(Players[1]);
            }
            else if (Players[2].Score < Players[0].Score && Players[2].Score < Players[1].Score)
            {

                Winner.Add(Players[2]);
            }

            else if (Players[0].Score == Players[1].Score && Players[0].Score < Players[2].Score)
            {

                Winner.Add(Players[0]);
                Winner.Add(Players[1]);
            }
            else if (Players[0].Score == Players[2].Score && Players[0].Score < Players[1].Score)
            {

                Winner.Add(Players[0]);
                Winner.Add(Players[2]);
            }

            else if (Players[1].Score == Players[2].Score && Players[1].Score < Players[0].Score)
            {
                Winner.Add(Players[1]);
                Winner.Add(Players[2]);
            }
            else 
            {
                Winner.Add(Players[0]);
                Winner.Add(Players[1]);
                Winner.Add(Players[2]);
            }

        }


        /// <summary>
        /// Returns the score of the players in string format;
        /// </summary>
        /// <returns></returns>
        public string DisplayScore()
        {
            string display = "";

            Players.ForEach(p => display += $"\n{p.Name}: \t \t{p.Score}");

            return display;
        }
        #endregion

    }
}
