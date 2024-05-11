using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Data class used for managing the details of a specific type of crime
[System.Serializable]
public class CrimeData
{
    public enum Crime
    {
        robbery, homicide, arson, fraud, invalid
    }
    public static List<Crime> AllCrimes = new List<Crime>() { Crime.robbery, Crime.homicide, Crime.arson, Crime.fraud };

    public Crime crimeValue;

    public string crimeText = "";

    public Sprite sprite;
}
