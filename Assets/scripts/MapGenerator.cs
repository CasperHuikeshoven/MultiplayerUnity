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

    public GameObject[] trees;
    public GameObject treesList;

    public void Start(){
        Invoke("GenerateMap", 1f);
    }

    public void GenerateMap(){
        
        terrainData.ApplyToMaterial(material);
        float[,] falloffMap = FalloffGenerator.GenerateFallOffMap(terrainData.size);
        float[,] noiseMap = Noise.GenerateNoiseMap(terrainData);
        Color[] colourMap = new Color[terrainData.size * terrainData.size];

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        float topLeftX = (width - 1)/-2f;
        float topLeftZ = (height -1)/2f;

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
                if(currentHeight >= regions[2].height && currentHeight <= regions[6].height &&Random.Range(0, 5) == 2){
                    int randomIndex = Random.Range(0, trees.Length-1);
                    float spawnPointY = terrainData.meshHeightCurve.Evaluate(noiseMap[x,y])*terrainData.heightMultiplier;
                    Vector3 treeSpawnPoint = new Vector3 (topLeftX + x, terrainData.meshHeightCurve.Evaluate(noiseMap[x,y])*terrainData.heightMultiplier, topLeftZ - y);
                    GameObject spawnedObject = Instantiate(trees[randomIndex], treeSpawnPoint*10, Quaternion.identity);
                    spawnedObject.transform.parent = treesList.transform;
                }
            }
        }

        terrainData.UpdateMeshHeights(material, terrainData.minHeight, terrainData.maxHeight);
        terrainData.ApplyToMaterial(material);

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
        
    }

}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
}
