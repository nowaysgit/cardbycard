using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class Map : MonoBehaviour
{
    //NEED REFACTORING
    [SerializeField] public Location curretLocation;
    [SerializeField] public Location previousLocation;
    [SerializeField] public Location targetLocation;
    public void Load()
    {
        Game.singletone.OnCompliteLocation.AddListener(CompliteLocation);
        Game.singletone.OnChooseLocation.AddListener(ChooseLocation);
    }
    public void ChooseLocation(Location Location)
    {
        if(!Location.IsPassed)
        {
            targetLocation = Location;
        }
    }

    public void CompliteLocation()
    {
        curretLocation.IsPassed = true;
        curretLocation.compliteImage.SetActive(true);
        targetLocation.IsMovable = true;
        previousLocation = curretLocation;
        curretLocation = targetLocation;
        targetLocation = curretLocation.NextLocation[0];

        var newloc = Game.FactoryLocation.MakeInfo(targetLocation.Info);
        newloc.dungeon = RandomDangeon(targetLocation.Info);
        var rand = Random.Range(0, 100);
        if (rand < 25) // GENERATE 2 WAY
        {
            targetLocation.NextLocation = new Location[2];
        }

        targetLocation.NextLocation[0] = Game.UIManager.UIMap.AddNew(curretLocation, targetLocation, targetLocation.NextLocation[0], Game.Data.CopyFromSerialize<InfoLocation>(newloc));

        if (rand < 25) // GENERATE 2 WAY
        {
            var newloc2 = Game.FactoryLocation.MakeInfo(targetLocation.Info);
            newloc2.dungeon = RandomDangeon(targetLocation.Info);
            targetLocation.NextLocation[1] = Game.UIManager.UIMap.AddNew(curretLocation, targetLocation, targetLocation.NextLocation[1], Game.Data.CopyFromSerialize<InfoLocation>(newloc2));
        }
        if (curretLocation.NextLocation.Length > 1 && curretLocation.NextLocation[1])
        {
            var newloc3 = Game.FactoryLocation.MakeInfo(curretLocation.NextLocation[1].Info);
            newloc3.dungeon = RandomDangeon(curretLocation.NextLocation[1].Info);
            var rand2 = Random.Range(0, 100);
            if (rand2 < 25) // GENERATE 2 WAY
            {
                curretLocation.NextLocation[1].NextLocation = new Location[2];
            }

            curretLocation.NextLocation[1].NextLocation[0] = Game.UIManager.UIMap.AddNew(curretLocation, curretLocation.NextLocation[1], curretLocation.NextLocation[1].NextLocation[0], Game.Data.CopyFromSerialize<InfoLocation>(newloc3));

            if (rand2 < 25) // GENERATE 2 WAY
            {
                var newloc4 = Game.FactoryLocation.MakeInfo(curretLocation.NextLocation[1].Info);
                newloc4.dungeon = RandomDangeon(curretLocation.NextLocation[1].Info);
                curretLocation.NextLocation[1].NextLocation[1] = Game.UIManager.UIMap.AddNew(curretLocation, curretLocation.NextLocation[1], curretLocation.NextLocation[1].NextLocation[1], Game.Data.CopyFromSerialize<InfoLocation>(newloc4));
            }
        }
    }

    public string RandomDangeon(InfoLocation location)
    {
        var dungeon = "";
        var rand = Random.Range(0, 100);
        if (rand < 75)
        {
            dungeon = location.dungeon;
        }
        else
        {
            dungeon = MakeNewRandomDangeon(location.dungeon);
        }
        return dungeon;
    }
    private string MakeNewRandomDangeon(string currentDangeon)
    {
        List<string> dangeonList = new List<string>();
        if (currentDangeon != "Forest") dangeonList.Add("Forest");
        if (currentDangeon != "Desert") dangeonList.Add("Desert");
        if (currentDangeon != "Swamp") dangeonList.Add("Swamp");
        if (currentDangeon != "Highland") dangeonList.Add("Highland");
        return dangeonList[Random.Range(0, 3)];
    }

}
