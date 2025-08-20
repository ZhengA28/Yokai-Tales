using Godot;
using System;
using System.Collections.Generic;

public partial class Paths : Node
{
    public static Paths Instance { get; set; }  //Initialize to allow autoload in Godot
    public string mainMenu = "res://Scenes/Levels/Main_Menu.tscn";

    public List<String> mainMenuPaths = [
        "res://Scenes/Levels/level_map.tscn",   //0 overworld map
        "res//Scenes/UI/Settings.tscn",         //1 settings
                                                //2 credits
    ];

    public List<string> mapAreaPaths = ["res://Scenes/UI/Level.tscn","res://Scenes/Levels/Introduction.tscn"];

    public string dialogue1_1 = "res://Dialogue/Chapter1.json";

    public override void _Ready()
    {
        Instance = this;
    }
}
