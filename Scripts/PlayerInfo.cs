using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerInfo : Node
{
    public static PlayerInfo Instance { get; set; }

    List<bool> unlockedLevels = [true, true, false, false, false, false, false, false];


    public override void _Ready()
    {
        Instance = this;
    }

    public void UpdateLevels(int levelID)
    {
        //unlockedLevels[level] = true;
    }

    public bool IsLevelUnlocked(int levelID)
    {
        return unlockedLevels[levelID];
    }
}
