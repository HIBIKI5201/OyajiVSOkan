using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static float Timer;
    public static int Score;
    [SerializeField] private Image HPbar;
    public static float Health;
    [SerializeField] private Image Drinkbar;
    public static float Drink;

    [SerializeField] private float PLHealth;
    [SerializeField] private float PLDrink;
    [SerializeField] private float DrinkDecrease;

    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    void Start()
    {
        Timer = 0;
        Score = 0;
        Health = PLHealth;
        Drink = 0;
    }

    void Update()
    {
        Timer += Time.deltaTime;

        int TimerMinute = (int)Mathf.Floor(Timer/60);
        int TimerSecond = (int)Mathf.Floor(Timer%60);
        TimerText.text = TimerMinute.ToString("D2") + ":" + TimerSecond.ToString("D2");

        ScoreText.text = "Score\n" + Score.ToString("D8");

        HPbar.fillAmount = Health / PLHealth;
        if (Health <= 0)
        {
            Debug.Log("GameOver");
        }
        
        if(Drink > 0)
        {
            Drink -= DrinkDecrease * Time.deltaTime;
        }
        Drinkbar.fillAmount = Drink / PLDrink;
    }
}
