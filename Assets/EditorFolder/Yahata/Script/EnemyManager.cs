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
    [Header("見た目変更")]
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private Sprite[] enemySprite;

    [Header("敵のステータス")]
    private float _enemyMoveSpeed;
    [SerializeField] private float[] _enemySpeed;
    [Space]
    [SerializeField] private int[] _hetScore;
    [SerializeField] private int[] _hitDamage;
    [Space]
    [SerializeField] private float _webReminTime = 10.0f;

    private bool spiderWebLaunch = false;
    private GameObject spiderWeb;

    private Vector3 aimedPosition;
    private float webReminTimer;
    private float _DestroyTimer;

    private float playerAngle;

    void Start()
    {
        //エネミーにはDestroyしてもらいたかったけど時間まではスクリプトに生きててもらいたかった。
        _DestroyTimer = _webReminTime;

        goAxis = 1;
        onGround = true;

        System.Array values = System.Enum.GetValues(typeof(EnemyKind));
        int randomIndex = Random.Range(0, values.Length);
        enemyKind = (EnemyKind)values.GetValue(randomIndex);

        if (enemyKind == EnemyKind.Spider || enemyKind == EnemyKind.G || enemyKind == EnemyKind.Dog)
        {
            trueAI = true;
        }
        else if(enemyKind == EnemyKind.Paper || enemyKind == EnemyKind.Smoke || enemyKind == EnemyKind.Fridge)
        {
            trueAI = false;
        }

        if(enemyKind == EnemyKind.Spider)
        {
            //変更点↓
            Destroy(GetComponent<SpriteRenderer>());

            GameObject spider3D = Instantiate(transform.parent.gameObject.transform.Find("SpiderEnemy").gameObject, transform.position, Quaternion.identity) as GameObject;
            spider3D.SetActive(true);
            spider3D.transform.parent = transform;

            _enemyMoveSpeed = _enemySpeed[0];
        }
        else if(enemyKind == EnemyKind.G)
        {
            //変更点↓
            Destroy(GetComponent<SpriteRenderer>());

            GameObject goki3D = Instantiate(transform.parent.gameObject.transform.Find("GokiEnemy").gameObject, transform.position, Quaternion.identity) as GameObject;
            goki3D.SetActive(true);
            goki3D.transform.parent = transform;

            gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;

            _enemyMoveSpeed = _enemySpeed[1];
        }
        else if(enemyKind == EnemyKind.Dog)
        {
            enemyRenderer.sprite = enemySprite[2];
            _enemyMoveSpeed = _enemySpeed[2];
        }
        else if (enemyKind == EnemyKind.Paper)
        {
            enemyRenderer.sprite = enemySprite[3];
        }
        else if(enemyKind == EnemyKind.Smoke)
        {
            enemyRenderer.sprite = enemySprite[4];
        }
        else if(enemyKind == EnemyKind.Fridge)
        {
            int random = Random.Range(5, 7);
            enemyRenderer.sprite = enemySprite[random];
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

            EnemyRB.freezeRotation = false;
            EnemyRB.AddTorque(90);

            if (enemyKind == EnemyKind.Spider)
            {
                Debug.Log("蜘蛛を倒した");
                GameManager.Score += _hetScore[0];
            } 
            else if(enemyKind == EnemyKind.G)
            {
                Debug.Log("ゴキブリを倒した");
                GameManager.Score += _hetScore[1];
            }
            else if(enemyKind== EnemyKind.Dog)
            {
                Debug.Log("犬を倒した");
                GameManager.Score += _hetScore[2];
            }
            else if(enemyKind == EnemyKind.Paper)
            {
                Debug.Log("ティッシュを倒した");
                GameManager.Score += _hetScore[3];
            }
            else if(enemyKind == EnemyKind.Smoke)
            {
                Debug.Log("タバコを倒した");
                GameManager.Score += _hetScore[4];
            }
            else if(enemyKind == EnemyKind.Fridge)
            {
                Debug.Log("冷蔵庫を倒した");
                GameManager.Score += _hetScore[5];
            }

            //倒した後destroyするコードを追記　※変数増やすの面倒なのでタイマー減らしたことをトリガー代わりにしてます。
            _DestroyTimer -= 0.01f;
        }

        if (collision.gameObject.CompareTag("EnemyTurn"))
        {
            goAxis *= -1;

            //現状GだけなのでGだけを指定しました。3Dモデルが増えるならbool増やせば同様に使えますね。
            if (enemyKind == EnemyKind.G)
            {
                transform.rotation = transform.rotation * new Quaternion(0, 1, 0, 0);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyKind == EnemyKind.Spider)
            {
                GameManager.Health -= _hitDamage[0] * Time.deltaTime;
            }
            else if (enemyKind == EnemyKind.G)
            {
                GameManager.Health -= _hitDamage[1] * Time.deltaTime;
            }
            else if (enemyKind == EnemyKind.Dog)
            {
                GameManager.Health -= _hitDamage[2] * Time.deltaTime;
            }
            else if (enemyKind == EnemyKind.Paper)
            {
                GameManager.Health -= _hitDamage[3] * Time.deltaTime;
            }
            else if (enemyKind == EnemyKind.Smoke)
            {
                GameManager.Health -= _hitDamage[4] * Time.deltaTime;
            }
            else if (enemyKind == EnemyKind.Fridge)
            {
                GameManager.Health -= _hitDamage[5] * Time.deltaTime;
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
                EnemyRB.gravityScale = 0.05f;

                //クモ止まった後playerめがけて糸を吐く
                if (!spiderWebLaunch && webReminTimer == 0)
                {
                    spiderWeb = Instantiate(transform.parent.gameObject.transform.Find("SpiderWeb").gameObject, transform.position, Quaternion.identity);
                    spiderWeb.SetActive(true);
                    spiderWeb.transform.parent = transform.parent.parent;

                    aimedPosition = GameObject.Find("Player").GetComponent<Transform>().transform.position;

                    spiderWebLaunch = true;
                }
            }
            else
            {
                EnemyRB.velocity = new Vector2(_enemyMoveSpeed * goAxis, EnemyRB.velocity.y);
            }
        }
        //クモの巣が消える処理
        if (webReminTimer > 0 && webReminTimer <= _webReminTime)
        { 
            webReminTimer += Time.deltaTime;
        }
        if (webReminTimer >= _webReminTime)
        {
            Destroy(spiderWeb);
            webReminTimer = 0.0f;
        }

        //エネミーを消す処理
        if (_DestroyTimer < _webReminTime)
        {
            _DestroyTimer -= Time.deltaTime;

            if (_DestroyTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    //フレームレート下がってもゲームスピードとオブジェクトの動きが一致するようにFixedUpdateで記述しました。
    private void FixedUpdate()
    {
        if (spiderWebLaunch)
        {
            spiderWeb.transform.position += (aimedPosition - spiderWeb.transform.position) * 0.01f;

            if (spiderWeb.transform.position.y <= aimedPosition.y+0.1)
            {
                webReminTimer += 0.0001f;
                spiderWebLaunch = false;
            }
        }
    }
}
