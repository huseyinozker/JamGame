using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLetter : MonoBehaviour
{
    public GameObject menu;
    MenuManager menuManager;
    string c;
    bool isWrited = false;
    private void Start()
    {
        menuManager = menu.GetComponent<MenuManager>();
        c = transform.GetChild(0).gameObject.GetComponent<Text>().text;
    }
    public void writeLetter()
    {
        if (!isWrited)
        {
            menuManager.addLetter(c);
            isWrited = true;
        }else if (isWrited)
        {
            menuManager.removeLetter();
            isWrited = false;
        }
    }
}
