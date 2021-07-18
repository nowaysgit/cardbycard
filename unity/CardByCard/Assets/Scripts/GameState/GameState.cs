using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class GameState : MonoBehaviour
{
    public GameState NextState;
    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }
}
