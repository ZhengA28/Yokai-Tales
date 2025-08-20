using Godot;
using System;

public partial class TurnsHandler : Control
{
	[Export] private int turns;
	private Label turnsLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		turnsLabel = GetNode<Label>("Panel/Label");
		turnsLabel.Text = Convert.ToString(turns);
	}

	private void _Update_Turns()
	{
		turns--;
		turnsLabel.Text = Convert.ToString(turns);
	}
}
