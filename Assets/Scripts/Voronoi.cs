using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Voronoi
{
    public static Vector2[] GenerateRandomPoints(int amount, int width, int height)
    {
        Vector2[] points = new Vector2[amount];
        
        for (int i = 0; i < points.Length; i++)
        {
            float x = Random.value * width;
            float y = Random.value * height;

            points[i] = new Vector2(x, y);
        }

        return points;
    }

    public static Texture2D VoronoiNoise(int width, int height, Vector2[] seedPoints)
    {
        Texture2D tex = new Texture2D(width, height);
        
        for (int x = 0; x < tex.width; x++)
        for (int y = 0; y < tex.height; y++)
        {
            float[] distances = new float[seedPoints.Length];
            for (int i = 0; i < seedPoints.Length; i++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), seedPoints[i]);
                distances[i] = distance;
            }

            int n = 0;
            Array.Sort(distances);
            float noise = distances[n] / width;
            tex.SetPixel(x, y, new Color(noise, noise, noise, 1));
        }

        tex.Apply();
        return tex;
    }
}
