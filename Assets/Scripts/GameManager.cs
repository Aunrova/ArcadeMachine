using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Threading;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Frogger frogger;
    public Home[] homes;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverMenu;
    private int time;
    public int score = 0;
    public int live;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        frogger = FindObjectOfType<Frogger>();
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + live;
    }
    
    private void NewGame()
    {
        gameOverMenu.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewLevel();
        StartCoroutine(Timer(30));
    }

    public void HomeOccupied(){
        frogger.gameObject.SetActive(false);
        int bonusPoints = time*10;
        SetScore(score + 600 + bonusPoints);
        if(Cleared())
        {
            SetScore(score + 2000);
            Invoke(nameof(NewLevel), 1f);
        }
        else
        {
            Invoke(nameof(Respawn), 1f);
        }
    }

    private bool Cleared(){
        for (int i = 0; i < homes.Length; i++)
        {
            if (homes[i].enabled == false)
            {
                return false;
            }
        }
        return true;
    }


    private void Respawn(){
        frogger.Respawn();

        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    private System.Collections.IEnumerator Timer(int duration)
    {
        time = duration;
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time--;
        }
        frogger.Death();
    }

    public void AdvancedRow(){
        SetScore(score + 10);
    }

    private void NewLevel()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        Respawn();
    }
    
    public void SetScore(int newScore)
    {
        score = newScore;
        scoreText.text = "Score: " + score;
    }

    public void Died(){
        live--;
        if(live > 0)
        {
            Invoke(nameof(Respawn), 1f);
        }else{
            Invoke(nameof(GameOver), 1f);
        }
    }

    private void GameOver()
    {
        frogger.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(CheckForPlayAgain());
    }
 
    private System.Collections.IEnumerator CheckForPlayAgain()
    {
        bool playAgain = false;

        while(!playAgain)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                playAgain = true;
            }
            yield return null;
        }
        NewGame();
    }

    private void SetLives(int newLives)
    {
        live=newLives;
    }
}
