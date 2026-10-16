using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuScript : MonoBehaviour {
    public GameObject mainMenuPanel;
    public GameObject levelMenuPanel;
    private int maxlevel = 3;

    void Start() {
        if (!PlayerPrefs.HasKey("highest_level")) {
            PlayerPrefs.SetInt("highest_level", 0);
            PlayerPrefs.Save();
        }
    }

    private void nastavLevelMenu() {
        levelMenuPanel.SetActive(true);
        int highestLevel = PlayerPrefs.GetInt("highest_level", 0);
        
        for (int i = 2; i <= maxlevel; i++) {
            GameObject myObject = GameObject.Find("b" + i + "off");
            if (myObject != null) {
                if (i <= highestLevel + 1) {
                    myObject.SetActive(false); 
                } else {
                    myObject.SetActive(true); 
                }
            } else {
                Debug.Log("b" + i + "off - not found");
            }
        }
    }

    public void spracujKliknutie(int buttonNo) {
        Debug.Log("Button clicked = " + buttonNo);
        SceneManager.LoadScene("level" + buttonNo);
    }

    public void zobrazMenuLevel() {
        mainMenuPanel.SetActive(false);
        nastavLevelMenu();
    }

    public void zobrazMenuMain() {
        levelMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void GameOver() {
        Application.Quit();
        Debug.Log("Hra skončila");
    }
    public void ResetLevels() {
        PlayerPrefs.SetInt("highest_level", 0);
        PlayerPrefs.Save();
        Debug.Log("Levely boli resetované.");
        nastavLevelMenu();
        zobrazMenuMain();
    }
}


