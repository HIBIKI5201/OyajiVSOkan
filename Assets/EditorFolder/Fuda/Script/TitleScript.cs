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

    public AudioClip _selectSE;
    public AudioClip _enterSE;
    public AudioClip _missSE;
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
            audioSource.PlayOneShot(_selectSE);
            if (_menuCount == _menu.Length)
            {
                _menuCount = 0;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _menuCount--;
            audioSource.PlayOneShot(_selectSE);
            if (_menuCount == -1)
            {
                _menuCount = _menu.Length-1;
            }

        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) )
        {
            if(_menuCount == 0)
            {
                audioSource.PlayOneShot(_enterSE);
                SceneManager.LoadScene("InGame");
            }
            else 
            {
                audioSource.PlayOneShot(_missSE);
            }

        }

        transform.position = _menu[_menuCount].transform.position;
    }
}
