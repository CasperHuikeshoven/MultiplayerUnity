using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TreesData : ScriptableObject
{

    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public Tree[] trees;
    
}

[System.Serializable]
public class Tree{
    public GameObject tree;
    [Range(0,1)]
    public float maxHeight;
    public float density;
}
