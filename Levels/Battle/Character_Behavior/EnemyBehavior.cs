using Godot;
using System;

public partial class EnemyBehavior : Control
{
	//private HealthBar healthBar = new HealthBar();
	private HealthBar healthBar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		healthBar = (HealthBar)GetNode<ProgressBar>("HealthBar");
		
	}

	public void _Update_Health(int damage)
	{
		healthBar.UpdateHealth(damage);
	}
}
