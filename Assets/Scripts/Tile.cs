using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Color color
    {
        get{
            return spriteRenderer.color;
        }
        set{
            spriteRenderer.color = value;
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
