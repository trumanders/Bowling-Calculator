using System.Diagnostics;

namespace MAU_lab7_BowlingCalculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private int numberOfPlayers;    /* UI INPUT */
    private Layout layout;
    private Calculator calculator;

    public MainWindow()
    {
        InitializeComponent();

        // Show dialog for number of players input
        NumberOfPlayersDialog numberOfPlayersDialog = new NumberOfPlayersDialog();
        numberOfPlayersDialog.ShowDialog();
        this.numberOfPlayers = numberOfPlayersDialog.NumberOfPlayers;

        layout = new Layout(this.Width, numberOfPlayers);        
        
        // Set window height based in the gridLayout parameters set in GenerateMainGrid-method.
        this.Height = layout.GetWindowHeight();
        this.Content = layout.GetViewbox();

        SetBallScoreInputListeners();
        SetResetButtonClickListener();
        calculator = new Calculator();
        layout.SetTextboxStyles();
    }


    /// <summary>
    /// Set text change listeners for the ball-score textboxes.
    /// </summary>
    private void SetBallScoreInputListeners()
    {
        // Iterate through players
        for (int player = 0; player < numberOfPlayers; player++)
        {
            // Iterate through each ball-score textbox for each player's scoreboard
            for (int box = 1; box <= Scoreboard.NumberOfBallScores; box++)
            {                
                // Define the text change event handler for each textbox.
                layout.PlayerManager.GetPlayer(player).Scoreboard.GetTextBox(box).TextChanged += new TextChangedEventHandler(CalculateAll);
            }
        }             
    }

    private void SetResetButtonClickListener()
    {
        layout.ResetButton.Click += new RoutedEventHandler(btn_reset_Click);
    }


    /// <summary>
    /// Event handler called on text change on the ball-score textboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CalculateAll(object sender, TextChangedEventArgs e)
    {       
        calculator.CalculateAll(layout.PlayerManager);        
    }


    /// <summary>
    /// Is called when the reset button is called. This method restarts the application.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btn_reset_Click(object sender, RoutedEventArgs e)
    {
        Process.Start(Process.GetCurrentProcess().MainModule.FileName);
        Application.Current.Shutdown();
    }
}