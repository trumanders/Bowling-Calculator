namespace MAU_lab7_BowlingCalculator;
public class PlayerManager
{
    private static int maxNumberOfPlayers = 6;
    private int numberOfPlayers;
    private Player[] allPlayers;

    public int NumberOfPlayers { get { return numberOfPlayers; } }
    public static int MaxNumberOfPlayers { get { return maxNumberOfPlayers; } }

    public PlayerManager(int numberOfPlayers)
    {
        if (numberOfPlayers > 0 && numberOfPlayers <= maxNumberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
            allPlayers = new Player[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                allPlayers[i] = new Player();
            }            
        }
        else
        {
            MessageBox.Show("Please chose 1-" + maxNumberOfPlayers + " players");
        }            
    }


    /// <summary>
    /// Gets one player object.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>One player object.</returns>
    public Player GetPlayer(int index)
    {
        return allPlayers[index];
    }
}
