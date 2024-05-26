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

    [Header("プレイヤー")]
    [SerializeField] private GameObject Player;
    Animator animator;

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

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClip;

    private bool drunkEnd =false;
    private bool drunknRoll = false;

    //インゲーム開始時の初期設定
    void Start()
    {
        Timer = 0;
        Score = 0;
        Health = PLHealth;
        Drink = 0;

        Feverbool = false;

        audioSource.PlayOneShot(audioClip[0]);
        animator = Player.GetComponent<Animator>();
    }

    //フィーバー
    private IEnumerator FeverTime()
    {
        if (!drunkEnd)
        {
            //フィーバー演出が完成するまで時間停止を一時削除中

            //Time.timeScale = 0;

            //yield return new WaitForSecondsRealtime(FeverCutInTime);

            Time.timeScale = 1;

            audioSource.Stop();
            audioSource.PlayOneShot(audioClip[1]);

            Debug.Log("FeverTimeStart");
            yield return new WaitForSeconds(FeverBattleTime);
            if (!drunkEnd)
            {
                Debug.Log("FeverTimeEnd");
                Feverbool = false;


                audioSource.Stop();
                audioSource.PlayOneShot(audioClip[0]);
            }
        }
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
        if(Health >= PLHealth) 
        {
            Health = PLHealth;
        }

        Debug.Log(Health);

        if (Health <= 0)
        {
            Debug.Log("GameOver");
            sceneChanger.SwitchScene("Result");
        }

        //酒での死亡演出
        if (Drink >= PLDrink&& !drunkEnd)
        {
            StartCoroutine(DrunkEnd());
            drunkEnd=true;
        }
        if (drunknRoll)
        {
            DrunkdPlayerRoll();
        }

        if (Drink > 0)
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

    //酒での死亡演出
    private IEnumerator DrunkEnd()
    {
        Player.GetComponent<PlayerController>().enabled = false;
        Player.GetComponent<CapsuleCollider2D>().enabled = false;

        animator.SetBool("drunkn", true);

        Player.transform.position += new Vector3(0, 4.0f, 0);
        Player.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        audioSource.Stop();
        audioSource.PlayOneShot(audioClip[2]);

        yield return new WaitForSeconds(1.0f);

        drunknRoll = true;

        audioSource.Stop();
        audioSource.PlayOneShot(audioClip[3]);

        yield return new WaitForSeconds(8.0f);

        Debug.Log("GameOver");
        sceneChanger.SwitchScene("Result");
    }

    private void DrunkdPlayerRoll()
    {
        Player.transform.GetChild(7).rotation *= new Quaternion(0, 0, 0.02f, 0.98f);
        Player.transform.position += new Vector3(0, -0.008f,0);
    }
}
