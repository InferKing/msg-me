using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    private static readonly Color _color;
    public static readonly char[] punctutation = {'.', ',', '!', '?', '-', ':'};
    public static readonly string Output = "output";
    public static readonly string Input = "input";
    public static readonly float delayPunctuation = 0.06f;
    public static readonly float delayCharacter = 0.025f;
    public static readonly Dictionary<Character, Color> characterColors = new()
    {
        { Character.Anya, ColorUtility.TryParseHtmlString("#F2FF66", out _color) ? _color : Color.white },
        { Character.Andrey, ColorUtility.TryParseHtmlString("#486DCC", out _color) ? _color : Color.white },
        { Character.Sonya, ColorUtility.TryParseHtmlString("#FA7577", out _color) ? _color : Color.white },
        { Character.Mother, ColorUtility.TryParseHtmlString("#B9EEE2", out _color) ? _color : Color.white },
        { Character.Father, ColorUtility.TryParseHtmlString("#A49474", out _color) ? _color : Color.white },
        { Character.Slave, ColorUtility.TryParseHtmlString("#914949", out _color) ? _color : Color.white },
        { Character.Teacher, ColorUtility.TryParseHtmlString("#9AB0DA", out _color) ? _color : Color.white },
        { Character.Chairperson, ColorUtility.TryParseHtmlString("#E2E2E2", out _color) ? _color : Color.white },
        { Character.StoryTeller, ColorUtility.TryParseHtmlString("#E2E2E2", out _color) ? _color : Color.white }
    };
}
