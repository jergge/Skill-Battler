using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration{
[System.Serializable]
public class Layer
{
    public Texture2D texture;
    public float textureScale;
    public Color tint;
    [Range(0,1)]
    public float tintStrength;
    [Range(0,1)]
    public float startHeight;
    [Range(0,1)]
    public float blendStrength;

}}
