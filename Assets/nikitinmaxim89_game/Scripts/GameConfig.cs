using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Create new Game Config")]
public class GameConfig : ScriptableObject
{
    [Space(10)]
    public GameObject coinPrefab;

    [Space(10)]
    public float gameDuration;
    public float interval;

    [Space(10)]
    public float appearanceTime;
    public float disappearanceTime;

    [Space(10)]
    public float delayTime;
    public int moleCount;

    [Space(10)]
    public float deltaDifficulty;
    public float maxDifficulty;
}
