using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject timer;

    public void AddScore(float remainingTime)
    {
        //float remainingTime = timer.GetComponent<TimerScript>().timeRemaining;
        string scoreStr = scoreText.text;
        int score = int.Parse(Regex.Match(scoreStr, @"\d+").Value);
        score += (int)remainingTime;
        scoreText.text = string.Format("${0}", score);
    }
}
