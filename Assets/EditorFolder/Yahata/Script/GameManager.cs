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

    [Header("�v���C���[")]
    [SerializeField] private GameObject Player;
    Animator animator;

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

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClip;

    private bool drunkEnd =false;
    private bool drunknRoll = false;

    //�C���Q�[���J�n���̏����ݒ�
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

    //�t�B�[�o�[
    private IEnumerator FeverTime()
    {
        if (!drunkEnd)
        {
            //�t�B�[�o�[���o����������܂Ŏ��Ԓ�~���ꎞ�폜��

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

        //���ł̎��S���o
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

    //���ł̎��S���o
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
