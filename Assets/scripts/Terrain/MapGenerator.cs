using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode {NoiseMap, ColourMap, FalloffMap, Mesh, TreeNoise};
    public DrawMode drawMode;

    public TerrainData terrainData;
    public TreesData treesData;
    public bool generateTrees;

    public bool autoUpdate;

    public Material material;

    public TerrainType[] regions;
    public GameObject terrainList;

    public LayerMask groundLayerMask;
    

    public void Start(){
        Invoke("GenerateMap", 1f);
    }

    public void GenerateMap(){
        GameObject treesList = new GameObject("TreesList");
        treesList.transform.parent = terrainList.transform;
        terrainData.ApplyToMaterial(material);
        float[,] falloffMap = FalloffGenerator.GenerateFallOffMap(terrainData.size);
        float[,] noiseMap = Noise.GenerateNoiseMap(terrainData);
        float[,] treeNoise = TreesNoise.GenerateTreesNoiseMap(terrainData, treesData, noiseMap, falloffMap);
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
                if(generateTrees){
                    for(int i = 0; i < treesData.trees.Length; i++){
                        if(treeNoise[x,y] > 0.5f && currentHeight <= treesData.trees[i].maxHeight){
                            float spawnPointY = terrainData.meshHeightCurve.Evaluate(noiseMap[x,y])*terrainData.heightMultiplier;
                            Vector3 treeSpawnPoint = new Vector3 (topLeftX + x + treeNoise[x,y]*10f, 100f, topLeftZ - y + noiseMap[x,y]*10f);
                            RaycastHit hit;
                            if (Physics.Raycast(treeSpawnPoint*10f, Vector3.down, out hit, Mathf.Infinity, groundLayerMask) && hit.point.y > 35f)
                            {
                                GameObject spawnedObject = Instantiate(treesData.trees[i].tree, hit.point, Quaternion.identity);
                                spawnedObject.transform.parent = treesList.transform;
                            }
                            break;
                        }
                    }
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
        } else if(drawMode == DrawMode.TreeNoise){
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(treeNoise));
        }
        
    }

}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
}
