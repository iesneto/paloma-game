using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCowData", menuName = "ScriptableObjects/CowDataAsset", order = 2)]
public class CowData : ScriptableObject
{
    public int id;
    public int reward;
    public float mass;
    public float movementSpeed;
    public int tier;

}
