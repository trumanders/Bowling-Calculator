namespace MAU_lab7_BowlingCalculator;

/// <summary>
/// Contains methods that designs the layout for the grid and textboxes of the bowling scoreboard.
/// </summary>
public class Layout
{
    private const double BORDER_MARGIN_RATIO = 0.01;        // The width of the margin around the main grid
    private const double RIGHT_UI_PART_RATIO = 0.2;         // The width of space reserved for other UI-stuff.
    private const int scoreboardWidthSquareCount = 24;      // The scoreboard width in grid squares 
    private const int scoreboardHeightSquareCount = 2;      // The scoreboard height in grid squares                                     
    private double windowWidthPixels;                       
    private double borderMarginPixels;
    private double gridSquareSizePixels;                    // The size of the grid squares in pixels
    private int numberOfColumns;                            // Number of main grid collumns
    private int numberOfRows;                               // Number of main grid rows
    private Grid mainGrid;
    private PlayerManager playerManager;
    private bool showGridLines = false;                     // Show grid lines (for development)

    public PlayerManager PlayerManager { get { return playerManager; } }    
    public Button ResetButton { get; private set; }

    public Layout(double windowWidth, int numberOfPlayers)
    {
        // Create grid object
        this.mainGrid = new Grid();
        this.playerManager = new PlayerManager(numberOfPlayers);
        this.windowWidthPixels = windowWidth;
        GenerateMainGrid();
    }


    /// <summary>
    /// Generates a grid of sqares based on the class variables, and number of players (number of scoreboards)
    /// </summary>
    /// <param name="windowWidth">The window width in pixels</param>
    /// <param name="windowHeight">The window height in pixels</param>
    public void GenerateMainGrid()
    {        
        this.mainGrid.Width = this.windowWidthPixels;        
        this.mainGrid.ShowGridLines = showGridLines;
        // Determine grid size (each square)
        // Subtract borders and the right UI part
        this.borderMarginPixels = windowWidthPixels * BORDER_MARGIN_RATIO;      // The margin size in pixels
        double rightUIPartPixels = windowWidthPixels * RIGHT_UI_PART_RATIO;     // Other UI width in pixels
        double scoreboardWidthPixels = windowWidthPixels - borderMarginPixels - rightUIPartPixels;  // The scoreboard width in pixels

        gridSquareSizePixels = scoreboardWidthPixels / scoreboardWidthSquareCount;
        CreateRows();
        CreateColumns();
        AddControlsToGrid();
    }


    /// <summary>
    /// Calculates the window height in pixels based on number of rows and margin size.
    /// </summary>
    /// <returns>Window height in pixels.</returns>
    public int GetWindowHeight()
    {
        int numOfMarginRows = 2;
        return (int)((numberOfRows - numOfMarginRows) * gridSquareSizePixels + (borderMarginPixels * 2));
    }



    /// <summary>
    /// Creates and configures a ViewBox object to put the main grid in.
    /// </summary>
    /// <returns>A ViewBox object.</returns>
    public Viewbox GetViewbox()
    {
        Viewbox viewbox = new Viewbox();
        viewbox.StretchDirection = StretchDirection.Both;
        viewbox.Stretch = Stretch.Fill;
        viewbox.Child = mainGrid;
        return viewbox;
    }


