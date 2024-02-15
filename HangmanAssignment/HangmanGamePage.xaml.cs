using System.ComponentModel;
using System.Diagnostics;

namespace HangmanAssignment;

public partial class HangmanGamePage : ContentPage, INotifyPropertyChanged
{
    #region fields
    public string Spotlight
    {
        get => _spotlight;

        set
        {
            _spotlight = value;
            OnPropertyChanged();
        }
    }

    List<string> words = new List<string>()
     {
        "Gucci",
        "Yves Saint Laurent",
        "Marc Jacobs",
        "Ralph Lauren",
        "Calvin Klein",
        "Hermes",
        "Giorgio Armani",
        "Burberry",
        "Bvlgari",
        "Balenciaga",
        "Fendi",
        "Versace",
        "Givenchy",
        "Louis Vuitton",
        "Chanel",
        "Christian Dior",
        "Van Cleef",
        "Valentino",
        "Coach",
        "Prada",
        "Alexander McQueen",
        "Dolce & Gabbana",
        "Cartier"
     };

    List<char> guessed = new();

    string answer = string.Empty;

    private string _spotlight;

    private int error = 0;

    public string CurrentImageName
    {
        get => _currentImageName; set
        {
            _currentImageName = value;
            OnPropertyChanged();
        }
    }

    public string GameStatus
    {
        get => _gameStatus; set
        {
            _gameStatus = value;
            OnPropertyChanged();
        }
    }

    public string Message
    {
        get => _message; set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    public List<char> Letters
    {
        get => _letters;
        set
        {
            _letters = value;
            OnPropertyChanged();
        }
    }

    private List<char> _letters = new();
    private string _message;
    private string _gameStatus = "Errors : 0 of 8";
    private string _currentImageName = "hang1.png";
    #endregion

    public HangmanGamePage()
    {
        InitializeComponent();
        _letters.AddRange("abcdefghijklmnopqrstuvwxyz&");
        _letters.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        BindingContext = this;
        PickWord();
        CalculateWord(answer, guessed);
    }

    #region gameEngine

    private void PickWord()
    {
        answer = words[new Random().Next(0, words.Count)];

        Debug.WriteLine(answer);
    }

    private void CalculateWord(string answer, List<char> guessed)
    {
        var temp = answer.Select(x => guessed.IndexOf(x) >= 0 ? x : '_')
            .ToArray();

        Spotlight = string.Join(' ', temp);
    }

    #endregion

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn != null)
        {
            var letter = btn.Text;

            btn.IsEnabled = false;

            HandleGuess(letter[0]);
        }

    }

    private void HandleGuess(char letter)
    {
        if (guessed.IndexOf(letter) == -1)
        {
            guessed.Add(letter);
        }

        if (answer.IndexOf(letter) >= 0)
        {
            CalculateWord(answer, guessed);
            CheckGameWon();
        }
        else if (answer.IndexOf(letter) == -1)
        {
            error++;
            SetGameStatus();
            CheckIfGameLost();
        }
    }

    private void SetGameStatus()
    {
        GameStatus = $"Errors: {error} of {8}";
        SetCurrentImage();

    }

    private void SetCurrentImage()
    {
        CurrentImageName = $"hang{error}.png";
    }

    private void CheckIfGameLost()
    {
        if (error == 8)
        {
            Message = "You Dead...";

            DisableLetters();
        }
    }

    private void DisableLetters()
    {
        foreach (var item in letterContainer.Children)
        {
            var btn = item as Button;

            if (btn != null)
                btn.IsEnabled = false;
        }
    }

    private void EnableLetters()
    {
        foreach (var item in letterContainer.Children)
        {
            var btn = item as Button;

            if (btn != null)
                btn.IsEnabled = true;
        }
    }

    private void CheckGameWon()
    {
        if (answer == Spotlight.Replace(" ", ""))
        {
            Message = "You Won!";
            EnableLetters();
        }
    }

    private void Reset_Clicked(object sender, EventArgs e)
    {
        error = 0;
        Message = string.Empty;
        guessed = new();
        PickWord();
        CalculateWord(answer, guessed);
        SetGameStatus();
        EnableLetters();
    }
}