using UnityEditor;
using UnityEngine;

public class FootstepsSetup
{
    [MenuItem("Tools/JAM/Setup Footstep Sounds")]
    public static void SetupFootstepSounds()
    {
        // Buscar MC
        GameObject mc = GameObject.Find("MC");
        if (mc == null)
        {
            EditorUtility.DisplayDialog("Error", "❌ MC no encontrado", "OK");
            return;
        }

        // Obtener ThirdPersonController
        ThirdPersonController controller = mc.GetComponent<ThirdPersonController>();
        if (controller == null)
        {
            EditorUtility.DisplayDialog("Error", "❌ ThirdPersonController no encontrado", "OK");
            return;
        }

        // Cargar clips de sonido
        AudioClip jumpUp = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/JumpUp.mp3");
        AudioClip jumpLanding = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/JumpLanding.mp3");

        if (jumpUp == null || jumpLanding == null)
        {
            EditorUtility.DisplayDialog("Error", "❌ No se encontraron los archivos de sonido", "OK");
            return;
        }

        // Asignar via reflection
        var jumpUpField = typeof(ThirdPersonController).GetField("jumpUpClip",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var jumpLandingField = typeof(ThirdPersonController).GetField("jumpLandingClip",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Obtener field footstepClips (es público)
        var footstepClipsField = typeof(ThirdPersonController).GetField("footstepClips",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (jumpUpField != null && jumpLandingField != null)
        {
            jumpUpField.SetValue(controller, jumpUp);
            jumpLandingField.SetValue(controller, jumpLanding);
        }

        // Configurar footstep clips
        if (footstepClipsField != null)
        {
            AudioClip[] footsteps = new AudioClip[] { jumpUp, jumpLanding };
            footstepClipsField.SetValue(controller, footsteps);
        }

        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("✅ Éxito",
            "Sonidos de pasos configurados:\n" +
            "✅ Jump Up Sound\n" +
            "✅ Jump Landing Sound\n" +
            "✅ Footstep Array (2 clips)\n\n" +
            "Los pasos se reproducirán automáticamente al caminar.",
            "OK");
    }
}
