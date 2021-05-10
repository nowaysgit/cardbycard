using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

[System.Serializable]
public class ItemInfo
{
    public bool spawned;
    public int id;
    public string title;
    public bool stackable;
    public int maxInStack;
    public float damage;
    public float manaCost;
    public string type;
    public string typeAbility;
    public string spriteName;
    public int cost;
    public int lvl;
}
[System.Serializable]
public class CardInfo
{
    public bool spawned;
    public int id;
    public string title;
    public string type;
    public string color;
    public float health;
    public float maxHealth;
    public float mana;
    public int size;
    public float damage;
    public string spriteName;
    public int lvl;
}
[System.Serializable]
public class Data
{
    [SerializeField] 
    private CardInfo[] enemyList;
    [SerializeField] 
    private CardInfo[] lootList;
    [SerializeField] 
    private CardInfo[] blockList;
    [SerializeField] 
    private CardInfo[] shopList;
    [SerializeField] 
    private CardInfo[] emptyList;
    [SerializeField] 
    private ItemInfo[] itemList;
    
    public CardInfo[] EnemyList { get { return enemyList; } }
    public CardInfo[] LootList { get { return lootList; } }
    public CardInfo[] BlockList { get { return blockList; } }
    public CardInfo[] ShopList { get { return shopList; } }
    public CardInfo[] EmptyList { get { return emptyList; } }
    public ItemInfo[] ItemList { get { return itemList; } }

    public T CopyFromSerialize<T>(T SerializableObject) where T : new()
    {
        try
        {
            MemoryStream stream = new MemoryStream();

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, SerializableObject);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
        catch (Exception ex)
        {
            return new T();
        }
    }
}