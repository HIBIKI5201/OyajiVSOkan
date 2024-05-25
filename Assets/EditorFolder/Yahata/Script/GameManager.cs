using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static float Timer;
    public static int Score;
    [Header("UIバーの設定")]
    [SerializeField] private Image HPbar;
    public static float Health;
    [SerializeField] private Image Drinkbar;
    public static float Drink;
    [SerializeField] private SpriteRenderer OyajiHead;
    [SerializeField] private Sprite[] OyajiHeadSprite;
    private int OyajiHeadNumber;


[Header("プレイヤーステータスとワールド設定")]
    [SerializeField] private float PLHealth;
    [SerializeField] private float PLDrink;
    [SerializeField] private float DrinkDecrease;

    [Header("UIオブジェクト")]
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI ScoreText;


    [Header("時間設定")]
    [SerializeField] private float NomalBattleTime;
    [SerializeField] private float FeverBattleTime;
    [SerializeField] private float FeverCutInTime;
    public static bool Feverbool;

    public static int[] getItems;

    [Header("シーン切り替え")]
    [SerializeField] private SceneChanger sceneChanger;

    //インゲーム開始時の初期設定
    void Start()
    {
        Timer = 0;
        Score = 0;
        Health = PLHealth;
        Drink = 0;
    }

    //フィーバー
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
            sceneChanger.SwitchScene("Result");
        }
        
        if(Drink > 0)
        {
            Drink -= DrinkDecrease * Time.deltaTime;
        }
        Drinkbar.fillAmount = Drink / PLDrink;

        if(Health / PLHealth >= 0.75)
        {
            OyajiHeadNumber = 2;
        }
        else if (Health / PLHealth >= 0.25)
        {
            OyajiHeadNumber = 1;
        }
        else
        {
            OyajiHeadNumber = 0;
        }
        
        if(Drink / PLDrink >= 0.01)
        {
            OyajiHeadNumber += 3;
        }

        Debug.Log(Health / PLHealth);

        OyajiHead.sprite = OyajiHeadSprite[OyajiHeadNumber];
    }
}
