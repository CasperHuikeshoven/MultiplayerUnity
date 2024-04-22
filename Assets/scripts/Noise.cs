using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
   
    public static float[,] GenerateNoiseMap(TerrainData terrainData, float[,] falloffMap){
        float[,] noiseMap = new float[terrainData.size, terrainData.size];

        System.Random prng = new System.Random(terrainData.seed);
        Vector2[] octaveOffsets = new Vector2[terrainData.octaves];
        for(int i = 0; i < terrainData.octaves; i++){
            float offsetX = prng.Next(-100000, 100000) + terrainData.offset.x;
            float offsetY = prng.Next(-100000, 100000) + terrainData.offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(terrainData.noiseScale <= 0) terrainData.noiseScale = 0.00001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){

                float ampplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0;

                for(int i = 0; i < terrainData.octaves; i++){
                    float sampleX = x / terrainData.noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = y / terrainData.noiseScale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * ampplitude;

                    ampplitude *= terrainData.persistance;
                    frequency *= terrainData.lacunarity;
                }

                if(noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x,y] = noiseHeight;
            }
        }

        for(int y = 0; y < terrainData.size; y++){
            for(int x = 0; x < terrainData.size; x++){
                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
            }
        }

        return noiseMap;
    }

}
