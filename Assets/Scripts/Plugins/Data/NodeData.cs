using UnityEngine;

public enum Character
{
    Anya,
    Andrey,
    Sonya,
    Mother,
    Father,
    Slave,
    Teacher,
    Chairperson,
    StoryTeller
}

[System.Serializable]
public class NodeData
{
    public CharacterInfo characterInfo;
    public string text;
    public Sprite sprite;
    public AudioClip music, fx;
}
