using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    [SerializeField] GameObject[] _menu;
    int _menuCount;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _menuCount++;

            if (_menuCount == _menu.Length)
            {
                _menuCount = 0;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _menuCount--;

            if (_menuCount == -1)
            {
                _menuCount = _menu.Length-1;
            }

        }
        else if (Input.GetKeyDown(KeyCode.Return) )
        {
            if(_menuCount == 0)
            {

                SceneManager.LoadScene("YahataSence");
            }
            else if (_menuCount == 1)
            {

                SceneManager.LoadScene("Help");
            }
            else 
            {
 
            }

        }

        transform.position = _menu[_menuCount].transform.position;
    }
}
