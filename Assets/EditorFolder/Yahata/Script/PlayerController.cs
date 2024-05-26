using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRB;

    [Header("プレイヤーの移動ステータス")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpPower;

    [Header("攻撃システム")]
    [SerializeField] BoxCollider2D attackCollider;
    [SerializeField] float _attackTime;
    [SerializeField] BoxCollider2D getCollider;
    [SerializeField] float _getTime;

    [Header("アニメーター")]
    [SerializeField] Animator animator;

    [Header("オーディオ")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClips;

    //初期値の保存
    float defMoveSpeed;
    float defJumpPower;

    int strugled=0;

    Vector2 firstScale;

    void Start()
    {
        defJumpPower = _jumpPower;
        defMoveSpeed = _moveSpeed;
        firstScale = transform.localScale;
    }

    //衝突したときの処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("敵にぶつかった");
        }
    }

    //クモの巣による妨害
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trap"))
        {
            Debug.Log("クモの巣にひっかかった");

            _moveSpeed = defMoveSpeed*0.3f;
            _jumpPower = defJumpPower*0.3f;

            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
            {
                if (strugled>6)
                {
                    Destroy(collider.gameObject);
                    strugled = 0;
                }

                Debug.Log(strugled);
                strugled++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trap"))
        {
            Debug.Log("クモの巣からぬけた");

            _moveSpeed = defMoveSpeed;
            _jumpPower = defJumpPower;
        }
    }

    //攻撃キーを押された時の処理
    private IEnumerator Attack()
    {
        attackCollider.enabled = true;

        animator.SetBool("attack", true);

        audioSource.PlayOneShot(audioClips);

        yield return new WaitForSeconds(_attackTime);

        attackCollider.enabled = false;

        yield return new WaitForSeconds(0.2f);

        animator.SetBool("attack", false);
    }

    private IEnumerator Get()
    {
        //Getモーション追加
        getCollider.enabled = true;

        animator.SetBool("get", true);

        yield return new WaitForSeconds(_getTime);

        getCollider.enabled = false;

        Debug.Log("あいてむ");

        yield return new WaitForSeconds(0.2f);

        animator.SetBool("get", false);
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            playerRB.velocity = new Vector2(horizontal * _moveSpeed, playerRB.velocity.y);
            transform.localScale = new Vector2(horizontal * firstScale.x, firstScale.y);

            animator.SetBool("move", true);
        } else
        {
            animator.SetBool("move", false);
        }
    }

    //ジャンプ・攻撃キーの処理
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < 4)
        {
            playerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Attack());
        }

        // Getモーションを追加
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            StartCoroutine(Get());
        }
    }
}
