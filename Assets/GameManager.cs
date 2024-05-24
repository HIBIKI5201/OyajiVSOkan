using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static float Timer;
    public static int Score;
    public static int Health;
    [SerializeField] private int playerHealth;

    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    void Start()
    {
        Timer = 0;
        Score = 0;
        Health = playerHealth;
    }

    void Update()
    {
        Timer += Time.deltaTime;

        float TimerMinute = Mathf.Floor(Timer/60);
        float TimerSecond = Mathf.Floor(Timer%60);
        TimerText.text = TimerMinute + ":" + TimerSecond;

        ScoreText.text = "Score\n" + Score;
    }
}
