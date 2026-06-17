using UnityEngine;

[ExecuteAlways]
public class SkyboxApplier : MonoBehaviour
{
    public Texture2D skyTexture;
    [Range(0f, 8f)] public float exposure = 1f;

    void OnEnable() => Apply();

#if UNITY_EDITOR
    void OnValidate() => Apply();
#endif

    void Apply()
    {
        if (skyTexture == null) return;

        Material mat = new Material(Shader.Find("Skybox/Panoramic"));
        mat.SetTexture("_MainTex", skyTexture);
        mat.SetFloat("_Exposure", exposure);
        mat.SetFloat("_Rotation", 0f);

        RenderSettings.skybox = mat;
        DynamicGI.UpdateEnvironment();
    }
}
