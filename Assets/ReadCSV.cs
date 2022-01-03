using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ReadCSVFile();
    }

    void ReadCSVFile()
    {
        StreamReader streamReader = new StreamReader("./Assets/test.csv");

        while(true)
        {
            string data = streamReader.ReadLine();
            if (data == null)
            {
                // We reached End of Line so break out of reading the file.
                break;
            }
            string[] values = data.Split(',');
            foreach (var value in values)
            {
                Debug.Log(value.ToString());
            }
        }
    }
}