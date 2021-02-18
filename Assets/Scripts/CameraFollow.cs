using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject;
    public Vector2 followOffset;
    private Vector2 threshold;

    public float speed;

    void Start()
    {
        calculateThreshold();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 follow = followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);

        Vector3 newPosititon = transform.position;
        if (Mathf.Abs(xDifference) >= threshold.x)
        {
            newPosititon.x = follow.x;
        }

        transform.position = Vector3.MoveTowards(transform.position, newPosititon, speed * Time.deltaTime);
    }
    private Vector3 calculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;

        return t;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}
