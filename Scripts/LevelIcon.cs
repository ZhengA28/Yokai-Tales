using Godot;
using System;

public partial class LevelIcon : Control
{
	[Export] int levelID;
	TextureButton level;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		level = GetNode<TextureButton>("LevelButton");

		if (!PlayerInfo.Instance.IsLevelUnlocked(levelID))	//Check if player has unlocked level
		{
			level.Disabled = true;
		}
	}

	public void _On_Level_Pressed()
	{
		GetTree().ChangeSceneToFile(Paths.Instance.mapAreaPaths[levelID]);
	}
}
