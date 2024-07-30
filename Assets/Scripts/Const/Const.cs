using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const
{
    public static readonly float ReloadingTimeAdded = 1f;
    public static readonly float maxZombieSpeed = 5f;
    public static readonly float minZombieSpeed = 3f;
}

public class Constants
{
    public const string JoinKey = "j";
    public const string DifficultyKey = "d";
    public const string GameTypeKey = "t";

    public static readonly List<string> GameTypes = new() { "Battle Royal", "Capture The Flag", "Creative" };
    public static readonly List<string> Difficulties = new() { "Easy", "Medium", "Hard" };
}
