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
    [Header("UI�o�[�̐ݒ�")]
    [SerializeField] private Image HPbar;
    public static float Health;
    [SerializeField] private Image Drinkbar;
    public static float Drink;
    [SerializeField] private SpriteRenderer OyajiHead;
    [SerializeField] private Sprite[] OyajiHeadSprite;
    private int OyajiHeadNumber;


[Header("�v���C���[�X�e�[�^�X�ƃ��[���h�ݒ�")]
    [SerializeField] private float PLHealth;
    [SerializeField] private float PLDrink;
    [SerializeField] private float DrinkDecrease;

    [Header("UI�I�u�W�F�N�g")]
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI ScoreText;


    [Header("���Ԑݒ�")]
    [SerializeField] private float NomalBattleTime;
    [SerializeField] private float FeverBattleTime;
    [SerializeField] private float FeverCutInTime;
    public static bool Feverbool;

    public static int[] getItems;

    [Header("�V�[���؂�ւ�")]
    [SerializeField] private SceneChanger sceneChanger;

    //�C���Q�[���J�n���̏����ݒ�
    void Start()
    {
        Timer = 0;
        Score = 0;
        Health = PLHealth;
        Drink = 0;
    }

    //�t�B�[�o�[
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
