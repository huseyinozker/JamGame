using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject inPortal;
    public GameObject outPortal;
    Animator animator;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (GameManager.instance.PORTAL)
        {
            animator.SetBool("hidden", false);
        }
    }
    public void openEnd()
    {
        animator.SetBool("portal", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (animator.GetBool("portal")==true)
        {
            if(collision.gameObject.name == "Player")
            {
                collision.gameObject.transform.position = outPortal.transform.position;
                Invoke("closePortal", 2f);
            }
        }
    }
    void closePortal()
    {
        GameManager.instance.PORTAL = false;
        animator.SetBool("close", true);
        outPortal.GetComponent<Animator>().SetBool("close", true);
    }
    public void closeEnd()
    {
        Destroy(gameObject);
    }
}
