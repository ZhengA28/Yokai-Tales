using Godot;
using System;
using System.Collections.Generic;

public partial class Paths : Node
{
    public static Paths Instance { get; set; }  //Initialize to allow autoload in Godot
    public string mainMenu = "res://UI/Title_Screen/Main_Menu.tscn";
    public string previousScene;    //Store the players previous scene

    public List<String> mainMenuPaths = [
        "res://Maps/Over_World_Map/Over_World.tscn",   //0 overworld map
        "res//Scenes/UI/Settings.tscn",         //1 settings
                                                //2 credits
    ];

    public List<string> mapAreaPaths = ["res://Levels/Battle/Level.tscn","res://Levels/Cutscenes/Introduction.tscn"];

    public string dialogue1_1 = "res://Levels/Cutscenes/Dialogue_Resource/Chapter1.json";

    public override void _Ready()
    {
        Instance = this;
    }
}
