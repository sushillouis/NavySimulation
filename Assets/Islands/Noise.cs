using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] CreateNoiseMap(int width, int height, float scale)
    {
        float[,] noiseMap = new float[width, height];

        if(scale <= 0)
            scale = 0.00001f;

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                float x = i / scale;
                float y = j / scale;

                float perl = Mathf.PerlinNoise(x, y);
                noiseMap[i,j] = perl;
            }
        }

        return noiseMap;
    }
    
}
