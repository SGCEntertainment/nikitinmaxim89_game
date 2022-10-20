using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

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

    bool gameStarted;

    float currentDifficulty;
    int currentMoleCount;

    [HideInInspector]
    public float currentAppearanceTime;

    [HideInInspector]
    public float currentDisappearanceTime;

    [HideInInspector]
    public float currentDelayTime;


    Mole[] topMoles;
    Mole[] bottomMoles;

    Transform topPlayer;
    Transform bottomPlayer;

    IEnumerator gameProcessTop;
    IEnumerator gameProcessBottom;

    float currentTimerCount;
    Text timerText;

    int topScoreCount;
    Text topScoreCountText;

    int bottomScoreCount;
    Text bottomScoreCountText;


    public GameConfig gameConfig;

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if(!gameStarted)
        {
            return;
        }

        UpdateTimer();
    }

    void UpdateTimer()
    {
        currentTimerCount -= Time.deltaTime;

        float min = Mathf.FloorToInt(currentTimerCount / 60);
        float sec = Mathf.FloorToInt(currentTimerCount % 60);

        if (currentTimerCount <= 0)
        {
            currentTimerCount = 0;
            timerText.text = string.Format("{0:00}:{0:00}", 0, 0);

            StopCoroutine(gameProcessTop);
            StopCoroutine(gameProcessBottom);

            CancelInvoke(nameof(UpdateDifficult));

            gameStarted = false;
            return;
        }

        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    void CacheComponents()
    {
        timerText = GameObject.Find("timerText").GetComponent<Text>();

        topScoreCountText = GameObject.Find("top score").GetComponent<Text>();
        bottomScoreCountText = GameObject.Find("bottom score").GetComponent<Text>();

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

    void UpdateDifficult()
    {
        currentDifficulty += gameConfig.deltaDifficulty;
    }

    public void UpdatePlayerScore()
    {
        int playerId = Camera.main.ScreenToWorldPoint(Input.mousePosition).y > 0 ? 0 : 1;

        if(playerId == 0)
        {
            topScoreCountText.text = (++topScoreCount).ToString();
        }
        else
        {
            bottomScoreCountText.text = (++bottomScoreCount).ToString();
        }
    }

    public void StartGame()
    {
        topScoreCount = 0;
        bottomScoreCount = 0;

        currentTimerCount = gameConfig.gameDuration;

        currentAppearanceTime = gameConfig.appearanceTime;
        currentDisappearanceTime = gameConfig.disappearanceTime;

        currentDelayTime = gameConfig.delayTime;
        currentMoleCount = gameConfig.moleCount;

        HildeAllMoles();

        gameProcessTop = GameProcess(topMoles);
        gameProcessBottom = GameProcess(bottomMoles);

        StartCoroutine(gameProcessTop);
        StartCoroutine(gameProcessBottom);

        InvokeRepeating(nameof(UpdateDifficult), 0.0f, gameConfig.interval);

        gameStarted = true;
    }

    IEnumerator GameProcess(Mole[] playerMoles)
    {
        while (true)
        {
            int count = currentMoleCount;
            List<Mole> molesInGame = new List<Mole>();

            while (molesInGame.Count != count)
            {
                Mole tmp = playerMoles[Random.Range(0, playerMoles.Length)];
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

            float returnAfterSeconds = currentDelayTime + currentAppearanceTime + Random.Range(1.0f, 2.0f);
            yield return new WaitForSeconds(returnAfterSeconds);
        }
    }
}
