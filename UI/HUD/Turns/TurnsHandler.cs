using Godot;
using System;

public partial class TurnsHandler : Control
{
	[Export] private int totalTurns;
	private int remainingTurns;
	private Label turnsLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		remainingTurns = totalTurns;
		turnsLabel = GetNode<Label>("Panel/Label");
		turnsLabel.Text = Convert.ToString(remainingTurns + " / " + totalTurns);
	}

	private void _Update_Turns()
	{
		remainingTurns--;
		turnsLabel.Text = Convert.ToString(remainingTurns + " / " + totalTurns);
	}
}
