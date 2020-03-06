using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EffectRenderer : MonoBehaviour
{
    private CommandBuffer _commandBuffer;
    private readonly Dictionary<Camera, CommandBuffer> _cameras = new Dictionary<Camera, CommandBuffer>();
    private static readonly int ParamsShaderPropertyName = Shader.PropertyToID("_params");
    private static bool _forceUpdateCommandBuffer;

    private void Cleanup()
    {
        foreach (var cam in _cameras)
            if (cam.Key)
                cam.Key.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, cam.Value);
        _cameras.Clear();
    }

    public void OnDisable()
    {
        Cleanup();
    }

    public void OnEnable()
    {
        Cleanup();
    }

    public static void ForceUpdate()
    {
        _forceUpdateCommandBuffer = true;
    }

    public void OnWillRenderObject()
    {
        var render = gameObject.activeInHierarchy && enabled;
        if (!render || _forceUpdateCommandBuffer)
        {
            Cleanup();
            _forceUpdateCommandBuffer = false;
        }

        var cam = Camera.current;
        if (!cam || _cameras.ContainsKey(cam))
            return;

        _commandBuffer = new CommandBuffer {name = "Effect"};
        _cameras[cam] = _commandBuffer;

        var identifier = Shader.PropertyToID("_Temp01");
        _commandBuffer.GetTemporaryRTArray(identifier, -1, -1, EffectSystem.SubscribedObjects.Count, 24, FilterMode.Bilinear, RenderTextureFormat.Default);
        var i = 0;
        foreach (var o in EffectSystem.SubscribedObjects)
        {
            _commandBuffer.SetRenderTarget(identifier, 0, CubemapFace.Unknown, i);
            _commandBuffer.ClearRenderTarget(true, true, Color.black);
            var r = o.GetComponent<Renderer>();
            var glowMat = EffectObject.Mat;
            if (r && glowMat)
                _commandBuffer.DrawRenderer(r, glowMat);
            i++;
        }

        _commandBuffer.SetGlobalInt("_MaskArrayCount", EffectSystem.SubscribedObjects.Count);
        _commandBuffer.SetGlobalTexture("_MaskArray", identifier);
        _commandBuffer.ReleaseTemporaryRT(identifier);

        cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _commandBuffer);
    }

    private void Update()
    {
        // TODO only set when params change
        Shader.SetGlobalTexture(ParamsShaderPropertyName, EffectSystem.ParamsTexture);
    }
}