using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRB;

    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpPower;

    [SerializeField] BoxCollider2D attackCollider;
    [SerializeField] float _attackTime;
    [SerializeField] BoxCollider2D getCollider;
    [SerializeField] float _getTime;

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
        }
    }

    private IEnumerator Attack()
    {
        attackCollider.enabled = true;

        yield return new WaitForSeconds(_attackTime);

        attackCollider.enabled = false;
    }

    private IEnumerator Get()
    {
        //Getモーション追加
        getCollider.enabled = true;

        yield return new WaitForSeconds(_getTime);

        getCollider.enabled = false;

        Debug.Log("あいてむ");
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            playerRB.velocity = new Vector2(horizontal * _moveSpeed, playerRB.velocity.y);
            transform.localScale = new Vector2(horizontal * firstScale.x, firstScale.y);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            StartCoroutine(Attack());
        }

        // Getモーションを追加
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Get());
        }
    }
}
