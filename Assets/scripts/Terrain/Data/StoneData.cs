using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StoneData : ScriptableObject
{

    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public Stone[] stones;
    
}

[System.Serializable]
public class Stone{
    public GameObject stone;
    [Range(0,1)]
    public float maxHeight;
    [Range(0,2)]
    public float density;
}
