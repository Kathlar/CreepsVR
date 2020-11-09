using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInfoWindow : MonoBehaviour
{
    public Image playerIconImage;
    public Text playerNameText;

    public Image healthBarImage;
    public Text healthBarText;

    public void SetUp(int playerNumber)
    {
        playerIconImage.color = Database.PlayerInfos[playerNumber].color;
        playerNameText.color = playerIconImage.color;
        playerNameText.text = "PLAYER " + (playerNumber + 1).ToString();
        UpdateHealthBar(100, 100);
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth / (float)maxHealth;
        healthBarText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }
}
