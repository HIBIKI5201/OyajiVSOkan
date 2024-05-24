using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Rigidbody2D EnemyRB;

    private int goAxis;
    private bool onGround;
    private bool enemyDead;
    [SerializeField] private float _enemyMoveSpeed;

    void Start()
    {
        goAxis = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;
            Debug.Log(collisionNormal.x);

            if(collisionNormal.x  > 0.8f)
            {
                goAxis = 1;
            }
            else if (collisionNormal.x < -0.8f)
            {
                goAxis= -1;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Attack")
        {
            enemyDead = true;
            onGround = false;

            GetComponent<CircleCollider2D>().enabled = false;
            EnemyRB.velocity = new Vector2(-6, 10);
            //StartCoroutine(EnemyDead());
        }
    }

    void Update()
    {
        if (onGround && !enemyDead)
        {
            EnemyRB.velocity = new Vector2(_enemyMoveSpeed * goAxis, 0);
        }
    }
}
