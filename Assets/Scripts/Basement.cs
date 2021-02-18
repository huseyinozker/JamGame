using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : MonoBehaviour
{
    public Vector2 targetVector;
    public float speed = 2f;

    BoxCollider2D bc2d;
    
    // Update is called once per frame
    private void Start()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (GameManager.instance.GROUND)
        {
            bc2d.enabled = true;
            if (!GameManager.instance.approximate(transform.position.y, targetVector.y, 0.01f))
                transform.position = Vector2.MoveTowards(transform.position, targetVector, speed * Time.deltaTime);
        }
    }
}
