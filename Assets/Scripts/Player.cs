using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Slider healtBar;
    public Vector3 offsetSlider;
    public float health = 100;

    private Rigidbody2D rb2d;
    private Animator animator;
    [SerializeField] float speed;
    float horizontalMove = 0;
    float direction = 1f;
    public bool isGround=true;
    public bool isJumping = false;
    bool takeHit = false;
    bool dying = false;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] Vector3 jump;

    private bool attackState = false;
    enum AttackType { Attack1,Attack2,Attack3};
    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemy;



    private float scaleX;
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        scaleX = transform.localScale.x;

        jump = new Vector3(0, 2, 0);

        /* healtBar initialization */
        healtBar.maxValue = 100f;
        healtBar.minValue = 0f;
        healtBar.value = health;

    }

    // Update is called once per frame
    void Update()
    {
        /* Slider Position */
        healtBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + offsetSlider);
        healtBar.value = health;

        if (transform.position.y < -5f)
            Die();
        
        if (!GameManager.instance.isPaused)
        {
            anim();

            if (!dying)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    if (!GameManager.instance.JUMP && isGround) {
                        isJumping = true;
                        isGround = false;
                        rb2d.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
                    }else if (GameManager.instance.JUMP)
                    {
                        Debug.Log("daşşak");
                        isJumping = true;
                        isGround = false;
                        rb2d.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
                    }
                    

                }

                if (timeBtwAttack <= 0 && isGround)
                {
                    if (Input.GetMouseButtonDown(0) && !GameManager.instance.cheatBox.activeSelf && !EventSystem.current.IsPointerOverGameObject())
                    {
                        int i = Random.Range(0, 2);
                        if (i == 0)
                            Attack(AttackType.Attack1);
                        if (i == 1)
                            Attack(AttackType.Attack2);
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        Attack(AttackType.Attack3);
                    }
                }
                else
                {
                    timeBtwAttack = timeBtwAttack - Time.deltaTime;
                }

                if (attackState && takeHit)
                    attackState = false;
            }
            if (animator.GetInteger("condition") != 6)
                takeHit = false;
        }
    }

    void Attack(AttackType a)
    {
        horizontalMove = 0f;
        attackState = true;
        if (a == AttackType.Attack1)
            animator.SetInteger("condition", 2);
        if (a == AttackType.Attack2)
            animator.SetInteger("condition", 3);
        if (a == AttackType.Attack3)
            animator.SetInteger("condition", 4);

        timeBtwAttack = startTimeBtwAttack;
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.isPaused && !dying)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            flip();
            move();
        }
    }
    void flip()
    {
        Vector2 theScale = transform.localScale;

        if (horizontalMove < 0f)
            theScale.x = -scaleX;
        else if (horizontalMove > 0f)
            theScale.x = scaleX;


        transform.localScale = theScale;
    }
    void move()
    {
        /*if (attackState == false && !isJumping)
        {
            Vector2 newPos = rb2d.position + new Vector2(horizontalMove, 0) * Time.fixedDeltaTime * speed;
            rb2d.MovePosition(newPos);
        }*/
        if (attackState == false) { 
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.right * -speed * Time.deltaTime;
            }
        }
    }
    void anim()
    {
        if (!dying) {
            if (horizontalMove != 0f && attackState == false && isGround)
            {
                animator.SetInteger("condition", 1);
            }
            else if (horizontalMove == 0f && attackState == false && !isJumping && !takeHit)
            {
                animator.SetInteger("condition", 0);
            }
            else if (isJumping && !isGround)
            {
                Debug.Log("şeyho");
                animator.SetInteger("condition", 5);
            }
            else if (takeHit)
            {
                animator.SetInteger("condition", 6);
            }

            
        }
        else if (dying)
        {
            animator.SetInteger("condition", 7);
        }
    }
    void Hurt(float damage)
    {
        float knockRange = damage / 100 * 2;
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].GetComponent<Enemy>() != null)
                enemiesToDamage[i].GetComponent<Enemy>().takeDamage(damage, knockRange);
        }
    }
    public void finishAttack()
    {
        attackState = false;
    }
    public void finishTakeHit()
    {
        takeHit = false;
        rb2d.velocity = new Vector2(0, 0);
    }
    public void takeDamage(float f)
    {
        health = health - f;
        takeHit = true;
        knockBack(f / 10);
        if (health <= 0)
            Die();
    }
    protected void knockBack(float range)
    {
        rb2d.AddForce(new Vector2(range * direction * -1f, 0), ForceMode2D.Impulse);
    }
    public void Die()
    {
        takeHit = false;
        dying = true;
        Invoke("callRestart", 2f);
    }
    void callRestart()
    {
        GameManager.instance.restartLevel();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
