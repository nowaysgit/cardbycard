using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPreparationMenu : MonoBehaviour
{
    [SerializeField] private GameObject UIEquipment;
    [SerializeField] private GameObject UIShop;
    
    public void Open(int uiId)
    {
        if (uiId == 1)
        {
            UIEquipment.SetActive(!UIEquipment.activeSelf);
            UIShop.SetActive(false);
        }
        else
        {
            UIShop.SetActive(!UIShop.activeSelf);
            UIEquipment.SetActive(false);
        }
    }
}
