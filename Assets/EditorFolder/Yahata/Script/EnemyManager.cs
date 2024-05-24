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
    private bool trueAI;

    public EnemyKind enemyKind;
    public enum EnemyKind
    {
        Spider,
        G,
        Dog,
        Paper,
        Smoke,
        Fridge
    }
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private Sprite[] enemySprite;

    private float playerAngle;

    void Start()
    {
        goAxis = 1;
        onGround = true;

        if(enemyKind == EnemyKind.Spider || enemyKind == EnemyKind.G || enemyKind == EnemyKind.Dog)
        {
            trueAI = true;
        }
        else if(enemyKind == EnemyKind.Paper || enemyKind == EnemyKind.Smoke || enemyKind == EnemyKind.Fridge)
        {
            trueAI = false;
        }

        if(enemyKind == EnemyKind.Spider)
        {
            enemyRenderer.sprite = enemySprite[0];
            _enemyMoveSpeed = 1.5f;
        }
        else if(enemyKind == EnemyKind.G)
        {
            enemyRenderer.sprite = enemySprite[1];
            _enemyMoveSpeed = 2.25f;
        }
        else if(enemyKind == EnemyKind.Dog)
        {
            //enemyRenderer.sprite = enemySprite[2];
            _enemyMoveSpeed = 3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Attack")
        {
            enemyDead = true;
            onGround = false;

            GetComponent<CircleCollider2D>().enabled = false;

            playerAngle = GameObject.Find("Player").GetComponent<Transform>().localScale.x;
            EnemyRB.velocity = new Vector2(3 * playerAngle, 10);

            if(enemyKind == EnemyKind.Spider)
            {
                GameManager.Score += 50;
            } 
            else if(enemyKind == EnemyKind.G)
            {
                GameManager.Score += 100;
            }
            else if(enemyKind== EnemyKind.Dog)
            {
                GameManager.Score += 200;
            }
            else if(enemyKind == EnemyKind.Paper)
            {
                GameManager.Score += 50;
            }
            else if(enemyKind == EnemyKind.Smoke)
            {
                GameManager.Score += 100;
            }
            else if(enemyKind == EnemyKind.Fridge)
            {
                GameManager.Score += 200;
            }
        }

 

        if(collision.gameObject.CompareTag("EnemyTurn"))
        {
            goAxis *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyKind == EnemyKind.Spider)
            {
                GameManager.Health -= 10;
            }
            else if (enemyKind == EnemyKind.G)
            {
                GameManager.Health -= 15;
            }
            else if (enemyKind == EnemyKind.Dog)
            {
                GameManager.Health -= 20;
            }
            else if (enemyKind == EnemyKind.Paper)
            {
                GameManager.Health -= 10;
            }
            else if (enemyKind == EnemyKind.Smoke)
            {
                GameManager.Health -= 15;
            }
            else if (enemyKind == EnemyKind.Fridge)
            {
                GameManager.Health -= 20;
            }

            Debug.Log(GameManager.Health);
        }
    }

    void Update()
    {
        if (onGround && !enemyDead && trueAI)
        {
            EnemyRB.velocity = new Vector2(_enemyMoveSpeed * goAxis, EnemyRB.velocity.y);
        }
    }
}
