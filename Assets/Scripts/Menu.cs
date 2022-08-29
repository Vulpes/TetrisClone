using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{

    [SerializeField]
    private Button play;
    [SerializeField]
    private Button quit;
    [SerializeField]
    private Button howToPlay;
    [SerializeField]
    private Button closeHowToPlay;

    [SerializeField]
    private TextMeshProUGUI highscore;
    [SerializeField]
    private TextMeshProUGUI coinsText;

    [SerializeField]
    private GameObject PanelHowToPlay;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip Click;
    [SerializeField]
    private AudioClip Close;

    private string coinsKey = "Coins";
    private string recordKey = "Record";
    private int coins;
    private int score;

    
    void Start()
    {
        play.onClick.AddListener(tetris);
        quit.onClick.AddListener(quitGame);
        howToPlay.onClick.AddListener(openPanel);
        closeHowToPlay.onClick.AddListener(closePanel);

        coins = PlayerPrefs.GetInt(coinsKey);
        score = PlayerPrefs.GetInt(recordKey);
        highscore.text = "Highest Score: " + score;
        coinsText.text = "Coins: " + coins;
    }


    private void tetris(){
        if(audioSource != null){
            audioSource.clip = Click;
            audioSource.Play();
        }
        SceneManager.LoadScene("Tetris");
    }

    private void quitGame(){
        if(audioSource != null){
            audioSource.clip = Click;
            audioSource.Play();
        }
        Application.Quit();
    }

    private void openPanel(){
        if(audioSource != null){
            audioSource.clip = Click;
            audioSource.Play();
        }
        PanelHowToPlay.SetActive(true);
    }

    private void closePanel(){
        if(audioSource != null){
            audioSource.clip = Close;
            audioSource.Play();
        }
        PanelHowToPlay.SetActive(false);
    }
}
