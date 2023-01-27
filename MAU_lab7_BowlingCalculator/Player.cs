namespace MAU_lab7_BowlingCalculator;

public class Player
{
    private Scoreboard scoreboard;
    private static int playerNumber = 0;
    public Scoreboard Scoreboard {  get { return scoreboard; } }    

    public Player()
    {
        playerNumber++;
        this.scoreboard = new Scoreboard(playerNumber);
    }
}
