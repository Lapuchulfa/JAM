using UnityEditor;
using UnityEngine;

public class RespawnSoundSetup
{
    [MenuItem("Tools/JAM/Setup Respawn Sound")]
    public static void SetupRespawnSound()
    {
        // Buscar el MC en la escena
        GameObject mc = GameObject.Find("MC");
        if (mc == null)
        {
            EditorUtility.DisplayDialog("Error", "MC no encontrado en la escena", "OK");
            return;
        }

        // Obtener PlayerRespawn
        PlayerRespawn respawn = mc.GetComponent<PlayerRespawn>();
        if (respawn == null)
        {
            EditorUtility.DisplayDialog("Error", "PlayerRespawn no encontrado en MC", "OK");
            return;
        }

        // Buscar el audio clip
        AudioClip respawnClip = AssetDatabase.LoadAssetAtPath<AudioClip>(
            "Assets/Sounds/windows-10-usb-connected-sound-effect-128-ytshorts.savetube.me.mp3");

        if (respawnClip == null)
        {
            EditorUtility.DisplayDialog("Error", "No se encontró el archivo de sonido", "OK");
            return;
        }

        // Asignar mediante Reflection
        var field = typeof(PlayerRespawn).GetField("respawnClip",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field != null)
        {
            field.SetValue(respawn, respawnClip);
            EditorUtility.SetDirty(respawn);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("Éxito", "✅ Sonido de respawn asignado correctamente", "OK");
        }
    }
}
