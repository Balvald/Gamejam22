using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteCSV : MonoBehaviour
{
    public List<string[]> StationData = new List<string[]>();

    void WriteGeoDataCompatibleCSVFile(List<string[]> data, string path="./Assets/csvSavetestGeocompatible.csv")
    {
        StreamWriter streamWriter = new StreamWriter(path);

        foreach (string[] entry in data)
        {
            Debug.Log("Writing Geocompatible: " + entry[0] + ";" + entry[1] + ";" + entry[2]);
            streamWriter.WriteLine(";;;;"+ entry[0] + ";;;;;;;;;;;;;;;;;;;;;;;"+ entry[1] + ";" + entry[2]);
        }

        streamWriter.Close();
    }

    void WriteCustomCSVFile(List<string[]> data, string path = "./Assets/csvSavetestCustom.csv")
    {
        StreamWriter streamWriter = new StreamWriter(path);

        foreach (string[] entry in data)
        {
            Debug.Log("Writing: " + entry[0] + ";" + entry[1] + ";" + entry[2]);
            streamWriter.WriteLine(entry[0] + ";" + entry[1] + ";" + entry[2]);
        }

        streamWriter.Close();
    }

    public void WriteCSVofCurrentlyDisplayedStations(List<string[]> data)
    {
        WriteCustomCSVFile(data);
    }
}