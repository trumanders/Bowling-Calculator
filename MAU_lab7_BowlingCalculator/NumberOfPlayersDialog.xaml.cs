namespace MAU_lab7_BowlingCalculator;

/// <summary>
/// Shows dialog window for user to input number of players.
/// </summary>
public partial class NumberOfPlayersDialog : Window
{
    public int NumberOfPlayers { get; private set; }
    public NumberOfPlayersDialog()
    {
        InitializeComponent();
        tbx_numOfPlayers.Focus();
    }


    private void btn_dialogOk_Click(object sender, EventArgs e)
    {
        this.NumberOfPlayers = Convert.ToInt32(tbx_numOfPlayers.Text);
        this.Close();
    }
}
