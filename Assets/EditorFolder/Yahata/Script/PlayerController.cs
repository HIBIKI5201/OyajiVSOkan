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


    Vector2 firstScale;

    void Start()
    {
        firstScale = transform.localScale;
    }

    //�Փ˂����Ƃ��̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("�G�ɂԂ�����");

        }

        if (collision.gameObject.CompareTag("Item"))
        {
            Debug.Log("�A�C�e�����擾");
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

        yield return new WaitForSeconds(_getTime);

        getCollider.enabled = false;

        Debug.Log("�����Ă�");
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
        if (Input.GetKeyDown(KeyCode.Space))
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
