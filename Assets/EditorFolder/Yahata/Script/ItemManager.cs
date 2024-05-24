using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemKind itemKind;
    public enum ItemKind
    {
        Sake,
        Tsumami,
    }

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite[] itemSprite;

    void Start()
    {
        if(itemKind == ItemKind.Sake)
        {
            renderer.sprite = itemSprite[0];
        }
        else if (itemKind == ItemKind.Tsumami)
        {
            renderer.sprite = itemSprite[1];
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�A�C�e�����v���C���[��F��");

            if (itemKind == ItemKind.Tsumami)
            {
                Debug.Log("�񕜃A�C�e�����擾");

                GameManager.Score += 200;
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
                    GameManager.Health += 10;
                }
            }

            Destroy(gameObject);
        }
    }


    void Update()
    {
        
    }
}
