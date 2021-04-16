using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardBehaviour : MonoBehaviour
{
    public string playerName;
    public int score;
    public string finalPos;

    TextMeshProUGUI thisScoreTM;


    private void Awake()
    {
        print(PlayerPrefs.GetString("name" + finalPos));
        print(PlayerPrefs.GetString("score" + finalPos));
        thisScoreTM = this.gameObject.GetComponent<TextMeshProUGUI>();
        writeInfo();
    }

    public void setInfo(string name, int finalScore)
    {

        PlayerPrefs.SetString("name" + finalPos, name);
        PlayerPrefs.SetInt("score" + finalPos, finalScore);
    }

    void writeInfo()
    {
        thisScoreTM.text = finalPos + ": " + PlayerPrefs.GetString("name" + finalPos) + " " + PlayerPrefs.GetInt("score" + finalPos).ToString();
    }

}