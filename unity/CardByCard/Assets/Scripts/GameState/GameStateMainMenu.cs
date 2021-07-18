using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class GameStateMainMenu : GameState
{
    public GameObject Menu;
    public override void Enter()
    {
        Menu.SetActive(true);
    }
    public override void Exit()
    {
        Menu.SetActive(false);
    }
}
