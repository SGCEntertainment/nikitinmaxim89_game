using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    Mole[] topMoles;
    Mole[] bottomMoles;

    Transform topPlayer;
    Transform bottomPlayer;

    IEnumerator gameProcessTop;
    IEnumerator gameProcessBottom;

    public GameConfig gameConfig;

    private void Start()
    {
        CacheComponents();

        StartGame();
    }

    void CacheComponents()
    {
        topPlayer = GameObject.Find("top_player").transform;
        bottomPlayer = GameObject.Find("bottom_player").transform;

        topMoles = new Mole[topPlayer.childCount];
        for (int i = 0; i < topMoles.Length; i++)
        {
            topMoles[i] = topPlayer.GetChild(i).GetComponentInChildren<Mole>();
        }

        bottomMoles = new Mole[bottomPlayer.childCount];
        for (int i = 0; i < bottomMoles.Length; i++)
        {
            bottomMoles[i] = bottomPlayer.GetChild(i).GetComponentInChildren<Mole>();
        }
    }

    void HildeAllMoles()
    {
        foreach(Mole t in topMoles)
        {
            t.transform.localScale = Vector3.zero;
        }

        foreach (Mole t in bottomMoles)
        {
            t.transform.localScale = Vector3.zero;
        }
    }

    public void StartGame()
    {
        HildeAllMoles();

        gameProcessTop = GameProcess(topMoles);
        gameProcessBottom = GameProcess(bottomMoles);

        StartCoroutine(gameProcessTop);
        StartCoroutine(gameProcessBottom);
    }

    IEnumerator GameProcess(Mole[] playerMoles)
    {
        while (true)
        {
            int count = gameConfig.maxMoleCount;
            List<Mole> molesInGame = new List<Mole>();

            while (molesInGame.Count != count)
            {
                Mole tmp = playerMoles[UnityEngine.Random.Range(0, playerMoles.Length)];
                if (molesInGame.Contains(tmp))
                {
                    continue;
                }

                molesInGame.Add(tmp);
                yield return null;
            }

            for (int i = 0; i < count; i++)
            {
                StartCoroutine(molesInGame[i].Show());
                yield return null;
            }

            yield return new WaitForSeconds(gameConfig.delayTime + UnityEngine.Random.Range(1.0f, 2.0f));
        }
    }
}
