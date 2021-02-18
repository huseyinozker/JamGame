using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamClamp : MonoBehaviour
{
    [SerializeField]
    private Transform targetFollow;
    public float minX, maxX;
    public float minY, maxY;

    // Update is called once per frame
    private void Start()
    {
        transform.position = new Vector3(targetFollow.position.x, targetFollow.position.y, -10);
    }
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(targetFollow.position.x, minX, maxX),
            Mathf.Clamp(targetFollow.position.y, minY, maxY),
            transform.position.z);
    }
}
