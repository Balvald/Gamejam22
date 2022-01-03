using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    public GameObject myPrefab;
    public Transform parent;

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
            string[] values = data.Split(';');
            Debug.Log(values[4].ToString() + " " + values[27] + " " + values[28]);
            Vector3 currentPlacePosition = new Vector3(float.Parse(values[27], CultureInfo.InvariantCulture) * 0.01f, float.Parse(values[28], CultureInfo.InvariantCulture) * 0.01f, 0) + new Vector3(-3951, -52714, -4);
            var currentPlace = Instantiate(myPrefab, currentPlacePosition, Quaternion.identity);
            currentPlace.name = values[4].ToString();
            currentPlace.transform.SetParent(parent, false);
        }
    }
}