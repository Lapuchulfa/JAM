using UnityEditor.Animations;
using UnityEditor;
using UnityEngine;

public class AnimatorSetup
{
    [MenuItem("Tools/JAM/Setup Animator Parameters")]
    public static void SetupAnimatorParameters()
    {
        // Buscar MC
        GameObject mc = GameObject.Find("MC");
        if (mc == null)
        {
            EditorUtility.DisplayDialog("Error", "❌ MC no encontrado", "OK");
            return;
        }

        // Obtener Animator
        Animator animator = mc.GetComponent<Animator>();
        if (animator == null)
        {
            EditorUtility.DisplayDialog("Error", "❌ Animator no encontrado en MC", "OK");
            return;
        }

        // Obtener el Controller
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
        if (controller == null)
        {
            EditorUtility.DisplayDialog("Error", "❌ AnimatorController no encontrado", "OK");
            return;
        }

        // Agregar parámetros si no existen
        bool hasJump = false;
        bool hasLand = false;
        bool hasIsGrounded = false;
        bool hasSpeed = false;

        foreach (var param in controller.parameters)
        {
            if (param.name == "Jump") hasJump = true;
            if (param.name == "Land") hasLand = true;
            if (param.name == "isGrounded") hasIsGrounded = true;
            if (param.name == "Speed") hasSpeed = true;
        }

        // Agregar faltantes
        if (!hasJump)
        {
            controller.AddParameter("Jump", AnimatorControllerParameterType.Trigger);
            Debug.Log("✅ Parámetro 'Jump' agregado");
        }

        if (!hasLand)
        {
            controller.AddParameter("Land", AnimatorControllerParameterType.Trigger);
            Debug.Log("✅ Parámetro 'Land' agregado");
        }

        if (!hasIsGrounded)
        {
            controller.AddParameter("isGrounded", AnimatorControllerParameterType.Bool);
            Debug.Log("✅ Parámetro 'isGrounded' agregado");
        }

        if (!hasSpeed)
        {
            controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
            Debug.Log("✅ Parámetro 'Speed' agregado");
        }

        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("✅ Éxito",
            "Parámetros del Animator configurados correctamente:\n" +
            "✅ Jump (Trigger)\n" +
            "✅ Land (Trigger)\n" +
            "✅ isGrounded (Bool)\n" +
            "✅ Speed (Float)",
            "OK");
    }
}
