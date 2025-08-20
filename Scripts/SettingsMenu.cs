using Godot;
using System;

public partial class SettingsMenu : Control
{
	private HSlider bgmSlide;
	private HSlider sfxSlide;
	private TextureButton bgmButton;
	private TextureButton sfxButton;
	private float bgmDB;
	private float sfxDB;

	private AudioStreamPlayer buttonSFX;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bgmSlide = GetNode<HSlider>("Background/BGMSlider");
		sfxSlide = GetNode<HSlider>("Background/SFXSlider");

		bgmButton = GetNode<TextureButton>("Background/BGMButton");
		sfxButton = GetNode<TextureButton>("Background/SFXButton");

		buttonSFX = GetNode<AudioStreamPlayer>("Background/Close/buttonSFX");
	}

	private void _On_Closed()
	{
		Visible = false;
		buttonSFX.Play();
	}

	private void _On_BGM()
	{

		if (bgmButton.ButtonPressed)
		{
			bgmDB = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Music"));   //Save current bgm volume
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Music"), 0);    //Mute bgm
			bgmSlide.Editable = false;	//Disable bgm slider
		}
		else
		{
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Music"), bgmDB);    //Turn bgm back on
			bgmSlide.Editable = true;	//Enable slider
		}
	}

	private void _On_SFX()
	{

		if (sfxButton.ButtonPressed)
		{
			sfxDB = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("SFX")); //Save current sfx volume
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("SFX"), 0);  //Mute sfx
			sfxSlide.Editable = false;	//Disable sfx slider
		}
		else
		{
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("SFX"), sfxDB);  //Turn sfx back on
			sfxSlide.Editable = true;	//Enable slider
		}
	}
}
