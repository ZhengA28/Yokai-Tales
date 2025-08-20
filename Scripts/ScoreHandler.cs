using Godot;
using System;

public partial class ScoreHandler : Control
{
	private int score = 0;
	private Label scoreLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scoreLabel = GetNode<Label>("Panel/Label");
		scoreLabel.Text = Convert.ToString("000000");
	}

	private void _Update_Score(int point)
	{
		score += point;
		scoreLabel.Text = Convert.ToString(score);
	}
}
