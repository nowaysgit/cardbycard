using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    public override float Health { get { return health; } set { health = value; game.textHealth.text = Convert.ToString(Mathf.Round(health)); game.sliderHealth.value = health; } }
    public override float HealthMax { get { return healthMax; } set { healthMax = value; } }
    public override float Mana { get { return mana; } set { mana = value; game.textMana.text = Convert.ToString(Mathf.Round(mana)); game.sliderMana.value = mana; } }
    public override float ManaMax { get { return manaMax; } set { manaMax = value; } }
    public int Money { get { return money; } set { money = value; game.textMoney.text = Convert.ToString(money); } }
    public float Damage { get { return damage; } set { damage = value; } }
    public float Shield { get { return shield; } set { shield = value; } }

    public GameObject AttackFXPrefab;

    [HideInInspector]
    private int money = 0;
    private float damage = 1.0f;
    private float shield = 0.0f;
    public float ManaCost = 0.0f;
    public string spriteName;
    public SpriteRenderer sprite;
    public InventoryPlayer inventory;
    public InventoryItems inventoryItems;


    private Vector2Int possition = new Vector2Int (0,0);
    private GameController game;
    private Controls controls;
    private bool Alive;

    private bool isInMoved;


    public void Load(int _health, int _mana, int _money, float _damage)
    {
        Health = _health;
        Mana = _mana;
        Money = _money;
        Damage = _damage;
        Alive = true;
    }
    private void Awake() 
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controls = new Controls();
        inventory = gameObject.GetComponent<InventoryPlayer>();
        inventoryItems = gameObject.GetComponent<InventoryItems>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        game.inventoryUIItems.Load();
    }
    private void OnEnable() {
        if (controls != null) controls.Enable();
    }
    private void OnDisable() {
        if (controls != null) controls.Disable();
    }

    public void Move(int x, int y)
    {
        if(x != 0 && y != 0) return;
        if(!Alive) return;
        if(isInMoved) return;
        int tryPossitionX = possition.x + x;
        int tryPossitionY = possition.y + y;
        if ((x != 0 && tryPossitionX >= 0 && tryPossitionX < 3) || (y != 0 && tryPossitionY >= 0 && tryPossitionY < 3)) 
        {
            if (game.field[tryPossitionY, tryPossitionX] == null) return; // X AND Y REVERSED <WHY>
            float getdamage;
            bool canmove;
            game.field[tryPossitionY, tryPossitionX].Event(damage, out getdamage, out canmove); // X AND Y REVERSED <WHY>
            sprite.sprite = Resources.Load<Sprite>(spriteName);
            if (canmove)
            {
                isInMoved = true;
                if(game.field[tryPossitionY, tryPossitionX].Info.type == "Loot" || game.field[tryPossitionY, tryPossitionX].Info.type == "Shop") { sprite.sprite = Resources.Load<Sprite>(spriteName+"Back"); }
                else { sprite.sprite = Resources.Load<Sprite>(spriteName); game.inventoryUILoot.gameObject.SetActive(false); }

                if(game.field[possition.y, possition.x])
                {
                    game.field[possition.y, possition.x].Die();
                    game.MakeCard(possition.y, possition.x, new Vector2(transform.position.x, transform.position.y));
                }
                ChangePossition(x,y);
            }
            if (getdamage > 0)
            {
                SetDamage(getdamage);
            }
        }
    }
    void ChangePossition(int x, int y)
    {
        if(!Alive) return;
        if (game.field[possition.x, possition.y] == null) {game.MakeCard(possition.x, possition.y, new Vector2(transform.position.x, transform.position.y)); }
        var tryChangeX = (transform.position.x + (x*2.2f));
        var tryChangeY = (transform.position.y + (y*2.2f));
        if (x != 0) 
        {
            StartCoroutine(AnimateMove(new Vector3(tryChangeX, transform.position.y, -1), 250f));
            possition.x+=x;
        } else 
        {
            StartCoroutine(AnimateMove(new Vector3(transform.position.x, tryChangeY, -1), 250f));
            possition.y+=y;
        }
        game.playerPossition = possition;
        game.isMoved();
    }
    private IEnumerator AnimateMove(Vector3 targetpos, float speed)
    {
        while (gameObject && Vector3.Distance(gameObject.transform.position, targetpos) > 0.01f)
        {
            if(!gameObject) yield break;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetpos, Time.deltaTime * speed);

            yield return new WaitForSeconds(0.02f);
        }
        transform.position = targetpos;
        isInMoved = false;
    }
    void SetDamage(float getdamage)
    {
        GameObject fxAttack = (GameObject)Instantiate(AttackFXPrefab, transform.position, Quaternion.identity) as GameObject;
        if(!Alive) return;
        if (Shield > 0.00f) {
            float damaged = Convert.ToSingle(Math.Round(getdamage*Shield, 2)); 
            Health = Convert.ToSingle(Math.Round(Health-(getdamage-damaged), 2));  
            Mana = Convert.ToSingle(Math.Round(Mana-ManaCost, 2));
            fxAttack.GetComponent<TextMesh>().text = Convert.ToString((getdamage-damaged) +"("+ damaged +")");
        }
        else
        {
            Health-= getdamage;  
            fxAttack.GetComponent<TextMesh>().text = Convert.ToString(getdamage);
        }
        if (Health<=0) 
        {
            Health = 0;
            Alive = false;
            game.diePanel.SetActive(true);
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    void AbilityUse()
    {
        if(!Alive) return;
    }
}
