using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    // Start is called before the first frame update
    void Start()
    {
        renderTexture = new RenderTexture(256, 256, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (renderTexture == null)
        {
        renderTexture = new RenderTexture(256, 256, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        }

        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.SetFloat("resolution", renderTexture.width);
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        Graphics.Blit(renderTexture, dst);
    }
}
