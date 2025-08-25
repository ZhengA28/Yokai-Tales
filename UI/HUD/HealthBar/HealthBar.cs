using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	private ProgressBar damageBar;
	private Timer timer;
	[Export] int health = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		damageBar = GetNode<ProgressBar>("DamageBar");
		timer = GetNode<Timer>("Timer");

		Value = health; //Current HP
		MaxValue = health;  //Max HP
		damageBar.Value = health;
		damageBar.MaxValue = health;
	}

	public void UpdateHealth(int damage)
	{
		int prevHealth = health;
		//health = Math.Min(MaxValue, damage);
		health = health - damage;
		Value = health;

		if (health < prevHealth)
		{
			timer.Start();
		}
	}

	public void _On_Timeout()
	{
		damageBar.Value = health;
	}
}
