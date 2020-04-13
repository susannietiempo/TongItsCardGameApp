using BOLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Controls
{
    public class HandEvaluator
    {
        #region Properties
        #endregion
        #region Methods

        public static int TotalHandPoints(Hand playerHand)
        {
            int points = 0;
            for (int i = 0; i < playerHand.Count; i++)
            {
                int cardPoints = (int)playerHand[i].FaceValue;
                points += cardPoints;
            }
            return points;
        }
        #endregion

    }
}
