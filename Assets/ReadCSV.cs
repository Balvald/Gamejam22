using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    public List<string[]> StationData = new List<string[]>();

    public List<string[]> ReadCSVFile(string path)
    {
        StreamReader streamReader = new StreamReader(path);

        while(true)
        {
            string data = streamReader.ReadLine();
            if (data == null)
            {
                // We reached End of Line so break out of reading the file.
                break;
            }
            string[] values = data.Split(';');
            Debug.Log(values[4] + ";...;" + values[27] + ";...;" + values[28]);
            StationData.Add(new string[]{ values[4], values[27], values[28] }); // Name, Geo X pos, Geo Y pos
        }

        return StationData;
    }

    public List<string[]> ReadCustomCSVFile(string path)
    {
        StreamReader streamReader = new StreamReader(path);

        while (true)
        {
            string data = streamReader.ReadLine();
            if (data == null)
            {
                // We reached End of Line so break out of reading the file.
                break;
            }
            string[] values = data.Split(';');
            Debug.Log(values[0] + " " + values[1] + " " + values[2]);
            StationData.Add(new string[] { values[0], values[1], values[2] }); // Name, Geo X pos, Geo Y pos
        }

        return StationData;
    }

}