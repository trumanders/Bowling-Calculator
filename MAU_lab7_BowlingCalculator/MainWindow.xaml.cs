using System.Diagnostics;
using System.Windows.Input;

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
        NumberOfPlayersDialog numberOfPlayersDialog = new NumberOfPlayersDialog(PlayerManager.MaxNumberOfPlayers);
        numberOfPlayersDialog.ShowDialog();
        this.numberOfPlayers = numberOfPlayersDialog.NumberOfPlayers;

        if (numberOfPlayers == 0)
            Application.Current.Shutdown();
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

                // Set select all text on focus for the textboxes
                layout.PlayerManager.GetPlayer(player).Scoreboard.GetTextBox(box).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(AutoSelectOnKeyboard);

                // Set select text when clicking on textboxes
                layout.PlayerManager.GetPlayer(player).Scoreboard.GetTextBox(box).MouseLeftButtonDown += new MouseButtonEventHandler(AutoSelectOnMouse);

                // This event is needed to make the selection not immediately go away
                layout.PlayerManager.GetPlayer(player).Scoreboard.GetTextBox(box).LostMouseCapture += new MouseEventHandler(AutoSelectOnMouse);
            }
        }             
    }


    /// <summary>
    /// Fires when the user selects a ball score textbox with the keyboard, and selects all text in it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AutoSelectOnKeyboard(object sender, KeyboardFocusChangedEventArgs e)
    {
        TextBox textbox = sender as TextBox;
        if (textbox.Text != "1")
            textbox.SelectAll();
    }

    /// <summary>
    /// Fires when the user selects a ball score textbox with the mouse, and selects all text in it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AutoSelectOnMouse(object sender, MouseEventArgs e)
    {
        TextBox textbox = sender as TextBox;
        if (textbox.Text != "1")
            textbox.Select(0, 2);
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
        TextBox textbox = sender as TextBox;
        if (Scoreboard.DisableTextChangeEventHandler)
            return;

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