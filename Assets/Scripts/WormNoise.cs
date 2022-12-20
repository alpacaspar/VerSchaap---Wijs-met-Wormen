using UnityEngine;

public class WormNoise : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Vector2Int resolution;
    [SerializeField] private int pointAmount = 20;

    private void Start()
    {
        Vector2[] points = Voronoi.GenerateRandomPoints(20 ,resolution.y, resolution.x);
        Texture2D voronoi = Voronoi.VoronoiNoise(resolution.x, resolution.y, points);

        Sprite sprite = Sprite.Create(voronoi, new Rect(0, 0, resolution.x, resolution.y), Vector2.one * 0.5f);

        spriteRenderer.sprite = sprite;
    }
}
