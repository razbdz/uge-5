using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RPSSLGUI;

public partial class MainWindow : Window
{

    private enum Pick   { Rock, Paper, Scissors, Lizard, Spock }
    private enum Result { Draw, Player, Agent }

    private const int Goal = 3;
    private int _p = 0;          
    private int _a = 0;         
    private readonly Random _rand = new();


    private TextBlock? _txtRound, _txtChoices, _txtScore;
    private Button? _bRock, _bPaper, _bScissors, _bLizard, _bSpock, _bReset, _bClose;


    private static readonly HashSet<(Pick, Pick)> _wins = new()
    {
        (Pick.Rock,     Pick.Scissors), (Pick.Rock,   Pick.Lizard),
        (Pick.Paper,    Pick.Rock),     (Pick.Paper,  Pick.Spock),
        (Pick.Scissors, Pick.Paper),    (Pick.Scissors, Pick.Lizard),
        (Pick.Lizard,   Pick.Paper),    (Pick.Lizard, Pick.Spock),
        (Pick.Spock,    Pick.Rock),     (Pick.Spock,  Pick.Scissors)
    };

    public MainWindow()
    {
        InitializeComponent();


        _txtRound   = this.FindControl<TextBlock>("RoundText");
        _txtChoices = this.FindControl<TextBlock>("ChoicesText");
        _txtScore   = this.FindControl<TextBlock>("ScoreText");

        _bRock      = this.FindControl<Button>("BtnRock");
        _bPaper     = this.FindControl<Button>("BtnPaper");
        _bScissors  = this.FindControl<Button>("BtnScissors");
        _bLizard    = this.FindControl<Button>("BtnLizard");
        _bSpock     = this.FindControl<Button>("BtnSpock");
        _bReset     = this.FindControl<Button>("BtnReset");
        _bClose     = this.FindControl<Button>("BtnClose");

  
        foreach (var b in new[] { _bRock, _bPaper, _bScissors, _bLizard, _bSpock })
            if (b is not null) b.Click += OnPick;

        if (_bReset is not null) _bReset.Click += (_, __) => ResetGame();
        if (_bClose is not null) _bClose.Click += (_, __) => Close();

        Render(null, null, null);
    }

   
    private void OnPick(object? sender, RoutedEventArgs e)
    {
        if (_p >= Goal || _a >= Goal) return;

        var btn = (Button)sender!;
        var player = Parse(btn.Content?.ToString());
        var agent  = RandomPick();
        var res    = Decide(player, agent);

        if (res == Result.Player) _p++;
        else if (res == Result.Agent) _a++;

        Render(player, agent, res);

        if (_p >= Goal || _a >= Goal)
        {
            if (_txtRound != null)
                _txtRound.Text = _p > _a ? "SLUT — Du tog sejren!" : "SLUT — Computeren vandt.";
            ToggleChoice(false);
        }
    }

    private void ResetGame()
    {
        _p = _a = 0;
        ToggleChoice(true);
        Render(null, null, null);
    }


    private static Result Decide(Pick player, Pick agent)
    {
        if (player == agent) return Result.Draw;
        return _wins.Contains((player, agent)) ? Result.Player : Result.Agent;
    }

    private Pick RandomPick()
    {
        var values = (Pick[])Enum.GetValues(typeof(Pick));
        return values[_rand.Next(values.Length)];
    }

    private static Pick Parse(string? s)
    {
        var t = (s ?? string.Empty).Trim().ToLowerInvariant();
        return t switch
        {
            "rock" or "sten"      => Pick.Rock,
            "paper" or "papir"    => Pick.Paper,
            "scissors" or "saks"  => Pick.Scissors,
            "lizard" or "firben"  => Pick.Lizard,
            "spock"               => Pick.Spock,
            _ => Pick.Rock
        };
    }


    private void Render(Pick? player, Pick? agent, Result? res)
    {
        if (_txtChoices != null)
            _txtChoices.Text = $"Spiller: {player?.ToString() ?? "-"}   |   Computer: {agent?.ToString() ?? "-"}";

        if (_txtRound != null)
        {
            _txtRound.Text = res switch
            {
                null            => "Vælg en figur for at komme i gang",
                Result.Draw     => "Det blev uafgjort.",
                Result.Player   => "Point til dig!",
                Result.Agent    => "Point til computeren.",
                _               => string.Empty
            };
        }

        if (_txtScore != null)
            _txtScore.Text = $"Stilling — Spiller: {_p} | Computer: {_a}";
    }

    private void ToggleChoice(bool enabled)
    {
        foreach (var b in new[] { _bRock, _bPaper, _bScissors, _bLizard, _bSpock })
            if (b is not null) b.IsEnabled = enabled;
    }
}



