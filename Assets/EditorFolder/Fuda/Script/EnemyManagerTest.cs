using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManagerTest : MonoBehaviour
{
    [SerializeField] Rigidbody2D EnemyRB;

    private int goAxis;
    private bool onGround;
    private bool enemyDead;
    [SerializeField] private float _enemyMoveSpeed;
    private bool trueAI;

    //変更点
    private bool enemy3DTurn = false;
    private bool _spiderWebLaunch = false;
    private GameObject _spiderWeb;

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

        System.Array values = System.Enum.GetValues(typeof(EnemyKind));
        int randomIndex = Random.Range(0, values.Length);
        enemyKind = (EnemyKind)values.GetValue(randomIndex);

        if (enemyKind == EnemyKind.Spider || enemyKind == EnemyKind.G || enemyKind == EnemyKind.Dog)
        {
            trueAI = true;
        }
        else if (enemyKind == EnemyKind.Paper || enemyKind == EnemyKind.Smoke || enemyKind == EnemyKind.Fridge)
        {
            trueAI = false;
        }

        if (enemyKind == EnemyKind.Spider)
        {
            //変更点↓
            Destroy(GetComponent<SpriteRenderer>());
            GameObject _spider = Instantiate(transform.parent.gameObject.transform.Find("SpiderEnemy").gameObject,transform.position, Quaternion.identity) as GameObject;
            _spider.SetActive(true);
            _spider.transform.parent = transform;
            
            // enemyRenderer.sprite = enemySprite[0];
            //変更点↑
            _enemyMoveSpeed = 1.5f;
        }
        else if (enemyKind == EnemyKind.G)
        {
            //変更点↓
            Destroy(GetComponent<SpriteRenderer>());
            GameObject _goki = Instantiate(transform.parent.gameObject.transform.Find("GokiEnemy").gameObject, transform.position, Quaternion.identity) as GameObject;
            _goki.SetActive(true);
            _goki.transform.parent = transform;


            // enemyRenderer.sprite = enemySprite[1];
            //変更点↑

            _enemyMoveSpeed = 2.25f;
        }
        else if (enemyKind == EnemyKind.Dog)
        {
            enemyRenderer.sprite = enemySprite[2];
            _enemyMoveSpeed = 3f;
        }
        else if (enemyKind == EnemyKind.Paper)
        {
            enemyRenderer.sprite = enemySprite[3];
        }
        else if (enemyKind == EnemyKind.Smoke)
        {
            enemyRenderer.sprite = enemySprite[4];
        }
        else if (enemyKind == EnemyKind.Fridge)
        {
            enemyRenderer.sprite = enemySprite[5];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Attack")
        {
            enemyDead = true;
            onGround = false;

            GetComponent<CircleCollider2D>().enabled = false;

            playerAngle = GameObject.Find("Player").GetComponent<Transform>().localScale.x;
            EnemyRB.velocity = new Vector2(3 * playerAngle, 10);
            EnemyRB.gravityScale = 0.05f;

            EnemyRB.freezeRotation = false;
            EnemyRB.AddTorque(90);

            if (enemyKind == EnemyKind.Spider)
            {
                Debug.Log("蜘蛛を倒した");
                GameManager.Score += 50;
            }
            else if (enemyKind == EnemyKind.G)
            {
                Debug.Log("ゴキブリを倒した");
                GameManager.Score += 100;
            }
            else if (enemyKind == EnemyKind.Dog)
            {
                Debug.Log("犬を倒した");
                GameManager.Score += 200;
            }
            else if (enemyKind == EnemyKind.Paper)
            {
                Debug.Log("ティッシュを倒した");
                GameManager.Score += 50;
            }
            else if (enemyKind == EnemyKind.Smoke)
            {
                Debug.Log("タバコを倒した");
                GameManager.Score += 100;
            }
            else if (enemyKind == EnemyKind.Fridge)
            {
                Debug.Log("冷蔵庫を倒した");
                GameManager.Score += 200;
            }
        }



        if (collision.gameObject.CompareTag("EnemyTurn"))
        {
            goAxis *= -1;
            if(enemyKind== EnemyKind.G)
            {
                transform.rotation = transform.rotation * new Quaternion(0, 1, 0, 0);
            }
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
            //変更点↓　一定の高さにまで落ちたら止まる。
            if (enemyKind == EnemyKind.Spider && transform.position.y < 2.5f)
            {
                EnemyRB.velocity = Vector2.zero;
                //クモ止まった後playerめがけて糸を吐く
                if (!_spiderWebLaunch)
                {
                    _spiderWeb = Instantiate(transform.parent.gameObject.transform.Find("SpiderWeb").gameObject, transform.position, Quaternion.identity);
                    _spiderWeb.SetActive(true);
                    _spiderWeb.transform.parent = transform.root.transform.gameObject.transform.parent;
                    _spiderWeb.transform.LookAt(GameObject.Find("Player").GetComponent<Transform>().transform.position);
                }
                _spiderWebLaunch = true;

            }
            else
            {
                EnemyRB.velocity = new Vector2(_enemyMoveSpeed * goAxis, EnemyRB.velocity.y);
            }
        }
    }
    private void FixedUpdate()
    {
        if (_spiderWebLaunch)
        {
           _spiderWeb.transform.position += new Vector3(1.0f,0,0);
        }
    }
}
