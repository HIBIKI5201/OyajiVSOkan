using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyManager;

public class ItemManager : MonoBehaviour
{
    public ItemKind itemKind;
    public enum ItemKind
    {
        Sake,
        Tsumami,
        Takoyaki,
        Ring,
        Figure
    }

    public bool Treasure;

    [Header("�X�v���C�g�̎��")]
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite[] itemSprite;

    void Start()
    {
        if (Treasure)
        {
            System.Array values = System.Enum.GetValues(typeof(ItemKind));
            int randomIndex = Random.Range(3, 5);
            itemKind = (ItemKind)values.GetValue(randomIndex);
        } else
        {
            System.Array values = System.Enum.GetValues(typeof(ItemKind));
            int randomIndex = Random.Range(0, 3);
            itemKind = (ItemKind)values.GetValue(randomIndex);
        }




        if (itemKind == ItemKind.Sake)
        {
            renderer.sprite = itemSprite[0];
        }
        else if (itemKind == ItemKind.Tsumami)
        {
            renderer.sprite = itemSprite[1];
        } 
        else if(itemKind == ItemKind.Takoyaki)
        {
            renderer.sprite = itemSprite[2];
        }
        else if(itemKind == ItemKind.Ring)
        {
            renderer.sprite = itemSprite[3];
        }
        else if( itemKind == ItemKind.Figure)
        {
            renderer.sprite = itemSprite[4];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�A�C�e�����v���C���[��F��");

            if (itemKind == ItemKind.Tsumami)
            {
                Debug.Log("�܂݂��擾");
            }
            else if (itemKind == ItemKind.Sake)
            {
                Debug.Log("�����擾");

                int index = Random.Range(1, 101);
                if (index <= 80)
                {
                    GameManager.Health += 20;
                }
                else
                {
                    GameManager.Health -= 10;
                }

                GameManager.Drink += 50;
            }
            else if(itemKind == ItemKind.Takoyaki)
            {
                Debug.Log("�����₫���擾");
                GameManager.Health += 5;
            }
            else if(itemKind == ItemKind.Ring)
            {
                Debug.Log("�w�ւ��擾");
                GameManager.Score += 2000;
            }
            else if(itemKind == ItemKind.Figure)
            {
                Debug.Log("�t�B�M���A���擾");
                GameManager.Score += 500;
            }

            Destroy(gameObject);
        }
    }


    void Update()
    {
        
    }
}
