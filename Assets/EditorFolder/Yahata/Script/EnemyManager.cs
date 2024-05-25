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
    [Header("�����ڕύX")]
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private Sprite[] enemySprite;

    [Header("�G�̃X�e�[�^�X")]
    private float _enemyMoveSpeed;
    [SerializeField] private float[] _enemySpeed;
    [SerializeField] private int[] _hetScore;
    [SerializeField] private int[] _hitDamage;

    //�ǉ��ϐ�
    private bool _spiderWebLaunch = false;
    private GameObject _spiderWeb;
    private Vector3 _aimedPosition;
    //�����܂�

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
        else if(enemyKind == EnemyKind.Paper || enemyKind == EnemyKind.Smoke || enemyKind == EnemyKind.Fridge)
        {
            trueAI = false;
        }

        if(enemyKind == EnemyKind.Spider)
        {
            //�ύX�_��
            Destroy(GetComponent<SpriteRenderer>());
            GameObject _spider = Instantiate(transform.parent.gameObject.transform.Find("SpiderEnemy").gameObject, transform.position, Quaternion.identity) as GameObject;
            _spider.SetActive(true);
            _spider.transform.parent = transform;

            // enemyRenderer.sprite = enemySprite[0];
            //�ύX�_��
            _enemyMoveSpeed = _enemySpeed[0];
        }
        else if(enemyKind == EnemyKind.G)
        {
            //�ύX�_��
            Destroy(GetComponent<SpriteRenderer>());
            GameObject _goki = Instantiate(transform.parent.gameObject.transform.Find("GokiEnemy").gameObject, transform.position, Quaternion.identity) as GameObject;
            _goki.SetActive(true);
            _goki.transform.parent = transform;
            //�R�_�C�_�[
            gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;
            // enemyRenderer.sprite = enemySprite[1];
            //�ύX�_��
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
                Debug.Log("�w偂�|����");
                GameManager.Score += _hetScore[0];
            } 
            else if(enemyKind == EnemyKind.G)
            {
                Debug.Log("�S�L�u����|����");
                GameManager.Score += _hetScore[1];
            }
            else if(enemyKind== EnemyKind.Dog)
            {
                Debug.Log("����|����");
                GameManager.Score += _hetScore[2];
            }
            else if(enemyKind == EnemyKind.Paper)
            {
                Debug.Log("�e�B�b�V����|����");
                GameManager.Score += _hetScore[3];
            }
            else if(enemyKind == EnemyKind.Smoke)
            {
                Debug.Log("�^�o�R��|����");
                GameManager.Score += _hetScore[4];
            }
            else if(enemyKind == EnemyKind.Fridge)
            {
                Debug.Log("�①�ɂ�|����");
                GameManager.Score += _hetScore[5];
            }
        }

 

        if(collision.gameObject.CompareTag("EnemyTurn"))
        {
            goAxis *= -1;

            //����G�����Ȃ̂�G�������w�肵�܂����B3D���f����������Ȃ�bool���₹�Γ��l�Ɏg���܂��ˁB
            if (enemyKind == EnemyKind.G)
            {
                transform.rotation = transform.rotation * new Quaternion(0, 1, 0, 0);
            }
            //�����܂�
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
            //�ύX�_���@���̍����ɂ܂ŗ�������~�܂�B
            if (enemyKind == EnemyKind.Spider && transform.position.y < 2.5f)
            {
                EnemyRB.velocity = Vector2.zero;
                EnemyRB.gravityScale = 0.05f;
                //�N���~�܂�����player�߂����Ď���f��
                if (!_spiderWebLaunch)
                {
                    _spiderWeb = Instantiate(transform.parent.gameObject.transform.Find("SpiderWeb").gameObject, transform.position, Quaternion.identity);
                    _spiderWeb.SetActive(true);
                    _spiderWeb.transform.parent = transform.parent.parent;
                    _aimedPosition = GameObject.Find("Player").GetComponent<Transform>().transform.position;
                }
                _spiderWebLaunch = true;

            }
            else
            {
                EnemyRB.velocity = new Vector2(_enemyMoveSpeed * goAxis, EnemyRB.velocity.y);
            }
            //�����܂�
        }
    }
    //�t���[�����[�g�������Ă��Q�[���X�s�[�h�ƃI�u�W�F�N�g�̓�������v����悤��FixedUpdate�ŋL�q���܂����B
    private void FixedUpdate()
    {
        if (_spiderWebLaunch)
        {
            _spiderWeb.transform.position += (_aimedPosition - _spiderWeb.transform.position) * 0.01f;
            if (_spiderWeb.transform.position == _aimedPosition)
            {
                _spiderWebLaunch = false;
            }
        }
    }
    //�����܂�
}
