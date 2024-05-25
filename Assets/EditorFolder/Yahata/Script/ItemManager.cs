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
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] itemSprite;

    [Header("�񕜃A�C�e���X�e�[�^�X")]
    [Header("0�͂܂݂̉� \n 1�͎��̉� \n 2�͎��̃_���[�W \n 3�͂����₫�̉� \n")]
    [SerializeField] private int[] _healAmount;
    [SerializeField] private int[] _getScore;
    [SerializeField] private float DestroyTime;
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


        spriteRenderer = GetComponent<SpriteRenderer>();

        if (itemKind == ItemKind.Sake)
        {
            spriteRenderer.sprite = itemSprite[0];
        }
        else if (itemKind == ItemKind.Tsumami)
        {
            spriteRenderer.sprite = itemSprite[1];
        } 
        else if(itemKind == ItemKind.Takoyaki)
        {
            spriteRenderer.sprite = itemSprite[2];
        }
        else if(itemKind == ItemKind.Ring)
        {
            spriteRenderer.sprite = itemSprite[3];
        }
        else if( itemKind == ItemKind.Figure)
        {
            spriteRenderer.sprite = itemSprite[4];
        }

        
        Invoke("Destroy", DestroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�A�C�e�����v���C���[��F��");

            if (itemKind == ItemKind.Tsumami)
            {
                GameManager.Health += _healAmount[0];
                Debug.Log("�܂݂��擾");
            }
            else if (itemKind == ItemKind.Sake)
            {
                Debug.Log("�����擾");

                int index = Random.Range(1, 101);
                if (index <= 80)
                {
                    GameManager.Health += _healAmount[1];
                }
                else
                {
                    GameManager.Health -= _healAmount[2];
                }

                GameManager.Drink += 50;
            }
            else if(itemKind == ItemKind.Takoyaki)
            {
                Debug.Log("�����₫���擾");
                GameManager.Health += _healAmount[3];
            }
            else if(itemKind == ItemKind.Ring)
            {
                Debug.Log("�w�ւ��擾");
                GameManager.Score += _getScore[0];

            }
            else if(itemKind == ItemKind.Figure)
            {
                Debug.Log("�t�B�M���A���擾");
                GameManager.Score += _getScore[1];
            }

            Destroy(gameObject);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }


    void Update()
    {
        
    }
}
