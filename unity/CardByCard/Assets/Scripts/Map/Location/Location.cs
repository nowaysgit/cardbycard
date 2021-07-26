using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
    public InfoLocation Info;
    public Location PreviousLocation;
    public Location[] NextLocation;
    public bool IsPassed;
    public bool IsMovable;
    public GameObject compliteImage;
}
