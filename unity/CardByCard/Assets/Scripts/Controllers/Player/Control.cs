using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

[RequireComponent(typeof(SpriteRenderer))]

public class Control : MonoBehaviour
{
    private Controls controls;
    public Interactive interactive;
    private ControllerPlayer player;
    protected Vector2Int possition = new Vector2Int(0, 0);
    private bool isInMoved;

    private void Awake()
    {
        interactive = gameObject.GetComponent<Interactive>();
        player = gameObject.GetComponent<ControllerPlayer>();
    }
    public void TouchEventStart(Vector2 vector) // LEAN
    {
        var normalized = vector.normalized;
        TryMove(Convert.ToInt32(normalized.x), Convert.ToInt32(normalized.y));
    }
    private void OnEnable()
    {
        if (controls != null) controls.Enable();
    }
    private void OnDisable()
    {
        if (controls != null) controls.Disable();
    }
    public void TryMove(int x, int y)
    {
        if (!interactive.Alive) return;
        if (isInMoved) return;
        int tryPossitionX = possition.x + x;
        int tryPossitionY = possition.y + y;
        if (((x != 0 && tryPossitionX >= 0 && tryPossitionX < 3) || (y != 0 && tryPossitionY >= 0 && tryPossitionY < 3)) == false) return;
        float getdamage;
        bool canmove = Game.singletone.GameStateInGame.CanMove(x, y, possition, out getdamage);
        if (canmove)
        {
            isInMoved = true;
            Game.singletone.GameStateInGame.TryMakeNewCard(possition, transform.position);
            ChangePossition(x, y);
        }
        else if (getdamage > 0)
        {
            player.SetDamage(getdamage);
        }

    }
    private void ChangePossition(int _x, int _y)
    {
        if (!interactive.Alive) return;

        var tryChangeX = (transform.position.x + (_x * 2.2f));
        var tryChangeY = (transform.position.y + (_y * 2.2f));
        if (_x != 0)
        {
            StartCoroutine(AnimateMove(new Vector3(tryChangeX, transform.position.y, -1), 250f));
            possition.x += _x;
        }
        else
        {
            StartCoroutine(AnimateMove(new Vector3(transform.position.x, tryChangeY, -1), 250f));
            possition.y += _y;
        }
        Game.singletone.GameStateInGame.PlayerPossition = possition;
        player.NumMoves++;
        Game.singletone.OnPlayerMoved.Invoke();
    }
    public void SetPossition(int x, int y)
    {
        var tryChangeX = (x * 2.2f);
        var tryChangeY = (y * 2.2f);
        transform.position = new Vector3(tryChangeX, tryChangeY, -1);
        possition.x = x;
        possition.y = y;
        Game.singletone.GameStateInGame.PlayerPossition = possition;
    }
    private IEnumerator AnimateMove(Vector3 _targetpos, float _speed)
    {
        while (gameObject && Vector3.Distance(gameObject.transform.position, _targetpos) > 0.01f)
        {
            if (!gameObject) yield break;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _targetpos, Time.deltaTime * _speed);

            yield return new WaitForSeconds(0.02f);
        }
        transform.position = _targetpos;
        isInMoved = false;
    }
}
