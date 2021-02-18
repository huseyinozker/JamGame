using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    private Rigidbody2D rb2d;
    [SerializeField] protected Slider healtBar;
    public Vector3 sliderOffset;

    public float health;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float AttackDistance = 0.5f;
    protected bool following = false;
    protected bool takeHit = false;
    protected bool attack = false;
    protected bool attacking = false;//for attack process
    protected bool dead = false;

    public Transform _target;
    private float _direction = 1f;
    float scaleX;

    public float timeBtwAttack;
    [SerializeField] protected float time;

    public GameObject floatingTextPrefab;

    

    private void Start()
    {
        /* component assigns */
        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("speed", moveSpeed / 2);
        rb2d = gameObject.GetComponent<Rigidbody2D>();


        /* healtBar initialization */
        healtBar.maxValue = 100f;
        healtBar.minValue = 0f;
        healtBar.value = health;

        scaleX = transform.localScale.x;

        time = timeBtwAttack;
    }
    private void Update()
    {
        
        if (!GameManager.instance.isPaused && GameManager.instance.ENEMY)
        {
            
            /* Slider Position */
            healtBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + sliderOffset);
            healtBar.value = health;
            if (_target != null && !dead)
            {
                if (GameManager.instance.approximate(transform.position.x, _target.position.x, AttackDistance) && !attacking)//Vector2.Distance(transform.position, new Vector2(_target.position.x, transform.position.y)) <= AttackDistance
                {
                    if (_target.GetComponent<Player>().health > 0)
                    {
                        following = false;
                        attack = true;
                    }

                }
                else if (Vector2.Distance(transform.position, new Vector2(_target.position.x, transform.position.y)) > AttackDistance && !attacking)
                {
                    following = true;
                    attack = false;
                    time = timeBtwAttack;//yakalayınca beklemeden direk vurması için.
                }

                if (attack && !takeHit && !dead)
                {
                    time = time + 1f * Time.deltaTime;
                    if (time >= timeBtwAttack)
                        Attack();

                }
                if (_target.GetComponent<Player>().health <= 0)
                {
                    following = false;
                    attack = false;
                }

                /* Direction and Sprite Facing */
                _direction = Mathf.Sign(_target.position.x - transform.position.x);
                flipCheck();


               
            }
            else if (_target == null && GameManager.instance.ENEMY)
            {
                following = false;
                attack = false;
                playIdle();
            }
        }
        else if (GameManager.instance.ENEMY == false)
        {
            if(animator.GetInteger("condition")!=4)
                Dead();
        }

        if (animator.GetInteger("condition") != 3)
            attacking = false;
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.isPaused)
        {
            if (following && takeHit == false && _target != null)
            {
                followTarget();
            }
        }
    }
    public virtual void takeDamage(float takenDamage, float knockBackRange)
    {
        if (!dead)
        {
            takeHit = true;
            animator.ForceStateNormalizedTime(0f);
            animator.SetInteger("condition", 2);

            health = health - takenDamage;
            

            knockBack(knockBackRange);
            popDamageText(takenDamage);

            time = 0f;


            if (health <= 0)
                Dead();
        }
    }
    public void finishTakeDamage()//hit animasyon eventi tarafından çağrılacak
    {
        takeHit = false;
        animator.SetInteger("condition", 0);
        time = timeBtwAttack - 0.2f;
        rb2d.velocity = new Vector2(0, 0);
    }
    void Attack()
    {
        attacking = true;
        time = 0f;
        animator.ForceStateNormalizedTime(0f);
        animator.SetInteger("condition", 3);
    }
    protected void Dead()
    {
        rb2d.velocity = new Vector2(0, 0);
        following = false;
        attack = false;
        takeHit = false;
        animator.ForceStateNormalizedTime(0f);
        animator.SetInteger("condition", 4);
        dead = true;

        if (gameObject.GetComponent<ItemDrop>() != null)
            gameObject.GetComponent<ItemDrop>().dropItem();

        healtBar.gameObject.SetActive(false);
        Destroy(gameObject, 2f);

    }
    void finishAttack()
    {
        animator.SetInteger("condition", 0);
        attacking = false;
        Debug.Log("asdasdsadxzczxc");
    }
    public void followTarget()
    {
        animator.SetInteger("condition", 1);
        rb2d.velocity = new Vector2(0, 0);//knockback için
        Vector2 newPos = rb2d.position + new Vector2(_direction, 0) * moveSpeed * Time.fixedDeltaTime;
        rb2d.MovePosition(newPos);  
    }
    void flipCheck()
    {
        Vector2 theScale = transform.localScale;
        if (_direction > 0f)
            theScale.x = scaleX; //3.5f;
        else
            theScale.x = -scaleX; //3.5f;

        transform.localScale = theScale;
    }
    protected void knockBack(float range)
    {
        rb2d.AddForce(new Vector2(range * _direction * -1f, 0), ForceMode2D.Impulse);
    }
    void playIdle()
    {
        rb2d.velocity = new Vector2(0, 0);
        animator.SetInteger("condition", 0);
    }
    protected void popDamageText(float damage)
    {
        GameObject g = Instantiate(floatingTextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        g.transform.position = gameObject.transform.position;
        g.GetComponent<TextMesh>().text = "-" + damage.ToString();
    }
}
