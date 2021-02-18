using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Text;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioSource audioSource;
    public AudioClip footStep;

    [SerializeField] float timeBtwCheat;
    float _time =5f;

    public Text timeTxt;

    public GameObject cheatBox;
    public bool canCheat = false;
    public string cheatText;
    public Text cheatTxt;
    public int textPos=0;
    public Text pressText;
    public Button menuBtn;
    public float remainCheat = 2;
    public Text remainCheatTxt;

    public bool hasKey = false;

    /* CHEAT BOOLEAN VARIABLES */
    public bool DODGE = true;
    public bool GROUND = false;
    public bool ENEMY = true;
    public bool HEALTH = true;
    public bool PORTAL = true;
    public bool SPEED = true;
    public bool JUMP = false;

    [SerializeField] string[] cheats = {"DODGE-","GROUND+","SWORD+","HEALTH+","PORTAL","SPEED+" };
    public string[] puzzles =new string[] {"", "RDUXUZOCAATDGEP-+MNY","+123-5EGDOGR1213D15OUND","UND3POP+-MRO12LUGRTAJ"};

    public GameObject pauseMenu;
    public Button[] otherButtons;

    public GameObject tutorialMenu;
    [TextArea] public string[] tuts;
    public Text tutTxt;
    public int tutCounter = 0;
    public int tutIndex;

    public bool isPaused = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    public bool approximate(float a, float b, float tolerance)
    {
        return (Mathf.Abs(a - b) < tolerance);
    }
    private void Start()
    {
        /* first tutorial */
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            tutCounter = 0;
            openTutMenu();
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 0)
        {

            Destroy(gameObject);
        }
        if (_time >0 && !isPaused && remainCheat>0)
        {
            _time = _time - 1f * Time.deltaTime;
            canCheat = false;
            
        }
        else if(remainCheat>0 && _time<1f)
        {
            canCheat = true;
            if (!pressText.gameObject.activeSelf && !cheatBox.activeSelf)
            {
                pressText.gameObject.SetActive(true);
                menuBtn.gameObject.SetActive(true);
            }
        }

        if (canCheat)
        {
            if (Input.GetKeyUp(KeyCode.I))
            {
                if (!cheatBox.activeSelf)
                    openCheatBox();
                else
                    closeCheatBox();
            }
        }
        else if (!canCheat)
        {
            pressText.gameObject.SetActive(false);
            menuBtn.gameObject.SetActive(false);
        }

        timeTxt.text = _time.ToString("00");
        cheatTxt.text = cheatText;
        remainCheatTxt.text = remainCheat.ToString();

    }
    public void openCheatBox()
    {
        cheatBox.SetActive(true);
        cheatBox.GetComponent<CheatBox>().openBox();
        pressText.gameObject.SetActive(false);
        menuBtn.gameObject.SetActive(false);
        loadLetters();
        EventSystem.current.GetComponent<EventSystem>().firstSelectedGameObject = cheatBox.GetComponent<CheatBox>().letterButtons[0].gameObject;
        isPaused = true;
    }
    public void closeCheatBox()
    {
        cheatBox.GetComponent<CheatBox>().closeBox();
    }
    void loadLetters()
    {

    }
    public void resetCheatVariables()
    {
        canCheat = false;
        cheatText = "";
        cheatTxt.text = "";
        textPos = 0;
        if (remainCheat > 0)
            _time = timeBtwCheat;
        else
            _time = 0;
    }
    public void resetLevelVariables()
    {
        remainCheat = 2;
        hasKey = false;
        DODGE = true;
        GROUND = false;
        ENEMY = true;
        HEALTH = true;
        PORTAL = false;
        SPEED = true;
        JUMP = false;
        _time = timeBtwCheat;
}
    public void controlCheat()
    {
        string s = cheatText;
        foreach(string a in cheats)
        {
            if(s == a)
            {
                Debug.Log(s + a);
                cheat(s);
            }
        }
    }
    void cheat(string s)
    {
        Debug.Log(s);
        if (s == "DODGE-")
            DODGE = false;
        else if (s == "GROUND+")
            GROUND = true;
        else if (s == "JUMP+")
            JUMP = true;
        else if (s == "PORTAL")
            PORTAL = true;
        else if (s == "ENEMY-")
        {
            ENEMY = false;
        }
        else if (s == "KEY")
            hasKey = true;

        closeCheatBox();
    }
    public void addLetter(string a)
    {
        cheatText = cheatText.Insert(textPos, a);
        textPos++;
        Debug.Log(textPos);
    }
    public void removeLetter()
    {
        textPos--;
        cheatText = cheatText.Remove(textPos);
        
    }
    /* PAUSE MENU FUNCTIONS */
    public void openPauseMenu()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
        disableOthers();
    }
    public void closePauseMenu()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        activeOthers();
    }
    void disableOthers()
    {
        foreach (Button b in otherButtons)
            b.interactable = false;
    }
    void activeOthers()
    {
        foreach (Button b in otherButtons)
            b.interactable = true;
    }
    public void resumeGame()
    {
        closePauseMenu();
    }
    public void goMainMenu()
    {
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void restartLevel()
    {
        resetCheatVariables();
        resetLevelVariables();
        closePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void quitGame()
    {
        Application.Quit();
    }
    /* TUTORIAL for LEVEL1 */
    public void openTutMenu()
    {
        tutorialMenu.SetActive(true);
        disableOthers();
        isPaused = true;
        Time.timeScale = 0f;
        tutorial(tutIndex);
    }
    public void closeTutMenu()
    {
        tutorialMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        activeOthers();
        tutIndex++;
    }
    public void tutorial(int i)
    {
        tutTxt.text = tuts[i];
    }
}
