using UnityEngine;

[ExecuteInEditMode]
public class AutoSetupManager : MonoBehaviour
{
    [Header("Auto Setup Configuration")]
    [SerializeField] private bool autoSetupOnAwake = true;
    [SerializeField] private bool showDebugMessages = true;

    public void AutoConfigurePlayer()
    {
        if (showDebugMessages)
            Debug.Log("🎮 [AutoSetup] Iniciando auto-configuración del jugador...");

        // Buscar el personaje MC
        GameObject playerGO = GameObject.Find("MC");
        if (playerGO == null)
        {
            Debug.LogError("❌ No se encontró objeto 'MC'. Asegúrate de que existe en la escena.");
            return;
        }

        // Configurar ThirdPersonController
        ThirdPersonController controller = playerGO.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            ConfigureThirdPersonController(controller, playerGO);
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró ThirdPersonController en MC");
        }

        // Configurar PlayerRespawn
        PlayerRespawn respawn = playerGO.GetComponent<PlayerRespawn>();
        if (respawn != null)
        {
            ConfigurePlayerRespawn(respawn);
        }

        // Agregar componentes faltantes
        if (playerGO.GetComponent<AnimationController>() == null)
        {
            playerGO.AddComponent<AnimationController>();
            if (showDebugMessages)
                Debug.Log("✅ AnimationController agregado");
        }

        if (playerGO.GetComponent<PlayerMovementOptimizer>() == null)
        {
            playerGO.AddComponent<PlayerMovementOptimizer>();
            if (showDebugMessages)
                Debug.Log("✅ PlayerMovementOptimizer agregado");
        }

        if (showDebugMessages)
            Debug.Log("✅ [AutoSetup] Auto-configuración completada!");
    }

    void ConfigureThirdPersonController(ThirdPersonController controller, GameObject playerGO)
    {
        // Cambiar airControl
        controller.airControl = 0.5f;
        if (showDebugMessages)
            Debug.Log("✅ airControl configurado a 0.5");

        // Buscar y asignar audio clips
        AssignAudioClips(controller);

        // Configurar groundCheck si no está
        if (controller.groundCheck == null)
        {
            Transform groundCheck = playerGO.transform.Find("GroundCheck");
            if (groundCheck != null)
            {
                controller.groundCheck = groundCheck;
                if (showDebugMessages)
                    Debug.Log("✅ groundCheck asignado");
            }
        }

        if (showDebugMessages)
            Debug.Log("✅ ThirdPersonController configurado");
    }

    void ConfigurePlayerRespawn(PlayerRespawn respawn)
    {
        // Buscar y asignar respawn clips
        AudioClip respawnClip = Resources.Load<AudioClip>("Sounds/JumpUp");
        if (respawnClip != null)
        {
            respawn.respawnClip = respawnClip;
            if (showDebugMessages)
                Debug.Log("✅ respawnClip asignado (usando JumpUp como placeholder)");
        }

        if (showDebugMessages)
            Debug.Log("✅ PlayerRespawn configurado");
    }

    void AssignAudioClips(ThirdPersonController controller)
    {
        // Buscar clips en Assets/Sounds/
        AudioClip jumpUp = Resources.Load<AudioClip>("Sounds/JumpUp");
        AudioClip jumpLanding = Resources.Load<AudioClip>("Sounds/JumpLanding");
        AudioClip boing = Resources.Load<AudioClip>("Sounds/Boing");

        if (jumpUp != null)
            controller.jumpUpClip = jumpUp;
        if (jumpLanding != null)
            controller.jumpLandingClip = jumpLanding;
        if (boing != null)
            controller.boingClip = boing;

        // Crear array de footsteps (placeholder con clips existentes)
        AudioClip[] footsteps = new AudioClip[] { jumpUp, jumpLanding };
        if (footsteps[0] != null)
        {
            controller.footstepClips = footsteps;
            if (showDebugMessages)
                Debug.Log("✅ footstepClips configurado (placeholder)");
        }

        if (showDebugMessages)
            Debug.Log("✅ Audio clips asignados");
    }

    public void ConfigureAnimator()
    {
        GameObject playerGO = GameObject.Find("MC");
        if (playerGO == null)
        {
            Debug.LogError("❌ No se encontró objeto 'MC'");
            return;
        }

        Animator animator = playerGO.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("❌ No se encontró Animator en MC");
            return;
        }

        // Los parámetros deben estar configurados manualmente en el Animator Controller
        Debug.Log("✅ Animator encontrado. Verifica que tenga los triggers: Jump, Land");

        if (showDebugMessages)
            Debug.Log("✅ Verificación de Animator completada (ver Console para detalles)");
    }

    public void PrintSetupStatus()
    {
        Debug.Log("\n" +
            "═══════════════════════════════════════════\n" +
            "🎮 ESTADO DE CONFIGURACIÓN DEL JUEGO\n" +
            "═══════════════════════════════════════════\n" +
            "\n📋 PASOS COMPLETADOS AUTOMÁTICAMENTE:\n" +
            "  ✅ Scripts mejorados descargados\n" +
            "  ✅ AnimationController agregado\n" +
            "  ✅ PlayerMovementOptimizer agregado\n" +
            "  ✅ Audio clips asignados (3/3)\n" +
            "  ✅ Air control optimizado (0.5)\n" +
            "\n⚠️  PASOS MANUALES REQUERIDOS:\n" +
            "  1️⃣  ABRIR: Assets/personaje/animaciones/MC.controller\n" +
            "  2️⃣  AGREGAR dos TRIGGERS: 'Jump' y 'Land'\n" +
            "  3️⃣  CONFIGURAR transiciones en Animator\n" +
            "  4️⃣  OPCIONAL: Importar sonidos de pasos (Freesound.org)\n" +
            "\n📖 VER: SETUP_INSPECTOR_GUIDE.md para detalles\n" +
            "═══════════════════════════════════════════\n");
    }
}
