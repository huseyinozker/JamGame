using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ShieldEnemy : Enemy
{
    private float strength = 100; // for dodging
    public float dodgeThreshold = 5;
    public float strengthThreshold = 0;//to decrease strength after take damage

    public override void takeDamage(float takenDamage, float knockBackRange)
    {
        bool isDodge = false;
        /* HİLE KONTROLÜ */
        if (GameManager.instance.DODGE)
        {
            isDodge = true;
            if(SceneManager.GetActiveScene().buildIndex == 1 && GameManager.instance.tutIndex==1) // yani level 1 ise
            {
                GameManager.instance.tutCounter++;
                if (GameManager.instance.tutCounter >= 3)
                {
                    GameManager.instance.openTutMenu();
                }
            }
        }
        else if (GameManager.instance.DODGE == false)//yani hile açık
            isDodge = false;

        if (!dead && !isDodge)
        {
            takeHit = true;
            animator.ForceStateNormalizedTime(0f);
            animator.SetInteger("condition", 2);

            health = health - takenDamage;
            healtBar.value = health;

            knockBack(knockBackRange);
            popDamageText(takenDamage);

            time = 0f;

            strength = strength - (takenDamage * strengthThreshold);

            if (health <= 0)
                Dead();
        }
        else if (!dead && isDodge)
        {
            takeHit = true;
            animator.SetInteger("condition", 5); // kalkan 5 ile çağrılıyor.
            knockBack(knockBackRange);
            time = 0f;
        }
    }
    bool computeDodge()// compute dodge chance
    {
        float dodgeChance = Random.RandomRange(0, 100);
        float dodgeMax = strength * dodgeThreshold / 10;

        if (dodgeChance >= 0 && dodgeChance <= dodgeMax)
            return true;
        else
            return false;
    }
}
