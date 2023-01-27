namespace MAU_lab7_BowlingCalculator;

/// <summary>
/// This class has methods for calculating the score and keeping track of
/// strikes and spares.
/// </summary>
public class Calculator
{
    private Round[] allRounds;
    private Scoreboard scoreboard;

    public Calculator()
    {
        allRounds = new Round[Scoreboard.NumberOfRounds];
    }


    /// <summary>
    /// Calls the methods for analyzing and setting the score, each time the user changes something in the scoreboard.
    /// The playerManager is passed in to be able to iterate through players to calculate on each player's scoreboard.
    /// </summary>
    /// <param name="playerManager"></param>
    public void CalculateAll(PlayerManager playerManager)
    {
        // Loop through all player scoreboards and update everything
        for (int i = 0; i < playerManager.NumberOfPlayers; i++)
        {
            this.scoreboard = playerManager.GetPlayer(i).Scoreboard;
            scoreboard.UIScoresToRounds();
            SetStrikeAndSpare();
            CalculateScores();

            // When the scoreboard's rounds are set, convert back to output
            scoreboard.RoundScoreToUI();
        }
    }


    /// <summary>
    /// Go through the round scores and set attributes spare or strike.
    /// </summary>
    private void SetStrikeAndSpare()
    {
        allRounds = scoreboard.AllRounds;

        for (int i = 0; i < allRounds.Length; i++)
        {
            if (allRounds[i].FirstScore == -1 || (allRounds[i].FirstScore != 10 && allRounds[i].SecondScore == -1))
                return;

            // If first ball score is 10, set round to STRIKE, unless it is the last round
            if (allRounds[i].FirstScore == 10 && i != 9)
            {
                allRounds[i].IsStrike = true;
            }

            // If round is NOT strike, and not last round
            else if (!allRounds[i].IsStrike && i != 9)
            {
                // If first ball + second ball is 10, then round is SPARE
                if (allRounds[i].FirstScore + allRounds[i].SecondScore == 10 && allRounds[i].SecondScore != 0)
                {
                    allRounds[i].IsSpare = true;
                }
            }

            //// Last round is strike on first ball or SPARE - If last round and ball 1+2 is 10 or more...
            //if (i == 9 && (allRounds[9].FirstScore + allRounds[9].SecondScore >= 10) || allRounds[9].SecondScore == 1)
            //{
            //    // Activate last round's third ball
            //    allRounds[9].ThirdScoreActive = true;
            //}
            //else allRounds[9].ThirdScoreActive = false;
        }
    }


