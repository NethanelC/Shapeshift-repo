using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dupe : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PolygonCollider2D _collider;
    public void SetSpriteAndCollider(Sprite sprite, Vector2[] points)
    {
        _spriteRenderer.sprite = sprite;
        _collider.points = points;
    }
}
