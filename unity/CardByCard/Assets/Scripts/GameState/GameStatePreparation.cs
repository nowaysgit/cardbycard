using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class GameStatePreparation : GameState
{
    public GameObject Menu;
    public override void Enter()
    {
        Menu.SetActive(true);
        Game.singletone.OnGamePreparation.Invoke();
    }
    public override void Exit()
    {
        Menu.SetActive(false);
    }
}
