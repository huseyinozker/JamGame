using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTrap : MonoBehaviour
{
    public GameObject[] hiddenDoors;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            openHiddenDoors();
        }
    }
    void openHiddenDoors()
    {
        foreach(GameObject g in hiddenDoors)
        {
            g.GetComponent<Animator>().SetBool("open", true);
        }
    }
}
