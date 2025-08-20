using Godot;
using System;

public partial class MatchLevel : Control
{
	// Called when the node enters the scene tree for the first time.

	Control pauseMenu;
	public override void _Ready()
	{
		pauseMenu = GetNode<Control>("PauseMenu");
		//grid = GetNode<Node2D>("Grid");

		//Vector2 resolution = GetViewportRect().Size;
		//grid.Position = resolution / 2;
	}

	private void _On_Menu()
	{
		GetTree().Paused = true;
		pauseMenu.Show();
	}

	
}
