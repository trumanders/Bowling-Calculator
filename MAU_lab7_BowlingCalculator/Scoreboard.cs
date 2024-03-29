﻿namespace MAU_lab7_BowlingCalculator;

/// <summary>
/// Contains methods for creating the textboxes for the scoreboard.
/// It also manages the reading from the scoreboard when calculatng the score.
/// </summary>
public class Scoreboard
{
    public const int columns = 24;      // The number of grid squares for the width of the scoreboard
    public const int rows = 2;          // The number of grid squares for the height of the scoreboard
    private static int numberOfRounds = 10;
    private static int numberOfTextBoxesInScoreboard = 32;
    private static int numberOfBallScores = 21;    
    private int scoreboardNumber;
    private TextBox[] textBoxes;
    private TextBlock[] roundNumberText;
    private Round[] allRounds;


    public static int NumberOfRounds { get { return numberOfRounds; } }
    public static int NumberOfBallScores { get { return numberOfBallScores; } }
    public static int NumberOfTextBoxesInScoreboard { get { return numberOfTextBoxesInScoreboard; } }
    public Round[] AllRounds { get { return allRounds; } }


    public Scoreboard(int scoreboardNumber)
    {
        allRounds = new Round[numberOfRounds];
        textBoxes = new TextBox[numberOfTextBoxesInScoreboard];
        roundNumberText = new TextBlock[numberOfRounds];
        this.scoreboardNumber = scoreboardNumber;
        InitializeRoundParameters();
        ConfigureRoundNumberTextBlocks();
        ConfigureTextBoxes();
        CreateScoreboard();
    }


    /// <summary>
    /// Creates the scoreboard, sets the parameters for the textboxes.
    /// </summary>
    private void CreateScoreboard()
    {
        int adjustRowOrColumn = 0;
        for (int i = 0; i < textBoxes.Length; i++)
        {
            // textbox[0] is column 1-3
            // Adjust rows

            // Adjust the row to space the scoreboards correctly
            // Board number 1 gives row 1, board number 2 gives row 4, and so on.
            int row = (scoreboardNumber * 3) - 2;

            if (i == 0)
            {
                // Add name textbox on first column of the scoreboard
                Grid.SetRow(textBoxes[i], row);
                Grid.SetColumn(textBoxes[i], i + 1);
                Grid.SetRowSpan(textBoxes[i], 2);
                Grid.SetColumnSpan(textBoxes[i], 3);
            }

            // Add first row of textboxes (score for each ball)
            if (i >= 1 && i <= 21)
            {
                Grid.SetRow(textBoxes[i], row);
                Grid.SetColumn(textBoxes[i], i + 3);
            }

            // Add second row of text boxes (score for each round)
            if (i >= 22 && i <= 31)
            {
                Grid.SetRow(textBoxes[i], row + 1);
                Grid.SetColumn(textBoxes[i], i - 18 + adjustRowOrColumn);
                Grid.SetColumnSpan(textBoxes[i], 2);
                textBoxes[i].IsReadOnly = true;                         /* No user input in round-score boxes */
                adjustRowOrColumn++;

                // Add round number text blocks                
                Grid.SetRow(roundNumberText[i - 22], row - 1);          /* Place above first score row */
                Grid.SetColumn(roundNumberText[i - 22], i - 18 + adjustRowOrColumn);
                Grid.SetColumnSpan(roundNumberText[i - 22], 2);         /* Normal round has*/

                if (i == 31)
                    Grid.SetColumnSpan(textBoxes[i], 3);                /* Last round has three balls (columnSpan = 3)*/
            }
        }
    }


    /// <summary>
    /// Gets one textbox from the array, based in the passed in index
    /// </summary>
    /// <param name="index"></param>
    /// <returns>One textbox.</returns>
    public TextBox GetTextBox(int index)
    {
        return textBoxes[index];
    }

    /// <summary>
    /// Gets one textblock from the Round number array.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>One textblock.</returns>
    public TextBlock GetRoundNumberTextBlock(int index)
    {
        return roundNumberText[index];
    }


    /// <summary>
    /// Configure the textblocks visuals.
    /// </summary>
    private void ConfigureRoundNumberTextBlocks()
    {
        for (int i = 0; i < numberOfRounds; i++)
        {
            roundNumberText[i] = new TextBlock();
            roundNumberText[i].Text = (i + 1).ToString();
            roundNumberText[i].TextAlignment = TextAlignment.Left;
            roundNumberText[i].VerticalAlignment = VerticalAlignment.Bottom;
            roundNumberText[i].FontWeight = FontWeights.Bold;
        }
    }


    /// <summary>
    /// Textboxes visuals.
    /// </summary>
    private void ConfigureTextBoxes()
    {
        for (int i = 0; i < numberOfTextBoxesInScoreboard; i++)
        {
            textBoxes[i] = new TextBox();
            textBoxes[i].TextAlignment = TextAlignment.Center;
            textBoxes[i].FontSize = 16;
        }
    }



    /// <summary>
    /// Outputs the round object's score to the textboxes.
    /// </summary>
    public void RoundScoreToUI()
    {
        // iterate through rounds and set the score boxes
        for (int i = 0; i < allRounds.Length; i++)
        {
            // Round score boxes are 22 - 31
            if (allRounds[i].Score == -1)
            {
                textBoxes[i + 22].Text = "";
            }
            else
                textBoxes[i + 22].Text = allRounds[i].Score.ToString();
        }
    }


