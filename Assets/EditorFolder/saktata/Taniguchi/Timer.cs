using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer: MonoBehaviour
{
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //タイマーを更新
        elapsedTime += Time.deltaTime;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    
}
