using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

[RequireComponent(typeof(SpriteRenderer))]

public class Control : MonoBehaviour
{
    private Controls controls;
    private Interactive interactive;
    private ControllerPlayer player;
    protected Vector2Int possition = new Vector2Int (0,0);
    private bool isInMoved;
    private SpriteRenderer sprite;
    public string SpriteName;

    private void Awake() 
    {
        interactive = gameObject.GetComponent<Interactive>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        player = gameObject.GetComponent<ControllerPlayer>();
    }    
    public void TouchEventStart(Vector2 vector) // LEAN
    {
        var normalized = vector.normalized;
        Move(Convert.ToInt32(normalized.x), Convert.ToInt32(normalized.y));
    }
    private void SetSprite(string _spritename)
    {
        sprite.sprite = Resources.Load<Sprite>(_spritename);
    }
    private void OnEnable() {
        if (controls != null) controls.Enable();
    }
    private void OnDisable() {
        if (controls != null) controls.Disable();
    }
    public void Move(int _x, int _y)
    {
        if(_x != 0 && _y != 0) return;
        if(!interactive.Alive) return;
        if(isInMoved) return;
        int tryPossitionX = possition.x + _x;
        int tryPossitionY = possition.y + _y;
        if ((_x != 0 && tryPossitionX >= 0 && tryPossitionX < 3) || (_y != 0 && tryPossitionY >= 0 && tryPossitionY < 3)) 
        {
            if (Game.ControllerField.Field[tryPossitionY, tryPossitionX] == null) return; // X AND Y REVERSED <WHY>
            float getdamage;
            bool canmove;
            Game.ControllerField.Field[tryPossitionY, tryPossitionX].Event(interactive.Damage, out getdamage, out canmove); // X AND Y REVERSED <WHY>
            SetSprite(SpriteName);
            if (canmove)
            {
                isInMoved = true;
                if(Game.ControllerField.Field[tryPossitionY, tryPossitionX].Info.type == "Loot" || Game.ControllerField.Field[tryPossitionY, tryPossitionX].Info.type == "Shop") { SetSprite(SpriteName+"Back"); }
                else { SetSprite(SpriteName); Game.UIManager.UIBackpack.gameObject.SetActive(false); }

                if(Game.ControllerField.Field[possition.y, possition.x])
                {
                    Game.ControllerField.Field[possition.y, possition.x].Die();
                    Game.FactoryCard.Make(possition.y, possition.x, new Vector2(transform.position.x, transform.position.y));
                }
                ChangePossition(_x,_y);
            }
            else if (getdamage > 0)
            {
                player.SetDamage(getdamage);
            }
        }
    }
    void ChangePossition(int _x, int _y)
    {
        if(!interactive.Alive) return;
        if (Game.ControllerField.Field[possition.x, possition.y] == null) { Game.FactoryCard.Make(possition.x, possition.y, new Vector2(transform.position.x, transform.position.y)); }
        var tryChangeX = (transform.position.x + (_x*2.2f));
        var tryChangeY = (transform.position.y + (_y*2.2f));
        if (_x != 0) 
        {
            StartCoroutine(AnimateMove(new Vector3(tryChangeX, transform.position.y, -1), 250f));
            possition.x+=_x;
        } else 
        {
            StartCoroutine(AnimateMove(new Vector3(transform.position.x, tryChangeY, -1), 250f));
            possition.y+=_y;
        }
        Game.ControllerField.PlayerPossition = possition;
        Game.ControllerField.OnMoved.Invoke();
    }
    private IEnumerator AnimateMove(Vector3 _targetpos, float _speed)
    {
        while (gameObject && Vector3.Distance(gameObject.transform.position, _targetpos) > 0.01f)
        {
            if(!gameObject) yield break;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _targetpos, Time.deltaTime * _speed);

            yield return new WaitForSeconds(0.02f);
        }
        transform.position = _targetpos;
        isInMoved = false;
    }
}
