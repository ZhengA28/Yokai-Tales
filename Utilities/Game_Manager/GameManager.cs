using Godot;
using System;

public partial class GameManager : Control
{
	//[Signal] public delegate void UpdateScoreEventHandler(int points);
	[Signal] public delegate void UpdateTurnsEventHandler();
	[Signal] public delegate void UpdateEnemyTurnsEventHandler();
	[Signal] public delegate void UpdateEnemyHPEventHandler();
	[Signal] public delegate void UpdateHPEventHandler();
	[Signal] public delegate void GameWinEventHandler();
	[Signal] public delegate void GameLoseEventHandler();
	[Export] int turns;
	[Export] int enemyHP;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _Process(double delta)
    {
        
    }

}
