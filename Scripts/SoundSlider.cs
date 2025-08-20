using Godot;
using System;

public partial class SoundSlider : HSlider
{
	private int audioID;
	[Export] string audioName;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		audioID = AudioServer.GetBusIndex(audioName);
		_On_Slider_Change((float)Value);
	}

	public void _On_Slider_Change(float value)
	{
		//GD.Print(value);
		AudioServer.SetBusVolumeLinear(audioID, value);
	}
}
