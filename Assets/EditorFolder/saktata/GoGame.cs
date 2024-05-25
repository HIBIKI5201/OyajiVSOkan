using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoGame : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.Select();
    }

    // Update is called once per frame
    public void ChangeButton()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene("YahataSence");
        }
    }
}