    /// <summary>
    /// Configures the textboxes for the scoreboard
    /// </summary>
    public void SetTextboxStyles()
    {
        for (int player = 0; player < playerManager.NumberOfPlayers; player++)
        {
            // Final score textbox border (31)
            TextBox scoreText = PlayerManager.GetPlayer(player).Scoreboard.GetTextBox(31);
            scoreText.BorderBrush = Brushes.Black;
            scoreText.BorderThickness = new Thickness(3, 3, 3, 3);
            scoreText.TextAlignment = TextAlignment.Center;
            scoreText.VerticalContentAlignment = VerticalAlignment.Center;


            // Player name textboxes (0)
            TextBox playerTextbox = PlayerManager.GetPlayer(player).Scoreboard.GetTextBox(0);
            playerTextbox.BorderThickness = new Thickness(3, 3, 3, 3);
            playerTextbox.FontSize = 22;
            playerTextbox.FontWeight = FontWeights.Bold;
            playerTextbox.TextAlignment = TextAlignment.Center;
            playerTextbox.VerticalContentAlignment = VerticalAlignment.Center;
                        
            //for (int textbox = 1; textbox <= 21; textbox++)
            //{
            //    TextBox ballScoreTextbox = PlayerManager.GetPlayer(player).Scoreboard.GetTextBox(textbox);
            //    ballScoreTextbox.GetKeyBoard
            //}
        }

        // Set selectAll for ball-score textboxes
        
    }


    /// <summary>
    /// Creates row definitions and set the dimensions for the rows
    /// </summary>
    private void CreateRows()
    {
        // Create rows
        numberOfRows = (playerManager.NumberOfPlayers * (scoreboardHeightSquareCount + 1) + 3); // +1 : space between scoreboards
        RowDefinition[] mainRows = new RowDefinition[numberOfRows];  
        

        for (int i = 0; i < numberOfRows; i++)
        {
            mainRows[i] = new RowDefinition();
            
            // Set grid dimensions
            if (i == 0 || i == numberOfRows - 1)                            // Create margin grid for first and last row
                mainRows[i].Height = new GridLength(borderMarginPixels);
            else
            {
                mainRows[i].Height = new GridLength(gridSquareSizePixels);
                mainGrid.RowDefinitions.Add(mainRows[i]);
            }
        }
    }


    /// <summary>
    ///  Creates row definitions and set the dimensions for the collumns
    /// </summary>
    private void CreateColumns()
    {
        numberOfColumns = (int)(windowWidthPixels / gridSquareSizePixels);
        // Create columns (2 columns: 1 for scoreboard, 1 for other UI)
        ColumnDefinition[] mainColumns = new ColumnDefinition[numberOfColumns];  /* Two columns for scoreboard and UI, plus*/
        for (int i = 0; i < numberOfColumns; i++)
        {
            mainColumns[i] = new ColumnDefinition();

            if (i == 0 || i == numberOfColumns - 1)
                mainColumns[i].Width = new GridLength(borderMarginPixels);
            else
                mainColumns[i].Width = new GridLength(gridSquareSizePixels);
            mainGrid.ColumnDefinitions.Add(mainColumns[i]);
        }
    }


    /// <summary>
    /// Adds the scoreboard textboxes and the round-number textblocks
    /// </summary>
    private void AddControlsToGrid()
    {        
        for (int player = 0; player < playerManager.NumberOfPlayers; player++)
        {
            // Add textboxes
            for (int textbox = 0; textbox < Scoreboard.NumberOfTextBoxesInScoreboard; textbox++)
            {
                TextBox textboxToAdd = playerManager.GetPlayer(player).Scoreboard.GetTextBox(textbox);
                mainGrid.Children.Add(textboxToAdd);
            }

            // Add round number textblocks
            int numberOfRounds = Scoreboard.NumberOfRounds;
            for (int i = 0; i < numberOfRounds; i++)
            {
                TextBlock textBlockToAdd = playerManager.GetPlayer(player).Scoreboard.GetRoundNumberTextBlock(i);
                mainGrid.Children.Add(textBlockToAdd);
            }            
        }

        // Add reset button
        ResetButton = new Button();
        ResetButton.Content = "Reset";
        ResetButton.FontSize = 15;
        Grid.SetColumn(ResetButton, 26);
        Grid.SetRow(ResetButton, 1);
        Grid.SetColumnSpan(ResetButton, 3);
        Grid.SetRowSpan(ResetButton, 2);
        mainGrid.Children.Add(ResetButton);
    }   
}
