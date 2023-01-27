namespace MAU_lab7_BowlingCalculator;

public class Round
{        
    private int firstScore = -1;
    private int secondScore = -1;
    private int thirdScore = -1;
    private int score;
    private bool isSpare;
    private bool isStrike;


    public int FirstScore { get { return firstScore; } set { firstScore = value; } }
    public int SecondScore { get { return secondScore; } set { secondScore = value; } }
    public int ThirdScore { get { return thirdScore; } set { thirdScore = value; } }
    public int Score { get { return score; } set { score = value; } }        
    public bool IsSpare { get { return isSpare; } set { isSpare = value; } }
    public bool IsStrike { get { return isStrike; } set { isStrike = value; } }    


    /// <summary>
    /// Resets the round parameters.
    /// </summary>
    /// <param name="score">The round score.</param>
    /// <param name="firstScore">First ball-score for a round.</param>
    /// <param name="secondScore">Second ball-score for a round. </param>
    /// <param name="isSpare">Is the round a spare or not?</param>
    /// <param name="isStrike">Is the round a strike or not?</param>
    /// <param name="thirdScoreActive">Is the third score enabled for the round (only for last round)</param>
    public void SetParamenters(int score, int firstScore, int secondScore, bool isSpare, bool isStrike, bool thirdScoreActive)
    {
        this.score = score;
        this.firstScore = firstScore;
        this.secondScore = secondScore;
        this.isSpare = isSpare;
        this.isStrike = isStrike;
    }
}
