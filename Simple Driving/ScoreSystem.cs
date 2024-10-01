using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text speedometer;
    [SerializeField] Car car;

    float score;

    public const string HighScoreKey = "HighScore";

   
    void Update()
    {
        score += car.Speed * Time.deltaTime;

        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        speedometer.text = Mathf.FloorToInt(car.Speed).ToString() + "MPH";
    }

    private void OnDestroy()
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
    }
}
