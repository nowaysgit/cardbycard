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
public class InfoLocation
{
    public int id;
    public string title;
    public string type;
    public string dungeon;
    public string description;
    public int distance;
    public int lvl;
    public int uniqueId;
    public float uniquePosX;
    public float uniquePosY;
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
    [SerializeField] private InfoLocation[] locationList;

    public InfoCard[] EnemyList { get { return enemyList; } }
    public InfoCard[] LootList { get { return lootList; } }
    public InfoCard[] BlockList { get { return blockList; } }
    public InfoCard[] ShopList { get { return shopList; } }
    public InfoCard[] EmptyList { get { return emptyList; } }
    public InfoItem[] ItemList { get { return itemList; } }
    public InfoLocation[] LocationList { get { return locationList; } }

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
public static class ObjectExtensions
{
    #region Methods

    public static T Copy<T>(this T source)
    {
        var isNotSerializable = !typeof(T).IsSerializable;
        if (isNotSerializable)
            throw new ArgumentException("The type must be serializable.", "source");

        var sourceIsNull = ReferenceEquals(source, null);
        if (sourceIsNull)
            return default(T);

        var formatter = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }

    #region LookAt2D
    public static void LookAt2D(this Transform me, Vector2 eye, Vector2 target)
    {
        Vector2 look = target - (Vector2)me.localPosition;

        float angle = Vector2.Angle(eye, look);

        Vector2 right = Vector3.Cross(Vector3.forward, look);

        int dir = 1;

        if (Vector2.Angle(right, eye) < 90)
        {
            dir = -1;
        }

        me.rotation *= Quaternion.AngleAxis(angle * dir, Vector3.forward);
    }

    public static void LookAt2D(this Transform me, Vector2 eye, Transform target)
    {
        me.LookAt2D(eye, target.localPosition);
    }

    public static void LookAt2D(this Transform me, Vector2 eye, GameObject target)
    {
        me.LookAt2D(eye, target.transform.localPosition);
    }
    #endregion

    #endregion
}