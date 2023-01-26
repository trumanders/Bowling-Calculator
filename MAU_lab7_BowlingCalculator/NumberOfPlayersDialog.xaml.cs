namespace MAU_lab7_BowlingCalculator;

/// <summary>
/// Shows dialog window for user to input number of players.
/// </summary>
public partial class NumberOfPlayersDialog : Window
{
    public int NumberOfPlayers { get; private set; }
    private int maxNumberOfPlayers;

   public NumberOfPlayersDialog(int maxNumberOfPlayers)
    {
        InitializeComponent();
        //this.Closing += new CancelEventHandler(Window_Closing);
        NumberOfPlayers = 0;
        this.maxNumberOfPlayers = maxNumberOfPlayers;
        tbx_numOfPlayers.Focus();
    }


    /// <summary>
    /// When the user clicks the ok-button, the method to check for valid input is called.
    /// If valid, the numberOfPlayers property is set, and the window is closed. If invalid, 
    /// a massagebox is shown with instructions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btn_dialogOk_Click(object sender, RoutedEventArgs e)
    {
        if (IsValidInput(tbx_numOfPlayers.Text))
        {
            NumberOfPlayers = Convert.ToInt32(tbx_numOfPlayers.Text);
            this.Close();
        }            
        else
        {
            MessageBox.Show("Please chose 1-" + maxNumberOfPlayers + " players.");
        }
           
    }  


    /// <summary>
    /// Checks the user input. Must be an integer between 1-6.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>Bool: True if integer 1-6, otherwise false.</returns>
    private bool IsValidInput(string input)
    {
        return int.TryParse(input, out int num) && num > 0 && num <= maxNumberOfPlayers;
    }
}
