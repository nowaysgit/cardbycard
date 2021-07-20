using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

[System.Serializable]
public class InfoItem
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
    public string attackMethod;
    public string attackArea;
    public int kd;
    public string spriteName;
    public int cost;
    public int lvl;
    public string description;
}
[System.Serializable]
public class InfoCard
{
    public bool spawned;
    public int id;
    public string title;
    public string type;
    public string color;
    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;
    public int size;
    public float damage;
    public string spriteName;
    public int lvl;
    public bool canMove;
}
[System.Serializable]
public class Data
{
    [SerializeField] private InfoCard[] enemyList;
    [SerializeField] private InfoCard[] lootList;
    [SerializeField] private InfoCard[] blockList;
    [SerializeField] private InfoCard[] shopList;
    [SerializeField] private InfoCard[] emptyList;
    [SerializeField] private InfoItem[] itemList;
    
    public InfoCard[] EnemyList { get { return enemyList; } }
    public InfoCard[] LootList { get { return lootList; } }
    public InfoCard[] BlockList { get { return blockList; } }
    public InfoCard[] ShopList { get { return shopList; } }
    public InfoCard[] EmptyList { get { return emptyList; } }
    public InfoItem[] ItemList { get { return itemList; } }

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