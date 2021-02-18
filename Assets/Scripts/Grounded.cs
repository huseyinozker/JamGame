using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            gameObject.transform.parent.GetComponent<Player>().isGround = true;
            gameObject.transform.parent.GetComponent<Player>().isJumping = false;
            //transform.parent.gameObject.GetComponent<Animator>().SetInteger("condition", 0);
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            gameObject.transform.parent.GetComponent<Player>().isGround = false;
           // transform.parent.gameObject.GetComponent<Animator>().SetInteger("condition", 5);
        }
    }
}
