using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class PostProcess : MonoBehaviour
{
    public Material material;
    private Camera _cam;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_cam)
        {
            _cam = GetComponent<Camera>();
            _cam.depthTextureMode = DepthTextureMode.Depth;
        }
        
        Graphics.Blit(source, destination, material);
    }
}