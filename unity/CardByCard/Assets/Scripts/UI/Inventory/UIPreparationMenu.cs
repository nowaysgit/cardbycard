using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPreparationMenu : MonoBehaviour
{
    [SerializeField] private GameObject UIMap;
    [SerializeField] private GameObject UIEquipment;
    [SerializeField] private GameObject UIShop;
    
    public void Open(int uiId)
    {
        if (uiId == 1)
        {
            UIMap.SetActive(!UIMap.activeSelf);
            UIShop.SetActive(false);
            UIEquipment.SetActive(false);
        }
        else if (uiId == 2)
        {
            UIEquipment.SetActive(!UIEquipment.activeSelf);
            UIShop.SetActive(false);
            UIMap.SetActive(false);
        }
        else
        {
            UIShop.SetActive(!UIShop.activeSelf);
            UIEquipment.SetActive(false);
            UIMap.SetActive(false);
        }
    }
}
