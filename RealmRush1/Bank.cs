using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } } //Set-up public variable

    [SerializeField] TextMeshProUGUI displayBalance;

    void Awake()
    {
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount); //Deposits absolute amount to prevent errors
        UpdateDisplay();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount); //Withdraws absolute amount to prevent errors
        UpdateDisplay();

        if (currentBalance < 0) //Lose Condition
        {
            ReloadScene();
        }

    }

    void UpdateDisplay()
    {
        displayBalance.text = "Gold: " + currentBalance; //Updates Gold UI
    }

    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get current scene index

        SceneManager.LoadScene(currentScene.buildIndex); // Reload scene
    }
}
