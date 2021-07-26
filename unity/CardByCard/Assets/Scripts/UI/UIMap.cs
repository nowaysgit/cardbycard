using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game = GameManager;

public class UIMap : MonoBehaviour
{
    //NEED REFACTORING
    [SerializeField] private GameObject InfoPanel;
    [SerializeField] private Button ChooseButton;
    [SerializeField] private GameObject Content;
    [SerializeField] private float MaxSize;
    [SerializeField] private float MinSize;
    [SerializeField] private GameObject LocationPrefab;
    [SerializeField] private List<Location> Locations;
    [SerializeField] private Location LastChoosesLocation;
    [SerializeField] private GameObject frame;
    [SerializeField] private GameObject frame2;
    [SerializeField] private GameObject playerIco;
    [SerializeField] private Camera cam;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform linesCase;

    [SerializeField] private Vector3 constantVector;
    private float size;
    private int chooseId;
    private int playerStayId;
    private int lastChoosesLocationId;
    private void Awake()
    {
        size = 0.2f;
        chooseId = 0;
        MakeLine(Locations[0].transform, Locations[1].transform);
        MakeLine(Locations[1].transform, Locations[2].transform);
        gameObject.SetActive(false);
        Game.singletone.OnCompliteLocation?.AddListener(CompliteLocation);
    }
    private void CompliteLocation()
    {
        SetIntractableLocations(lastChoosesLocationId);
    }
    private void MakeLine(Transform start, Transform end)
    {
        var targetDir = end.localPosition - start.localPosition;
        var distance = targetDir.magnitude;

        var line = Instantiate(linePrefab, start.localPosition, Quaternion.identity);
        line.transform.SetParent(linesCase, true);
        line.transform.localPosition = start.localPosition;
        line.transform.localScale = new Vector3(1, 1, 1);
        line.GetComponent<RectTransform>().sizeDelta = new Vector2(distance, 10);

        line.transform.LookAt2D(line.transform.up, end);
        line.transform.rotation = Quaternion.Euler(0, 0, line.transform.rotation.eulerAngles.z + 90);
    }

