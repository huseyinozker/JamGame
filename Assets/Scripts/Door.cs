using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public int newIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (GameManager.instance.hasKey)
            {
                gameObject.GetComponent<Animator>().SetBool("open", true);
            }
        }
    }
    public void goNextLevel()
    {
        GameManager.instance.resetLevelVariables();
        GameManager.instance.resetCheatVariables();
        if(newIndex<5)
            PlayerPrefs.SetInt("MaxLevel", newIndex);
        SceneManager.LoadScene(newIndex);
    }
}
