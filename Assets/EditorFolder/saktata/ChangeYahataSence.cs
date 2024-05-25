using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ChangeYahataSence : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
    }
    public void  ChangeButton()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene("YahataSence");
        }
    }
}
