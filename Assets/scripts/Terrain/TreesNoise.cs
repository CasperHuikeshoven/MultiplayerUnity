using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TreesNoise 
{
    
    public static float[,] GenerateTreesNoiseMap(TerrainData terrainData, TreesData treesData, float[,] noiseMap, float[,] falloffMap){

        float[,] treeNoise = new float[terrainData.size, terrainData.size];

        System.Random prng = new System.Random(treesData.seed);
        Vector2[] octaveOffsets = new Vector2[treesData.octaves];
        for(int i = 0; i < treesData.octaves; i++){
            float offsetX = prng.Next(-100000, 100000) + treesData.offset.x;
            float offsetY = prng.Next(-100000, 100000) + treesData.offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(treesData.noiseScale <= 0) treesData.noiseScale = 0.00001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){

                float ampplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0;

                for(int i = 0; i < treesData.octaves; i++){
                    float sampleX = x / treesData.noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = y / treesData.noiseScale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * ampplitude;

                    ampplitude *= treesData.persistance;
                    frequency *= treesData.lacunarity;
                }

                if(noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }
                treeNoise[x,y] = noiseHeight;
            }
        }

        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){
                treeNoise[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, treeNoise[x,y]);
            }
        }

        float topLeftX = (terrainData.size - 1)/-2f;
        float topLeftZ = (terrainData.size -1)/2f;

        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){
                if(terrainData.island) Mathf.Clamp01(treeNoise[x,y] -= falloffMap[x,y]);
                if(treeNoise[x,y] < 0.5f || treeNoise[x,y] > 0.6f || noiseMap[x,y] > 0.8f || noiseMap[x,y] < 0.4f){
                    treeNoise[x,y] = 0;
                }
            }
        }

        return treeNoise;

    }
    
}
