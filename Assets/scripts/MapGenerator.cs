using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode {NoiseMap, ColourMap, FalloffMap, Mesh};
    public DrawMode drawMode;

    public TerrainData terrainData;

    public bool autoUpdate;

    public Material material;

    public TerrainType[] regions;

    public void Start(){
        GenerateMap();
    }

    public void GenerateMap(){
        terrainData.ApplyToMaterial(material);
        float[,] falloffMap = FalloffGenerator.GenerateFallOffMap(terrainData.size);
        float[,] noiseMap = Noise.GenerateNoiseMap(terrainData, falloffMap);
        Color[] colourMap = new Color[terrainData.size * terrainData.size];
        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){
                if(terrainData.island) Mathf.Clamp01(noiseMap[x,y] -= falloffMap[x,y]);
                float currentHeight = noiseMap[x,y];
                for(int i = 0; i < regions.Length; i++){
                    if(currentHeight <= regions[i].height){
                        colourMap[y * terrainData.size + x] = regions [i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap){
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        } else if(drawMode == DrawMode.ColourMap){
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, terrainData.size, terrainData.size));
        } else if(drawMode == DrawMode.FalloffMap){
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFallOffMap(terrainData.size)));
        } else if(drawMode == DrawMode.Mesh){
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, terrainData.heightMultiplier, terrainData.meshHeightCurve));
        }
        
        terrainData.UpdateMeshHeights(material, terrainData.minHeight, terrainData.maxHeight);
    }

}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
}
