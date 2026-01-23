using MemoryGame.Models;
using Timer = System.Timers.Timer;

namespace MemoryGame.Services;

public class GameService
{
    private const int CardTimerDuration = 2_000; // Milliseconds
    private const int GameTimerTickInterval = 1_000; // Milliseconds

    private readonly CardFactory _cardFactory;

    private readonly Timer _cardTimer = new(CardTimerDuration);
    private readonly Timer _gameTimer = new(GameTimerTickInterval);

    private readonly List<Card> _selectedCards = [];

    private bool _isGameOver;
    private bool _isGameStarted;

    public Action? OnStateChanged;

    public GameService(CardFactory cardFactory)
    {
        _cardFactory = cardFactory;
        _cardTimer.AutoReset = false;
        _cardTimer.Elapsed += (_, _) => ResetSelectedCards();
        _gameTimer.Elapsed += (_, _) =>
        {
            ElapsedTime += TimeSpan.FromMilliseconds(GameTimerTickInterval);
            OnStateChanged?.Invoke();
        };

        InitialisePlayField();
    }

    public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;
    public List<Card> PlayingCards { get; } = [];
    public Difficulty SelectedDifficulty { get; private set; } = Difficulty.Medium;

    public void SetDifficulty(Difficulty difficulty)
    {
        if (_isGameStarted || difficulty == SelectedDifficulty) return;

        ElapsedTime = TimeSpan.Zero;
        SelectedDifficulty = difficulty;

        InitialisePlayField();
    }

    public void HandleCardClick(Card card)
    {
        if (ShouldIgnoreClick(card))
            return;

        card.IsVisible = true;
        ProcessCardSelection(card);
    }

    private void ProcessCardSelection(Card card)
    {
        var selectionCount = _selectedCards.Count;

        switch (selectionCount)
        {
            case 0:
                AddFirstSelection(card);
                break;
            case 1:
                HandleSecondCardSelection(card);
                break;
            case 2:
                HandleThirdCardSelection(card);
                break;
        }
    }

    private void AddFirstSelection(Card card) => _selectedCards.Add(card);

    private void HandleSecondCardSelection(Card card)
    {
        _cardTimer.Start();
        _selectedCards.Add(card);

        if (!IsSecondSelectionMatch(card)) return;

        CompleteMatch(card);

        if (PlayingCards.All(c => c.IsMatched))
        {
            StopGame();
        }
    }

    private void HandleThirdCardSelection(Card card)
    {
        if (IsDuplicateSelection(card))
        {
            // No action - the player clicked the same card twice
            return;
        }

        ResetAfterMismatch(card);
    }

    private void CompleteMatch(Card card)
    {
        _selectedCards[0].IsMatched = true;
        card.IsMatched = true;

        _cardTimer.Stop();
        _selectedCards.Clear();
    }

    private bool ShouldIgnoreClick(Card card) => card.IsMatched || card.IsVisible || _isGameOver;

    private bool IsDuplicateSelection(Card card) => _selectedCards.Any(c => c.Id == card.Id);

    private bool IsSecondSelectionMatch(Card card)
    {
        var firstCard = _selectedCards[0];
        return firstCard.Id != card.Id && firstCard.Image.Equals(card.Image);
    }

    private void ResetAfterMismatch(Card card)
    {
        _cardTimer.Stop();

        ResetSelectedCards();

        _selectedCards.Add(card);
    }

    public void StartGame()
    {
        _isGameOver = false;
        _isGameStarted = true;

        _gameTimer.Start();

        OnStateChanged?.Invoke();
    }

    public void StopGame()
    {
        _isGameOver = true;
        _isGameStarted = false;

        _cardTimer.Stop();
        _gameTimer.Stop();

        _selectedCards.Clear();

        OnStateChanged?.Invoke();
    }

    private void InitialisePlayField()
    {
        var cardAmount = GetCardAmountForDifficulty();
        var cards = _cardFactory.Create(cardAmount).Shuffle();

        PlayingCards.Clear();
        PlayingCards.AddRange(cards);
    }

    private int GetCardAmountForDifficulty() => SelectedDifficulty switch
    {
        Difficulty.Easy => 8,
        Difficulty.Medium => 12,
        Difficulty.Hard => 16,
        _ => throw new ArgumentOutOfRangeException(nameof(SelectedDifficulty), SelectedDifficulty, null)
    };

    private void ResetSelectedCards()
    {
        _selectedCards.ForEach(card => card.IsVisible = false);
        _selectedCards.Clear();
        OnStateChanged?.Invoke();
    }
}