    public void ChangeSize(float amount)
    {
        if (size + amount <= MinSize || size + amount >= MaxSize) return;
        size = size + amount;
        Content.transform.localScale = new Vector3(size, size, 1f);
    }
    public Location AddNew(Location previousLocation, Location currentLocation, Location targetLocation, InfoLocation newInfo)
    {
        lastChoosesLocationId = previousLocation.Info.uniqueId;
        playerStayId = lastChoosesLocationId;
        var isTwo = false;
        var isSecond = false;
        if (currentLocation.NextLocation.Length > 1)
        {
            isTwo = true;
        }

        var lastLocationPos = new Vector3(currentLocation.Info.uniquePosX, currentLocation.Info.uniquePosY, 0);

        Vector3 nearestHeading = new Vector3(0, 0, 0);
        float nearestDistance = 100000;
        int nearestKey = 0;

        var count = Locations.Count;

        var originPoint = currentLocation.gameObject.transform.localPosition;
        originPoint += constantVector * 700f;

        for (int i = 0; i < count; i++)
        {
            var heading = lastLocationPos - Locations[i].transform.localPosition;
            var distance = heading.magnitude;

            if (distance < nearestDistance) ;
            {
                if (currentLocation.Info.uniqueId == i)
                {
                    continue;
                }
                if (isTwo && currentLocation.NextLocation[0] && currentLocation.NextLocation[0].Info.uniqueId == i)
                {
                    isSecond = true;
                    continue;
                }
                nearestDistance = distance;
                nearestHeading = heading;
                nearestKey = i;
            }
        }
        //MOVE FROM NEAREST
        var direction = nearestHeading / nearestDistance;
        originPoint += direction * 100f;


        //MOVE FROM PREVIOUS
        var headingToPrevious = currentLocation.transform.localPosition - currentLocation.PreviousLocation.transform.localPosition;
        var distanceToPrevious = headingToPrevious.magnitude;
        var directionToPrevious = headingToPrevious / distanceToPrevious;
        originPoint = originPoint + (directionToPrevious * 100f);

        //MOVE FROM RANDOM
        float radius = 100f;
        originPoint.x += Random.Range(-radius, radius);
        originPoint.y += Random.Range(-radius, radius);

        Debug.Log("");
        Debug.Log("START GENERATE");
        originPoint = GeneratePosition(originPoint, count, currentLocation, previousLocation, isSecond);
        Debug.Log("FINISH GENERATE");
        Debug.Log("");

        newInfo.uniqueId = count;
        newInfo.uniquePosX = originPoint.x;
        newInfo.uniquePosY = originPoint.y;

        GameObject locationUIElement = MakeButton(newInfo);

        //locationUIElement.GetComponent<Location>().Info.uniquePosX = locationUIElement.transform.localPosition.x;
        //locationUIElement.GetComponent<Location>().Info.uniquePosY = locationUIElement.transform.localPosition.y;


        Locations.Add(locationUIElement.GetComponent<Location>());

        Locations[count].transform.localPosition = new Vector3(Locations[count].Info.uniquePosX, Locations[count].Info.uniquePosY, 0);

        Locations[count].PreviousLocation = Locations[currentLocation.Info.uniqueId];
        MakeLine(Locations[currentLocation.Info.uniqueId].transform, Locations[count].transform);

        frame2.SetActive(false);
        InfoPanel.SetActive(false);
        Locations[count].gameObject.GetComponent<Button>().interactable = false;
        Locations[currentLocation.Info.uniqueId].gameObject.GetComponent<Button>().interactable = true;


        if (previousLocation.NextLocation.Length > 1)
        {
            if (previousLocation.NextLocation[0])
            {
                previousLocation.NextLocation[0].gameObject.GetComponent<Button>().interactable = true;
            }
            if (previousLocation.NextLocation[1])
            {
                previousLocation.NextLocation[1].gameObject.GetComponent<Button>().interactable = true;
            }

        }

        if (previousLocation.PreviousLocation.NextLocation[0])
        {
            previousLocation.PreviousLocation.NextLocation[0].gameObject.GetComponent<Button>().interactable = false;
        }
        if (previousLocation.PreviousLocation.NextLocation.Length > 1 && previousLocation.PreviousLocation.NextLocation[1])
        {
            previousLocation.PreviousLocation.NextLocation[1].gameObject.GetComponent<Button>().interactable = false;
        }

        Locations[previousLocation.Info.uniqueId].gameObject.GetComponent<Button>().interactable = false;

        if (Locations[previousLocation.Info.uniqueId].PreviousLocation)
        {
            Locations[previousLocation.PreviousLocation.Info.uniqueId].gameObject.GetComponent<Button>().interactable = true;
        }
        if (Locations[previousLocation.Info.uniqueId].PreviousLocation.PreviousLocation)
        {
            Locations[previousLocation.PreviousLocation.PreviousLocation.Info.uniqueId].gameObject.GetComponent<Button>().interactable = false;
        }
        if (!isSecond)
        {
            frame.transform.localPosition = Locations[previousLocation.NextLocation[0].Info.uniqueId].transform.localPosition;
            playerIco.transform.localPosition = new Vector3(previousLocation.Info.uniquePosX, previousLocation.Info.uniquePosY, 0);
        }
        targetLocation = Locations[count];
        targetLocation.GetComponent<Button>().onClick.AddListener(() => OnClick(newInfo.uniqueId));
        return targetLocation;
    }
    private GameObject MakeButton(InfoLocation item)
    {
        GameObject button = (GameObject)Instantiate(LocationPrefab) as GameObject;

        button.GetComponent<Location>().Info = item;

        button.transform.SetParent(Content.transform, true);
        button.transform.localScale = new Vector3(3, 3, 1);
        button.transform.localPosition = new Vector3(item.uniquePosX, item.uniquePosY, 0);
        button.transform.GetChild(2).GetComponent<Text>().text = item.title;
        button.transform.GetChild(3).GetComponent<Text>().text = item.distance.ToString();

        button.GetComponent<Button>().onClick.AddListener(() => OnClick(item.uniqueId));
        return button;
    }
    private Vector3 TryMoveByRules(Location location, Vector3 originPoint, float distanceTo, bool needOneMoreGet, out bool needOneMore)
    {
        if (location == null)
        {
            needOneMore = needOneMoreGet;
            return originPoint;
        }
        var heading = originPoint - location.transform.localPosition;
        var distance = heading.magnitude;
        var direction = heading / distance;
        if (distance < distanceTo)
        {
            needOneMore = true;
            return originPoint + (direction * 100f);
        }
        else
        {
            needOneMore = needOneMoreGet;
            return originPoint;
        }
    }
    private Location GetNearest(Vector3 originPoint, Location location, float lastDistance, out float distance)
    {
        var heading = originPoint - location.transform.localPosition;
        if (heading.magnitude < lastDistance)
        {
            distance = heading.magnitude;
            return location;
        }
        else
        {
            distance = lastDistance;
            return null;
        }
    }
    public Vector2 GeneratePosition(Vector3 originPoint, int count, Location currentLocation, Location previousLocation, bool isSecond)
    {
        var needOneMore = true;
        var iteration = 0;
        var iterationPREVIOUS = 0;
        var iterationNearestBranch = 0;
        var iterationPREVIOUSLOCATION = 0;
        var iterationFIRSTLOCATION = 0;
        var iterationNEARESTLOCATION = 0;
        while (needOneMore)
        {
            iteration++;
            needOneMore = false;

            //TO PREVIOUS PREVIOUS LOCATION
            var originPointBefore = originPoint;
            originPoint = TryMoveByRules(currentLocation.PreviousLocation, originPoint, 2000f, needOneMore, out needOneMore);
            if (originPointBefore != originPoint)
            {
                iterationPREVIOUS++;
            }

            //TO NEAREST branch 
            var distanceNearestBranch = 100000000f;
            Location nearestBranch = null;
            if (previousLocation.NextLocation.Length > 1)
            {
                if (previousLocation.NextLocation[0])
                {
                    if (previousLocation.NextLocation[0].NextLocation[0])
                    {
                        var branch = GetNearest(originPoint, previousLocation.NextLocation[0].NextLocation[0], distanceNearestBranch, out distanceNearestBranch);
                        if (branch != null) nearestBranch = branch;
                    }
                    if (previousLocation.NextLocation[0].NextLocation.Length > 1 && previousLocation.NextLocation[0].NextLocation[1])
                    {
                        var branch = GetNearest(originPoint, previousLocation.NextLocation[0].NextLocation[1], distanceNearestBranch, out distanceNearestBranch);
                        if (branch != null) nearestBranch = branch;
                    }
                }
                if (previousLocation.NextLocation[1])
                {
                    if (previousLocation.NextLocation[1].NextLocation[0])
                    {
                        var branch = GetNearest(originPoint, previousLocation.NextLocation[1].NextLocation[0], distanceNearestBranch, out distanceNearestBranch);
                        if (branch != null) nearestBranch = branch;
                    }
                    if (previousLocation.NextLocation[1].NextLocation.Length > 1 && previousLocation.NextLocation[1].NextLocation[1])
                    {
                        var branch = GetNearest(originPoint, previousLocation.NextLocation[1].NextLocation[1], distanceNearestBranch, out distanceNearestBranch);
                        if (branch != null) nearestBranch = branch;
                    }
                }
            }
            originPointBefore = originPoint;
            originPoint = TryMoveByRules(nearestBranch, originPoint, 2000f, needOneMore, out needOneMore);
            if (originPointBefore != originPoint)
            {
                iterationNearestBranch++;
            }

            //TO PREVIOUS LOCATION
            originPointBefore = originPoint;
            originPoint = TryMoveByRules(currentLocation, originPoint, 1000f, needOneMore, out needOneMore);
            if (originPointBefore != originPoint)
            {
                iterationPREVIOUSLOCATION++;
            }


            //TO FIRST LOCATION
            var distanceToFirstLocation = 0f;
            if (isSecond)
            {
                originPointBefore = originPoint;
                originPoint = TryMoveByRules(currentLocation.NextLocation[0], originPoint, 1000f, needOneMore, out needOneMore);
                if (originPointBefore != originPoint)
                {
                    iterationFIRSTLOCATION++;
                }
            }

            //TO NEAREST LOCATION
            var distanceNearest = 100000000f;
            Location nearestLocation = null;
            for (int i = 0; i < count; i++)
            {
                var heading = originPoint - Locations[i].transform.localPosition;
                var distance = heading.magnitude;

                if (distance < distanceNearest) ;
                {
                    nearestLocation = Locations[i];
                }
            }
            originPointBefore = originPoint;
            originPoint = TryMoveByRules(nearestLocation, originPoint, 1000f, needOneMore, out needOneMore);
            if(originPointBefore != originPoint)
            {
                iterationNEARESTLOCATION++;
            }



            if (iteration == 1000)
            {
                needOneMore = false;
            }
        }
        Debug.Log("iteration: " + iteration);
        Debug.Log("iterationPREVIOUS: " + iterationPREVIOUS);
        Debug.Log("iterationNearestBranch: " + iterationNearestBranch);
        Debug.Log("iterationPREVIOUSLOCATION: " + iterationPREVIOUSLOCATION);
        Debug.Log("iterationFIRSTLOCATION: " + iterationFIRSTLOCATION);
        Debug.Log("iterationNEARESTLOCATION: " + iterationNEARESTLOCATION);


        return originPoint;
    }
    public void OnClick(int id)
    {
        frame2.SetActive(true);
        frame2.transform.localPosition = Locations[id].gameObject.transform.localPosition;
        InfoPanel.SetActive(true);
        InfoPanel.transform.GetChild(0).GetComponent<Text>().text = Locations[id].Info.title;
        InfoPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = Locations[id].Info.description;

        if (Locations[id].IsPassed || Locations[id].IsMovable)
        {
            ChooseButton.interactable = false;
            SetIntractableLocations(id);
            playerStayId = id;
            if (Locations[id].IsMovable)
            {
                lastChoosesLocationId = id;
            }
        }
        else
        {
            ChooseButton.interactable = true;
        }
        chooseId = id;
    }
    private void SetIntractableLocations(int id)
    {
        playerIco.transform.localPosition = Locations[id].transform.localPosition;
        //SET INACTIVE NEXT PREVIOUS LOCATIONS
        if (Locations[playerStayId].PreviousLocation)
        {
            Locations[playerStayId].PreviousLocation.gameObject.GetComponent<Button>().interactable = false;
        }
        Locations[playerStayId].NextLocation[0].gameObject.GetComponent<Button>().interactable = false;
        if (Locations[playerStayId].NextLocation.Length > 1 && Locations[playerStayId].NextLocation[1])
        {
            Locations[playerStayId].NextLocation[1].gameObject.GetComponent<Button>().interactable = false;
        }

        //SET INACTIVE NEW LOCATION AND SET ACTIVE NEXT NEW LOCATIONS AND NEW PREVIOUS LOCATION
        Locations[id].gameObject.GetComponent<Button>().interactable = false;
        if (Locations[id].PreviousLocation)
        {
            Locations[id].PreviousLocation.gameObject.GetComponent<Button>().interactable = true;
        }
        Locations[id].NextLocation[0].gameObject.GetComponent<Button>().interactable = true;
        if (Locations[id].NextLocation.Length > 1 && Locations[id].NextLocation[1])
        {
            Locations[id].NextLocation[1].gameObject.GetComponent<Button>().interactable = true;
        }
    }
    public void OnChoose()
    {
        frame.transform.localPosition = Locations[chooseId].gameObject.transform.localPosition;
        Game.singletone.OnChooseLocation?.Invoke(Locations[chooseId]);
    }

}
