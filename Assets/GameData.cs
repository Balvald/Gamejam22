using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<StationData> allStationData;

    public List<TrainLineData> allTrainLineData;

    public PlayerResourceData playerResourceData;
}

[System.Serializable]
public class StationData
{
    public string stationName;

    public float posX; // X pos

    public float posY; // Y pos

    public bool unlocked;

    public bool ironbuilt;

    public bool coalbuilt;

    public bool moneybuilt;

    public int ironEfficiency;

    public int coalEfficiency;

    public int moneyEfficiency;
}

[System.Serializable]
public class PlayerResourceData
{
    public int iron;

    public int coal;

    public int money;
}

[System.Serializable]
public class TrainLineData
{
    public int[] stationIndices;
}