using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public UIBackpack UIBackpack;
    
    public void OnGameRestart()
    {
        UIInventory.UpdateInventory("Clear");
        UILoot.UpdateInventory("Clear");
        UIBackpack.ReloadAll();
        DiePanel.SetActive(false);
    }
    public void OnPlayerRespawn()
    {
        DiePanel.SetActive(false);
    }
}
