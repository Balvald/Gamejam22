using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    public List<string[]> StationData = new List<string[]>();

    public List<string[]> ReadCSVFile(string path)
    {
        // Reads GK3 type data and writes name and location of geographical name into StationData. Expects format as used in gn250 by the Bundesamt für Karthographie und Geodäsie
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
            // Name, Geo X pos, Geo Y pos. The Geographical positions of the gn250 are already for a cartesian coordinate system.
            StationData.Add(new string[]{ values[4], values[27], values[28] });
        }

        streamReader.Close();

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

        streamReader.Close();

        return StationData;
    }

}