using System;
using Assets.Scripts.Data;
using UnityEngine;

[Serializable]
public class AuthoredCase
{
    private string Guid; // To Make sure that we dont show the same thing again
    public bool NoDayRange = true; // If this is checked we dont insert cases in specific days
    public Vector2Int DayRanges; // Which day ranges we want it to appear
    public AnimalData Animal;
    public CrimeData Crime;

    public string GimmeYourGuid() => Guid;
    public AuthoredCase()
    {
        Guid = System.Guid.NewGuid().ToString();
    }
}