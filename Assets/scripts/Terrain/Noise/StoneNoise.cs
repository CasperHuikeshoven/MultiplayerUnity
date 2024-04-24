using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoneNoise 
{
    
    public static float[,] GenerateTreesNoiseMap(TerrainData terrainData, StoneData stoneData, float[,] noiseMap, float[,] falloffMap){

        float[,] stoneNoise = new float[terrainData.size, terrainData.size];

        System.Random prng = new System.Random(stoneData.seed);
        Vector2[] octaveOffsets = new Vector2[stoneData.octaves];
        for(int i = 0; i < stoneData.octaves; i++){
            float offsetX = prng.Next(-100000, 100000) + stoneData.offset.x;
            float offsetY = prng.Next(-100000, 100000) + stoneData.offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(stoneData.noiseScale <= 0) stoneData.noiseScale = 0.00001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){

                float ampplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0;

                for(int i = 0; i < stoneData.octaves; i++){
                    float sampleX = x / stoneData.noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = y / stoneData.noiseScale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * ampplitude;

                    ampplitude *= stoneData.persistance;
                    frequency *= stoneData.lacunarity;
                }

                if(noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }
                stoneNoise[x,y] = noiseHeight;
            }
        }

        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){
                stoneNoise[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, stoneNoise[x,y]);
            }
        }

        float topLeftX = (terrainData.size - 1)/-2f;
        float topLeftZ = (terrainData.size -1)/2f;

        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){
                if(terrainData.island) Mathf.Clamp01(stoneNoise[x,y] -= falloffMap[x,y]);
                if(stoneNoise[x,y] < 0.7f || noiseMap[x,y] > 0.8f || noiseMap[x,y] < 0.3f){
                    stoneNoise[x,y] = 0;
                }
            }
        }

        return stoneNoise;

    }
    
}
