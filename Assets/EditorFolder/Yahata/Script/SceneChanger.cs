using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SwitchScene(string Name)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(Name);
    }
}
