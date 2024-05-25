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

        yield return new WaitForSeconds(_attackTime);

        attackCollider.enabled = false;
    }

    //���E�ړ��̏���
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            playerRB.velocity = new Vector2(horizontal * _moveSpeed, playerRB.velocity.y);
            transform.localScale = new Vector2(horizontal * firstScale.x, firstScale.y);
        }
    }

    //�W�����v�E�U���L�[�̏���
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
    }
}
