using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public AudioSource audioSouurce;
    public AudioClip clickSound;

    public Text commandTxt;
    string commandText;
    int textPos = 0;

    public Button[] Buttons;
    public GameObject levelMenu;
    public Button[] levelButtons;

    public GameObject helperMenu;

    void Start()
    {
        textPos = 0;
        commandText = "";
    }
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            audioSouurce.PlayOneShot(clickSound);
    }
    public void addLetter(string a)
    {
        commandText = commandText.Insert(textPos, a);
        commandTxt.text = commandText;
        textPos++;
    }
    public void removeLetter() {
        textPos--;
        commandText = commandText.Remove(textPos);
        commandTxt.text = commandText;
    }
    public void command()
    {
        string s = commandText;
        if (s == "PLAY")
        {
            openLevelMenu();
        }else if (s == "QUIT")
        {
            Application.Quit();
        }
    }
    public void openLevelMenu()
    {
        disableButtons();
        int a = PlayerPrefs.GetInt("MaxLevel", 1);
        Debug.Log(a);
        for (int i = 0; i<a; i++ ){
            levelButtons[i].interactable = true;
        }
        levelMenu.SetActive(true);
    }
    public void closeLevelMenu()
    {
        activateButtons();
        levelMenu.SetActive(false);
    }
    public void goLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
    void disableButtons()
    {
        foreach (Button b in Buttons)
        {
            b.interactable = false;
        }
    }
    void activateButtons()
    {
        foreach (Button b in Buttons)
        {
            b.interactable = true;
        }
    }
    public void openHelperMenu()
    {
        helperMenu.SetActive(true);
        disableButtons();
    }
    public void closeHelperMenu()
    {
        activateButtons();
        helperMenu.SetActive(false);
    }
}
