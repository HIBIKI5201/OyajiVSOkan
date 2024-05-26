using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRB;

    [Header("�v���C���[�̈ړ��X�e�[�^�X")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpPower;

    [Header("�U���V�X�e��")]
    [SerializeField] BoxCollider2D attackCollider;
    [SerializeField] float _attackTime;
    [SerializeField] BoxCollider2D getCollider;
    [SerializeField] float _getTime;

    [Header("�A�j���[�^�[")]
    [SerializeField] Animator animator;

    [Header("�I�[�f�B�I")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClips;

    //�����l�̕ۑ�
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

    //�Փ˂����Ƃ��̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("�G�ɂԂ�����");
        }
    }

    //�N���̑��ɂ��W�Q
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trap"))
        {
            Debug.Log("�N���̑��ɂЂ���������");

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
            Debug.Log("�N���̑�����ʂ���");

            _moveSpeed = defMoveSpeed;
            _jumpPower = defJumpPower;
        }
    }

    //�U���L�[�������ꂽ���̏���
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
        //Get���[�V�����ǉ�
        getCollider.enabled = true;

        animator.SetBool("get", true);

        yield return new WaitForSeconds(_getTime);

        getCollider.enabled = false;

        Debug.Log("�����Ă�");

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

    //�W�����v�E�U���L�[�̏���
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

        // Get���[�V������ǉ�
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            StartCoroutine(Get());
        }
    }
}
