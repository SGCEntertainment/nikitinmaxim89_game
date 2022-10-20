using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Create new Game Config")]
public class GameConfig : ScriptableObject
{
    [Space(10)]
    public float gameDuration;

    [Space(10)]
    public float appearanceTime;
    public float disappearanceTime;

    [Space(10)]
    public float delayTime;
    public int maxMoleCount;
    public float maxDifficulty;
}
