using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int coins {get; private set;}

    private string coinsKey = "Coins";
    private string recordKey = "Record";
    private string levelKey = "Level";
    public int currentCoins {get; private set;}
    public int currentRecord {get; private set;}
    public int level {get; private set;}
    private int points = 0;
    private float delayLevelTime = 60f;

    [SerializeField]
    private TextMeshProUGUI coinsText;
    [SerializeField]
    private TextMeshProUGUI pointsText;
    [SerializeField]
    private TextMeshProUGUI recordText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Button restart;
    [SerializeField]
    private Button menu;

    [SerializeField]
    private AudioClip Click;
    private AudioSource audioSource;


    float currentTime = 0f;

    private void Awake(){
        Time.timeScale = 1;
        level = 0;
        points = 0;
        currentCoins = PlayerPrefs.GetInt(coinsKey);
        currentRecord = PlayerPrefs.GetInt(recordKey);
        Debug.Log("Current Record: " + currentRecord);
        coins = currentCoins;
        coinsText.text = "Coins: " + currentCoins;
        recordText.text = "Highest Score: " + currentRecord;
    }

    private void Start(){
        restart.onClick.AddListener(RestartLevel);
        menu.onClick.AddListener(menuReturn);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update(){
        currentTime += Time.deltaTime;

        if(currentTime >= delayLevelTime){
            this.level += 1;
            levelText.text = "Level: " + level;
            currentTime = 0f;
            PlayerPrefs.SetInt(levelKey, level);
        }
    }

    public int ReturnLevel(){
        return this.level;
    }

    public void AddCoins(int value){
        this.coins += value;
        coinsText.text = "Coins: " + this.coins;
    }    

    public void AddPoints(int value){
        if(level == 0){
            points += value * 1;
        }else{
            points += value * level;
        }
        if(value == 100) this.AddCoins(10);
        if(value == 300) this.AddCoins(30);
        if(value == 500) this.AddCoins(50);
        if(value == 800) this.AddCoins(100);
        
        pointsText.text = "Points: " + points;
        if(points > currentRecord){
            recordText.text = "Highest Score: " + points;
            PlayerPrefs.SetInt(recordKey, points);
        }
    }

    public void GameOver(){
        this.gameOverPanel.SetActive(true);
        PlayerPrefs.SetInt(coinsKey, coins);
        this.points = 0;
        Time.timeScale = 0;
    }

    private void RestartLevel(){
        this.gameOverPanel.SetActive(false);
        if(audioSource != null){
            audioSource.clip = Click;
            audioSource.Play();
        }
        PlayerPrefs.SetInt(coinsKey, coins);
        if(currentRecord < points){
            PlayerPrefs.SetInt(recordKey, points);
            recordText.text = "Highest Score: " + points;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void menuReturn(){
        if(audioSource != null){
            audioSource.clip = Click;
            audioSource.Play();
        }
        SceneManager.LoadScene("Menu");
    }

}
