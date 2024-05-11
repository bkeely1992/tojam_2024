using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create AuthoredCaseDatabase", fileName = "AuthoredCaseDatabase", order = 0)]
public class AuthoredCaseDatabase : ScriptableObject
{
    public List<AuthoredCase> Cases;
}