using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game = GameManager;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    public Text TextHealth;
    public Text TextMana;
    public Text TextMoney;
    public Slider SliderHealth;
    public Slider SliderMana;
    public GameObject DiePanel;
    public UIAnimator UIAnimator;

    [Header("Inventory UI")]
    public UIInventory UIInventory;
    public UILoot UILoot;

    [Header("MainMenu")]
    public GameObject MainMenu;
    
    public void OnGameRestart()
    {
        UIInventory.UpdateInventory("Clear");
        UILoot.UpdateInventory("Clear");
        DiePanel.SetActive(false);
    }
    public void OnPlayerRespawn()
    {
        SliderHealth.maxValue = Game.singletone.Player.HealthMax;
        SliderMana.maxValue = Game.singletone.Player.ManaMax;
        SliderHealth.value = Game.singletone.Player.HealthMax;
        SliderMana.value = Game.singletone.Player.ManaMax;
        TextHealth.text = Game.singletone.Player.HealthMax.ToString();
        TextMana.text = Game.singletone.Player.ManaMax.ToString();
        DiePanel.SetActive(false);
    }
    public void UpdateHealthBar(float amount, float amount2)
    {
        TextHealth.text = Convert.ToString(Mathf.Round(amount)); 
        SliderHealth.value = amount; 
    }
    public void UpdateManaBar(float amount, float amount2)
    {
        TextMana.text = Convert.ToString(Mathf.Round(amount)); 
        SliderMana.value = amount; 
    }
    public void UpdateMoney(int amount)
    {
        TextMoney.text = Convert.ToString(amount);
    }
}
