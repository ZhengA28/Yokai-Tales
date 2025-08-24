using Godot;
using System;

[GlobalClass]
public partial class Orbs : CharacterBody2D
{
	public enum TYPE { Blue, Green, White, Yellow, Red };
	[Export] public TYPE Color { get; set; }

	private bool matched = false;

	private AnimatedSprite2D animation;
	private AudioStreamPlayer popSFX;
	private Tween tween;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		popSFX = GetNode<AudioStreamPlayer>("PopSFX");
	}

	//Move orbs position with tween
	public void Swap(Vector2 position)
	{
		//GD.Print("X: " + position.X + ", Y: " + position.Y);
		tween = CreateTween();
		tween.TweenProperty(this, "position", position, 0.4)
			.SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
	}

	//Move orb position then back to original position
	//position1 orbs initial postion
	//position2 position to move orb to
	public void SwapBack(Vector2 position1, Vector2 position2)
	{
		tween = CreateTween();
		tween.TweenProperty(this, "position", position2, 0.4)
			.SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(this, "position", position1, 0.4)
			.SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
	}

	public void SpawnIn(Vector2 position)
	{
		tween = CreateTween();
		tween.TweenProperty(this, "position", position, 0.5 + Math.Abs(Position.Y) / 1000)
			.SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
	}

	public bool IsMatched()
	{
		return matched;
	}

	public void SetMatched()
	{
		matched = true;
	}


	//Play animation when orb is matched
	public void MatchAnimation()
	{

	}

	//Play sound when orb is removed
	public void PopSFX()
	{
		popSFX.Play();
	}

	//Remove orb from scene tree
	private void _On_Pop_Finished()
	{
		QueueFree();
	}
}
