using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : ScriptableObject
{

    [Header("Terrain")]
    public bool island;
    public int size;
    public float heightMultiplier;
    public AnimationCurve meshHeightCurve;

    float savedMinHeight;
    float savedMaxHeight;

    [Header("Noise")]
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;

    [Header("Shader")]
    public Color[] baseColours;
    [Range(0,1)]
    public float[] baseStartHeights;
    [Range(0,1)]
    public float[] baseBlends;

    public float minHeight
    {
        get
        {
            return heightMultiplier * meshHeightCurve.Evaluate(0)*9.5f;
        }
    }

    public float maxHeight
    {
        get
        {
            return heightMultiplier * meshHeightCurve.Evaluate(1)*9.5f;
        }
    }

    public void ApplyToMaterial(Material material){

        material.SetInt("baseColourCount", baseColours.Length);
        material.SetColorArray("baseColours", baseColours);
        material.SetFloatArray("baseStartHeights", baseStartHeights);
        material.SetFloatArray("baseBlends", baseBlends);

        UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);
    }

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight){
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
    }

    void OnValidate()
    {
        if (size < 1)
        {
            size = 1;
        }
        if(lacunarity < 1){
            lacunarity = 1;
        }
        if(octaves < 0){
            octaves = 0;
        }
    }
}
