using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject item;
    public void dropItem()
    {
        Vector3 offset = new Vector3(.5f, 1f,0);
         Instantiate(item, transform.position + offset, Quaternion.identity);
    }
}
