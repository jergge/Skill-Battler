using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoiseSystem;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Terrain System/Layers")]
public class TerrainLayers : UpdateableData
{
    [SerializeField] Material material;
    [SerializeField] List<TerrainLayer> layersList;

    public TerrainLayer[] layers => layersList.ToArray();

}

[System.Serializable]
public class TerrainLayer
{ 
    [Header("Primary Texture")]
    public Texture2D albedoMapPrimary;
    public Texture2D normalMapPrimary;
    public Texture2D displacementMapPrimary;
    public float textureScalePrimary;

    [Header("Secondary Texture")]
    public Texture2D albedoMapSecondary;
    public Texture2D normalMapSecondary;
    public Texture2D displacementMapSecondary;
    public float textureScaleSecondary;

    [Header("Colour tinting")]
    public Color tint;
    [Range(0,1)]
    public float tintStrength;

    [Header("Self Blending Settings")]
    [RequireInterface(typeof(INoiseSampler2D))][SerializeField] Object _noiseSampler2D;
    INoiseSampler2D noiseSampler2D => _noiseSampler2D as INoiseSampler2D;
    [SerializeField] bool blend = false;
    [SerializeField][Range(0, 1)] float changeThreshold;
    [SerializeField][Range(0, 1)] float belndingStrength;

    [Header("Blending with other layers")]
    [Range(0,1)] public float startHeight;
    [Range(0,1)] public float blendStrength;
}