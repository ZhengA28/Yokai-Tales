using Godot;
using System;

public partial class StoryMode : Control
{
	// Called when the node enters the scene tree for the first time.
	Control pauseMenu;
	public override void _Ready()
	{
		pauseMenu = GetNode<Control>("PauseMenu");
	}

	private void _On_Menu()
	{
		GetTree().Paused = true;
		pauseMenu.Show();
	}
}
