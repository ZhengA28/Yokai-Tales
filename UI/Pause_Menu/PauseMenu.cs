using Godot;
using System;

public partial class PauseMenu : Control
{
	private AudioStreamPlayer buttonSFX;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buttonSFX = GetNode<AudioStreamPlayer>("buttonSFX");
	}

	/*public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Menu"))
		{
			Visible = !Visible;
		}
	}*/

	private void _On_Resume()
	{
		//close menu and resume game
		buttonSFX.Play();
		GetTree().Paused = false;
		Hide();
	}

	private void _On_Quit()
	{
		//return to home screen
		buttonSFX.Play();
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile(Paths.Instance.mainMenu);
	}

}
