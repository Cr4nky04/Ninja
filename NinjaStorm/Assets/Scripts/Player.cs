using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 350;
    [SerializeField] private Animator anim;
    [SerializeField] private float jumpForce;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;


    private bool isGrounded;    //Check xem có ?ang trên m?t ??t hay không
    private bool isJumping;     //Check xem có ?ang nh?y hay không
    private bool isAttack;      //Check xem có ?ang t?n công hay không
    private bool isDead;
    private float horizontal;
    private float vertical;
    private string currentAnim;
    private int coin = 0;
    private Vector3 savePoint;


    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    void Update()
    {

        isGrounded = CheckGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");    //L?y ??u vào t? bàn phím di chuy?n trái hay ph?i
        //                                                //vertical = Input.GetAxisRaw("Vertical"); 


        if (isGrounded)
        {
            if(isDead)
            {
                return;
            }

            if (isJumping)
            {
                Debug.Log("return");
                return;
            }

            if (isAttack)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            //Attack
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                return;
            }

            //Throw
            if (Input.GetKeyDown(KeyCode.K))
            {
                Throw();
                return;
            }


            //Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                return;
            }

            //Change anim run
            if (Mathf.Abs(horizontal) > 0.01f)
            {
                ChangeAnim("Run");
            }
        }

        

        //Run
        if (Mathf.Abs(horizontal) > 0.01f && !isDead)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if(isGrounded && !isDead)
        {
            ChangeAnim("Idle");
            rb.velocity = Vector2.zero;
        }

        //Fall
        if (!isGrounded && rb.velocity.y < 0 )
        {
            ChangeAnim("Fall");
            isJumping = false;
        }

    }

    public override void OnInit()
    {
        base.OnInit();
        isDead = false;
        isAttack = false;
        transform.position = savePoint;
        DeactiveAttack();
        ChangeAnim("Idle");
        SavePoint();
        
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    public override void OnDeath()
    {
        base.OnDeath();
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);  // T?o ra m?t m?i tên b?n xu?ng ?? dài 1.1f ?? check xem có ch?m ??t hay không  

        //if (hit.collider != null)
        //{
        //    return true;
        //}
        //else return false;
        return hit.collider != null;
    }

    public void Attack()
    {
        isAttack = true;
        ChangeAnim("Attack");
        Invoke(nameof(ResetAttack), 0.5f);  //Tro ve trang thai Idle
        ActiveAttack();
        Invoke(nameof(DeactiveAttack), 0.5f);
    }

    public void Throw()
    {
        isAttack = true;
        ChangeAnim("Throw");
        Invoke(nameof(ResetAttack), 0.5f);   //Tro ve trang thai Idle
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    

    public void Jump()
    {
        ChangeAnim("Jump");
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("Idle");
    }

    private void ResetRun()
    {
        ChangeAnim("Idle");
    }

    private void ChangeAnim(string animName)
    {

        if (currentAnim != animName)
        {
         
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);

        }
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeactiveAttack()
    {
        attackArea.SetActive(false);
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        //Earn Coin
        if(collision.tag == "Coin")
        {
            
            Destroy(collision.gameObject);
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
        }

        //Die
        if(collision.tag == "DeathZone")
        {
            isDead = true;
            ChangeAnim("Die");
            Invoke(nameof(OnInit), 2f);
        }
    }

   
}
