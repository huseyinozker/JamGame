using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    private bool isClosed = true;

    public Text letterText;
    void Start()
    {
    }
    public void toggleButton()
    {
        if (isClosed)
        {
            letterText.gameObject.SetActive(true);
            string a = letterText.text;
            GameManager.instance.addLetter(a);
            isClosed = false;
        }else if (!isClosed)
        {
            letterText.gameObject.SetActive(false);
            GameManager.instance.removeLetter();
            isClosed = true;
        }
    }
    public void hideLetter()
    {
        letterText.gameObject.SetActive(false);
        isClosed = true;
    }
    public void showLetter()
    {
        letterText.gameObject.SetActive(true);
        isClosed = false;
    }
}
