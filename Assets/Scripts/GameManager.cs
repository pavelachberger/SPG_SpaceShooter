using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject player;
    public float spawnRate;
    public string playerName;

    public bool gameStarted = false;
    public bool animFinished = true;
    bool isPaused = false;

    //UI
    public GameObject scoreObj;
    public GameObject hpObj;
    public GameObject nameObj;
    public GameObject pauseObj;
    TextMeshProUGUI scoreTM;
    TextMeshProUGUI healthTM;
    TextMeshProUGUI playerNameTM;
    public int score = 0;

    HighscoreTable highScoreTable;
    List<string> leaderBoardData = new List<string>();


    private void Start()
    {
        scoreTM = scoreObj.GetComponent<TextMeshProUGUI>();
        healthTM = hpObj.GetComponent<TextMeshProUGUI>();
        playerNameTM = nameObj.GetComponent<TextMeshProUGUI>();

        highScoreTable = this.gameObject.GetComponent<HighscoreTable>();

        changeText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pauseGame();
            Debug.Log("pause initiated");
        }
        //Ship movement after starting game
        if (gameStarted)
        {
            if (player.transform.position == new Vector3(0, -4, 0))
            {
                animFinished = true;
            }
            if (!animFinished)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, -4, 0), 0.01f);
            }
        }
    }

    //Settings needed to start gameplay
    public void startGame()
    {
        changeText();
        playerName = playerNameTM.text;
        gameStarted = true;
        animFinished = false;

        StartCoroutine(SpawnObstacles());

        Debug.Log("started");
    }

    void pauseGame()
    {
        if (gameStarted)
        {
            if (!isPaused)
            {
                pauseObj.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;

            }
            else
            {
                pauseObj.SetActive(false);
                Time.timeScale = 1;
                isPaused = false;
            }
        }
    }

    public void endGame()
    {
        highScoreTable.AddHighscoreEntry(score, playerName);
        SceneManager.LoadScene("Gameplay");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    //Changing HUD, initialized after every change
    public void changeText()
    {
        scoreTM.text = "SCORE: " + score.ToString();
        healthTM.text = player.GetComponent<PlayerBehaviour>().playerHp.ToString() + "x ";
    }

    //Enemy spawner, "spawnRate" dictates the cycle length
    IEnumerator SpawnObstacles()
    {
        Instantiate(obstacle, new Vector2(Random.Range(-3f, 3f), 6), Quaternion.identity);
        yield return new WaitForSeconds(spawnRate);
        if (spawnRate > 1)
        {
            spawnRate *= 0.98f;
        }
        StartCoroutine(SpawnObstacles());
    }
}