using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] int maxEnergy;
    [SerializeField] int energyRechargeDuration;
    [SerializeField] TMP_Text energyText;
    [SerializeField] Button playButton;
    [SerializeField] AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] IOSNotificationHandler iosNotificationHandler;

    [SerializeField] AudioSource engineStart;

    int energy;

    const string EnergyKey = "Energy";
    const string EnergyReadyKey = "EnergyReady";

    void Start()
    {
        OnApplicationFocus(true);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) { return; }

        CancelInvoke();

        int score = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = "High Score: " + score;

        //Energy Handling
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if (energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            if (energyReadyString == string.Empty)
            {
                return;
            }

            //Reads time when energy will be ready and compares to current time
            DateTime energyReady = DateTime.Parse(energyReadyString); 

            if (DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            else
            {
                playButton.interactable = false;
                Invoke("RefillEnergy", (energyReady - DateTime.Now).Seconds);
            }
        }

        energyText.text = "Play (" + energy + ")";
    }

    void RefillEnergy()
    {
        playButton.interactable = true;
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = "Play (" + energy + ")";
    }

    public void PlayGame()
    {
        if (energy >= 1)
        {
            
            engineStart.Play();

            energy--;
            PlayerPrefs.SetInt(EnergyKey, energy);

            if (energy == 0)
            {
                DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
                PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());

                #if UNITY_ANDROID
                    androidNotificationHandler.ScheduleNotification(energyReady); //Wants date and time
                #elif UNITY_IOS
                    iosNotificationHandler.ScheduleNotification(energyRechargeDuration); //Wants time in hours minutes seconds
                #endif
            }

            Invoke("LoadTrack", 1f);
            
        }
    }

    void LoadTrack()
    {
        SceneManager.LoadScene(1);
    }
}
