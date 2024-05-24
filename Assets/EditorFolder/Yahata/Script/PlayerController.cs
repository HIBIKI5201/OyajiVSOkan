using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRB;

    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpPower;

    Vector2 firstScale;

    void Start()
    {
        firstScale = transform.localScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("敵にぶつかった");
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            Debug.Log("アイテムを取得");
            ItemManager.ItemKind getItemKind = collision.gameObject.GetComponent<ItemManager>().itemKind;

            if(getItemKind == ItemManager.ItemKind.AidKit)
            {
                Debug.Log("回復アイテムを取得");
            } 
            else if(getItemKind == ItemManager.ItemKind.Sake)
            {
                Debug.Log("酒を取得");
            }


            Destroy(collision.gameObject);
        }
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            playerRB.velocity = new Vector2(horizontal * _moveSpeed, playerRB.velocity.y);
            transform.localScale = new Vector2(horizontal * firstScale.x, firstScale.y) ;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }
}
