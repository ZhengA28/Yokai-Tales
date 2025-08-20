using Godot;
using System;
using System.IO;

public partial class MainMenu : Control
{
	private AudioStreamPlayer buttonSFX;
	private Control settingMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buttonSFX = GetNode<AudioStreamPlayer>("VBoxContainer/ButtonSFX");
		settingMenu = GetNode<Control>("Settings_Menu");
	}

	private void _On_Start_Pressed()
	{
		//GD.Print("Start Game");
		buttonSFX.Play();
		GetTree().ChangeSceneToFile(Paths.Instance.mainMenuPaths[0]);
	}

	private void _On_Settings_Pressed()
	{
		buttonSFX.Play();
		settingMenu.Visible = true;
	}

	private void _On_Credit_Button_Pressed()
	{
		buttonSFX.Play();
		GetTree().ChangeSceneToFile(Paths.Instance.mainMenuPaths[1]);
	}

}
