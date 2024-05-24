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

    [SerializeField] private float NomalBattleTime;
    [SerializeField] private float FeverBattleTime;
    [SerializeField] private float FeverCutInTime;
    public static bool Feverbool;
    void Start()
    {
        Timer = 0;
        Score = 0;
        Health = PLHealth;
        Drink = 0;
    }

    private IEnumerator FeverTime()
    {
        Time.timeScale = 0;
        
        yield return new WaitForSecondsRealtime(FeverCutInTime);
        
        Time.timeScale = 1;

        Debug.Log("FeverTimeStart");
        yield return new WaitForSeconds(FeverBattleTime);

        Debug.Log("FeverTimeEnd");
        Feverbool = false;
    }

    void Update()
    {
        Timer += Time.deltaTime;

        int TimerMinute = (int)Mathf.Floor(Timer/60);
        int TimerSecond = (int)Mathf.Floor(Timer%60);
        TimerText.text = TimerMinute.ToString("D2") + ":" + TimerSecond.ToString("D2");

        if((Timer % (NomalBattleTime + FeverBattleTime)) >= NomalBattleTime && !Feverbool)
        {
            Feverbool = true;
            Debug.Log("FeverTime");
            StartCoroutine(FeverTime());
        }

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
