using Godot;
using System.Text.Json;
using System.Collections.Generic;

public partial class DialogueBox : Control
{
	public class DialogueInfo	//Class for Json objects key-value pairs
	{
		public string Speaker { get; set;}
		public string Sprite { get; set;}
		public string Position { get; set;}
		public string Dialogue { get; set;}
	}

	List<DialogueInfo> dialogue;	//List of Json objects

	private int index = 0;
	private RichTextLabel dialogueText;	//text for dialogue box
	private Label speaker;	//character that is speaking
	private AudioStreamPlayer audio;
	private Polygon2D indicator;
	private Tween tween;
	private bool isFinished = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dialogueText = GetNode<RichTextLabel>("DialogueBox/Content");
		indicator = GetNode<Polygon2D>("Polygon2D");
		audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		speaker = GetNode<Label>("Speaker/Name");

		FileAccess fileAccess = FileAccess.Open(Paths.Instance.dialogue1_1, FileAccess.ModeFlags.Read);
		string file = fileAccess.GetAsText();
		
		dialogue = JsonSerializer.Deserialize<List<DialogueInfo>>(file);
		GD.Print(dialogue.Count);
		LoadDialogue();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			switch (mouseEvent.ButtonIndex)
			{
				case MouseButton.Right:
					if (tween.IsRunning()){
						FinishDialogue();
					}
					else{
						LoadDialogue();
					}
					break;
				case MouseButton.Left:
					if (tween.IsRunning()){
						FinishDialogue();
					}
					else{
						LoadDialogue();
					}
					break;
			}

			//GD.Print(@event.AsText());
		}

	}

	//Display dialogue using tween animation
	public void LoadDialogue()
	{
		if (index < dialogue.Count){
			Visible = true;
			tween = CreateTween();
			speaker.Text = dialogue[index].Speaker;
			dialogueText.Text = dialogue[index].Dialogue;

			dialogueText.VisibleRatio = 0;
			tween.TweenProperty(dialogueText, "visible_ratio", 1, 0.03 * dialogueText.Text.Length).
				SetTrans(Tween.TransitionType.Linear).
				SetEase(Tween.EaseType.InOut);
			tween.Play();
			audio.Play();
			index++;
		}
		else{
			//QueueFree();
			index = 0;
			Visible = false;
		}
	}

	//Stop tween animation and reveal rest of dialogue
	private void FinishDialogue()
	{
		tween.Stop();
		dialogueText.VisibleRatio = 100;
	}
}
