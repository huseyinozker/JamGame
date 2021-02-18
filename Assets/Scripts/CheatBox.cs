using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class CheatBox : MonoBehaviour
{
    public List<Button> letterButtons;

    public float closeTime = 10f;
    public Text closeTimeTxt;
    public Slider timeSlider;
    float firsTime = 5f;

    bool firstSeen = true;
    float firstSeenTime = 5f;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (firstSeen == false)
        {
            if (closeTime > 0)
            {
                closeTime = closeTime - 1f * Time.deltaTime;

            }
            else if (closeTime < 1)
            {
                closeBox();
            }
        }else if (firstSeen == true)
        {
            if (firstSeenTime > 0)
                firstSeenTime = firstSeenTime - 1f * Time.deltaTime;
            else if (firstSeenTime < 1)
            {
                hideLetters();
                firstSeen = false;
            }
        }
        closeTimeTxt.text = closeTime.ToString("00");
        timeSlider.value = closeTime;
    }
    public void openBox()
    {
        closeTime = 10f;
        firstSeen = true;
        firstSeenTime = 5f;
        loadLetters();
        displayLetters();
    }
    public void closeBox()
    {
        GameManager.instance.isPaused = false;
        resetCheat();
        GameManager.instance.resetCheatVariables();
        GameManager.instance.remainCheat--;
        gameObject.SetActive(false);
    }
    void hideLetters()
    {
        foreach(Button b in letterButtons)
        {
            b.GetComponent<LetterButton>().hideLetter();
            b.interactable = true;
        }
    }
    void displayLetters()
    {
        foreach (Button b in letterButtons)
        {
            b.GetComponent<LetterButton>().showLetter();
        }
    }
    void resetCheat()
    {
        foreach(Button b in letterButtons)
        {
            b.GetComponent<LetterButton>().hideLetter();
        }
    }
    void loadLetters()
    {
        int a = SceneManager.GetActiveScene().buildIndex;
        
        for(int i=0;i<20;i++)
        {
            letterButtons[i].GetComponent<LetterButton>().letterText.text = GameManager.instance.puzzles[a][i].ToString();
            letterButtons[i].interactable = false;
        }
    }
    void activeButtons()
    {
        foreach (Button b in letterButtons)
            b.interactable = true;
    }
}
