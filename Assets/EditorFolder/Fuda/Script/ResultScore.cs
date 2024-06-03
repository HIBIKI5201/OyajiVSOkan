using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class ResultScore : MonoBehaviour
{
    [SerializeField] private bool isTimerText;
    [SerializeField] private bool isRanking;
    private bool isRanked;
    private int rankedNum;
    private float rankedBlinkTimer;
    [SerializeField] private float blinkingTime= 0.3f;
    private static int[] ResultScores = new int[5];
    void Start()
    {
        if (isRanking)
        {
            for(int i=0; i < ResultScores.Length; i++)
            {
                if (ResultScores[i] < GameManager.Score)
                {
                    if (i != ResultScores.Length)
                    {
                        for (int j = i; j < ResultScores.Length-1; j++)
                        {
                            transform.GetChild(j+1).GetComponent<TextMeshProUGUI>().text = ResultScores[j].ToString("000000000");
                        }
                    }
                    ResultScores[i] = GameManager.Score;
                    transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = ResultScores[i].ToString("000000000");
                    rankedNum = i;
                    isRanked = true;
                    break;
                }
                if (ResultScores[i] > 0)
                {
                    transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = ResultScores[i].ToString("000000000");
                }
            }
        }
        else if(isTimerText)
        {
            int TimerMinute = (int)Mathf.Floor(GameManager.Timer / 60);
            int TimerSecond = (int)Mathf.Floor(GameManager.Timer % 60);
            
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TimerMinute.ToString("D2") + ":" + TimerSecond.ToString("D2");
        }
        else
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManager.Score.ToString("000000000");
        }
        
    }
    private void Update()
    {
        if (isRanked)
        {
            rankedBlinkTimer += Time.deltaTime;
            if (rankedBlinkTimer > blinkingTime)
            {
                transform.GetChild(rankedNum).GetComponent<TextMeshProUGUI>().color = Color.grey;
            }
            if (rankedBlinkTimer > blinkingTime * 2.0f)
            {
                transform.GetChild(rankedNum).GetComponent<TextMeshProUGUI>().color = Color.red;
                rankedBlinkTimer = 0.0f;
            }
            
        }
    }
}
