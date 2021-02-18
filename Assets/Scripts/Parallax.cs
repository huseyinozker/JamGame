using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float xLength;
    public float parallaxEffect;
    float startPos;
    void Start()
    {
        xLength = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        float temp = Camera.main.transform.position.x * (1 - parallaxEffect);
        float dist = Camera.main.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp >= startPos + xLength)
        {
            startPos += xLength;

        }
        else if (temp <= startPos - xLength)
        {
            startPos -= xLength;
        }
    }
}
