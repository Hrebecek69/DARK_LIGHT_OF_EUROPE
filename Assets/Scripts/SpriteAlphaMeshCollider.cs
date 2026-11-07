using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class SpriteAlphaMeshCollider : MonoBehaviour
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f;
}