    /// <summary>
    /// Takes the ball-score values and calculates the score for each round.
    /// It keeps track of which round scores should show and not, based on strikes and spares.
    /// </summary>
    private void CalculateScores()
    {
        // Iterate through ball scores (one iteration for each ball, 21 ball scoers)
        for (int currentBall = 0; currentBall < Scoreboard.NumberOfBallScores - 1; currentBall++)
        {
            bool isFirstBall = false, isSecondBall = false, isThirdBall = false;

            if (currentBall % 2 == 0)
            {
                isFirstBall = true;
                isSecondBall = false;
                isThirdBall = false;
            }
            else
            {
                isFirstBall = false;
                isSecondBall = true;
                isThirdBall = false;
            }
            if (currentBall == 20)
            {
                isFirstBall = false;
                isSecondBall = false;
                isThirdBall = true;
            }
                

            int currentRound = currentBall / 2;    // Two balls for each round.
            int firstScore = allRounds[currentRound].FirstScore;
            int secondScore = allRounds[currentRound].SecondScore;
            int thirdScore = allRounds[currentRound].ThirdScore;
            Round thisRound = allRounds[currentRound];
            Round prevRound = new Round();
            Round prevPrevRound = new Round();
            if (currentRound > 0)
            {
                prevRound = allRounds[currentRound - 1];
            }

            if (currentRound > 1)
            {
                prevPrevRound = allRounds[currentRound - 2];
            }

            if (IsSkippingScoreChecking(allRounds, currentRound, isFirstBall))
            {                
                return;
            }

            // On first ball - both previous rounds are strikes - (from round number 3)
            if (currentRound > 1 && isFirstBall && prevPrevRound.IsStrike && prevRound.IsStrike)
            {
                if (currentRound > 2)
                    prevPrevRound.Score = allRounds[currentRound - 3].Score + 20 + firstScore;
                else if (currentRound == 2)
                    prevPrevRound.Score = 20 + firstScore;
            }

            /* On first ball - Previous is Spare */
            else if (currentRound > 0 && isFirstBall && prevRound.IsSpare)
            {
                if (currentRound > 1)
                    prevRound.Score = prevPrevRound.Score + 10 + firstScore;
                else if (currentRound == 1)
                    prevRound.Score = 10 + firstScore;
            }

            // If current is strike - skip to next round - only calculate this round after two rounds. 
            if (thisRound.IsStrike) continue;

            /* On second ball - Previous round is Strike - set previous round score to include it's strike score + both balls on current round */
            else if (currentRound > 0 && isSecondBall && prevRound.IsStrike)
            {
                // Include round score from two rounds back + previous round's strike score (10) + current round score
                if (currentRound > 1)                
                    prevRound.Score = prevPrevRound.Score + 10 + firstScore + secondScore;

                // If on second round - include round score + ball score from first round.
                else if (currentRound == 1)
                    prevRound.Score = 10 + firstScore + secondScore;

                // Set current round score if not spare or strike
                if (!thisRound.IsStrike && !thisRound.IsSpare)
                    thisRound.Score = prevRound.Score + firstScore + secondScore;
            }

            /* On second ball - current AND previous is not Stike and not Spare */
            else if (currentRound > 0 && (isSecondBall || isThirdBall) && !thisRound.IsSpare && !thisRound.IsStrike && !prevRound.IsSpare)
                thisRound.Score = prevRound.Score + firstScore + secondScore;

            /* On second ball, not first round - Current is not Strike NOR Spare*/
            else if (currentRound != 0 && isSecondBall && !thisRound.IsSpare && !thisRound.IsStrike && prevRound.Score > 0)
                thisRound.Score = prevRound.Score + firstScore + secondScore;

            // If on first round, second ball and first + second is not 10
            else if (currentRound == 0 && isSecondBall && firstScore + secondScore != 10)
                thisRound.Score = firstScore + secondScore;

            // If on last round
            if (currentBall == 18)            
                thisRound.Score = prevRound.Score + thisRound.FirstScore;
            if (currentBall == 19)
                thisRound.Score = prevRound.Score + thisRound.FirstScore + thisRound.SecondScore;
            if (currentBall == 20)
                thisRound.Score = prevRound.Score + thisRound.FirstScore + thisRound.SecondScore + thisRound.ThirdScore;

            /* Wait for user to enter third score on last round before adding it (otherwise it is set to -1 */
            //if (thisRound.ThirdScore >= 0)
            //        thisRound.Score = allRounds[9].FirstScore + allRounds[9].SecondScore + allRounds[9].ThirdScore + allRounds[8].Score;
            //}


            /* Configure the final score textbox based on scores on the last round */
            // If third score is enabled (only possible for last round) and that score is entered (not -1).         
            if (allRounds[currentRound].ThirdScore >= 0)
            {
                /* Mark final score textbox*/
                scoreboard.GetTextBox(31).BorderBrush = System.Windows.Media.Brushes.Green;
                scoreboard.GetTextBox(31).BorderThickness = new Thickness(5, 5, 5, 5);
            }
            else if (currentBall == 19 && allRounds[currentRound].SecondScore >= 0 && allRounds[currentRound].SecondScore + allRounds[currentRound - 1].SecondScore < 10)
            {
                scoreboard.GetTextBox(31).BorderBrush = System.Windows.Media.Brushes.Green;
                scoreboard.GetTextBox(31).BorderThickness = new Thickness(5, 5, 5, 5);
            }

            else
            {
                // Normal border
                scoreboard.GetTextBox(31).BorderBrush = System.Windows.Media.Brushes.Black;
                scoreboard.GetTextBox(31).BorderThickness = new Thickness(3,3,3,3);
            }

           
        }
    }


    /// <summary>
    /// Checks if textboxes are empty, and there is no need to keep checking texboxes. 
    /// </summary>
    /// <param name="allRounds">The array of round objects</param>
    /// <param name="currentRound">The current round that is beeing checked.</param>
    /// <param name="isFirstBall">Is it the first or second ball score that is beeing checked?</param>
    /// <returns></returns>
    private bool IsSkippingScoreChecking(Round[] allRounds, int currentRound, bool isFirstBall)
    {
        bool skipScoreChecking = false;
        
        // Skip checking if no score is entered for current round, or if no second score is entered for the first round (no previous strike or spare possible to wait for)
        if (allRounds[currentRound].FirstScore == -1 || (allRounds[currentRound].SecondScore == -1 && (currentRound == 0 || !isFirstBall)))
            return true;

        // Skip checking if second score is not entered, unless previous round is strike, or the two previous was strike (then they would be waiting for the first score)
        if (allRounds[currentRound].SecondScore == -1 && currentRound > 0)
        {
            if (currentRound > 1)
            {
                if (allRounds[currentRound - 2].IsStrike) return false;
                if (allRounds[currentRound - 1].IsSpare) return false;
            }
            else if (currentRound > 0)
            {
                if (allRounds[currentRound - 1].IsSpare) return false;
            }
            return true;
        }
        return false;
    }
}