    /// <summary>
    /// Gets one Round object
    /// </summary>
    /// <param name="index">The index of the round to return.</param>
    /// <returns>One Round object.</returns>
    public Round GetRound(int index)
    {
        return allRounds[index];
    }


    /// <summary>
    /// Initialize the variables for all rounds.
    /// </summary>
    private void InitializeRoundParameters()
    {

        for (int i = 0; i < numberOfRounds; i++)
        {
            allRounds[i] = new Round();
            allRounds[i].SetParamenters(-1, -1, -1, false, false, false);
        }
    }



    /// <summary>
    /// Go through the textboxes and convert the score input to round-object with valid checking
    /// </summary>
    public void UIScoresToRounds()
    {

        ResetRounds();

        // Set third ball on round 10 as disabled as default
        //textBoxes[21].IsReadOnly = true;

        bool isValidScore = true;
        bool isFirstBall = false;
        bool isSecondBall = false;
        bool isThirdBall = false;

        bool wasStrike = false;

        // The textboxes are number 1-21 in the textbox array
        for (int i = 1; i <= Scoreboard.NumberOfBallScores; i++)
        {

            int currentRound = (i - 1) / 2;

            /* Disable last round's third score if score 1 and 2 is less than 10 */
            if (i == 20 && allRounds[9].FirstScore + allRounds[9].SecondScore < 10)
            {
                textBoxes[21].IsReadOnly = true;
                textBoxes[i].IsEnabled = true;
            }

            // Last score box
            if (i == 21)
            {
                currentRound = 9;       /* Prevent out of bounds since last round has three textboxex */
                isFirstBall = false;
                isSecondBall = false;
                isThirdBall = true;
            }

            // Second ball
            else if (i % 2 == 0)
            {
                isFirstBall = false;
                isSecondBall = true;
                isThirdBall = false;
            }

            // First ball
            else if (i % 2 != 0)
            {
                isFirstBall = true;
                isSecondBall = false;
                isThirdBall = false;
            }

            // Invalid exceptons:
            // If invalid score or no score, set ball-score -1 to indicate that it is not even zero/not set yet
            // Invalid is: not digit, over 10, under 0, if a score is entered without the one before being set, and if firstScore + secondScore > 10

            // Not digit, under 0 or over 10

            if (!int.TryParse(textBoxes[i].Text, out int ballScore))
            {
                isValidScore = false;

                if (isSecondBall && textBoxes[i].Text == "" && allRounds[currentRound].FirstScore == 10) // KEEP THIS
                {
                    isValidScore = true;              // KEEP THIS                    
                }
            }
            
            else if (ballScore < 0 || ballScore > 10)
                isValidScore = false;

            // First score is entered without the score before beeing entered
            else if (i > 1 && isFirstBall && allRounds[currentRound - 1].SecondScore == -1)
                isValidScore = false;

            // Second score is entered without first score beeing entered
            else if (isSecondBall && allRounds[currentRound].FirstScore == -1)
                isValidScore = false;

            // Third score on last round is entered without second score beeing entered
            else if (isThirdBall && allRounds[9].SecondScore == -1)
                isValidScore = false;

            // Entered second ball score too high
            else if (isSecondBall && Convert.ToInt32(textBoxes[i].Text) + Convert.ToInt32(textBoxes[i - 1].Text) > 10)
            {
                // If on last round-second ball, and first ball is strike, second ball can make 10 points too.
                if (i == 20 && allRounds[currentRound].FirstScore == 10) isValidScore = true;
                else isValidScore = false;
            }

            // Don't allow '0' (zero) on second score when first score is 10 (strike), unless it's the last round
            if (isSecondBall && allRounds[currentRound].FirstScore == 10 && textBoxes[i].Text == "0" && currentRound != 9)
                isValidScore = false;


            if (!isValidScore)
            {
                if (isFirstBall) allRounds[currentRound].FirstScore = -1;
                else if (isSecondBall) allRounds[currentRound].SecondScore = -1;
                else if (isThirdBall) allRounds[currentRound].ThirdScore = -1;
                textBoxes[i].Text = "";  // make output later
            }


            else if (isValidScore)
            {
                if (isFirstBall)
                {
                    allRounds[currentRound].FirstScore = ballScore;
                    if (ballScore == 10)
                    {
                        allRounds[currentRound].SecondScore = 0;
                    }

                }
                else if (isSecondBall)
                {
                    allRounds[currentRound].SecondScore = ballScore;
                }
                else if (isThirdBall) allRounds[currentRound].ThirdScore = ballScore;
            }

            // When on second score, round 10 (textbox 20), decice if third ball will be enabled:
            // If round 10's first + second ball score >= 10, then the third score is enabled, otherwise, exit loop.
            if (i == 20)
            {
                if (allRounds[currentRound].FirstScore + allRounds[currentRound].SecondScore < 10)
                {
                    return;
                }
                else
                {
                    // Enable third score
                    textBoxes[21].IsReadOnly = false;
                }
            }
        }
    }

    public void EmptyScoreboard()
    {
        foreach (var textbox in textBoxes)
        {
            textbox.Text = "";
        }
    }

    public void ResetRounds()
    {
        foreach (var round in allRounds)
        {
            round.FirstScore = -1;
            round.SecondScore = -1;
            round.ThirdScore = -1;
            round.Score = -1;
            round.IsSpare = false;
            round.IsStrike = false;
            round.ThirdScoreActive = false;
        }
    }
}
