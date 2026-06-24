using UnityEngine;
using UnityEditor;

public class JAMSetupWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private bool[] completedSteps = new bool[6];
    private bool showAdvanced = false;

    [MenuItem("Tools/JAM Setup Wizard")]
    public static void ShowWindow()
    {
        GetWindow<JAMSetupWindow>("JAM Setup");
    }

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        DrawHeader();
        EditorGUILayout.Space();

        DrawStepByStep();
        EditorGUILayout.Space();

        DrawActionButtons();
        EditorGUILayout.Space();

        DrawChecklist();
        EditorGUILayout.Space();

        DrawTroubleshooting();

        GUILayout.EndScrollView();
    }

    void DrawHeader()
    {
        EditorGUILayout.HelpBox(
            "🎮 JAM Setup Wizard\n\n" +
            "Esta herramienta te guía en la configuración de los componentes mejorados.\n\n" +
            "⏱️  Tiempo estimado: 20-30 minutos",
            MessageType.Info);
    }

    void DrawStepByStep()
    {
        EditorGUILayout.LabelField("📋 PASOS DE CONFIGURACIÓN", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        // Paso 1
        EditorGUILayout.LabelField("Paso 1: Seleccionar MC en Jerarquía", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. En la Jerarquía (panel izquierdo)\n" +
            "2. Busca el objeto 'MC'\n" +
            "3. Haz click en él\n\n" +
            "El Inspector debe mostrar sus componentes",
            MessageType.None);
        completedSteps[0] = EditorGUILayout.Toggle("✅ Completado", completedSteps[0]);
        EditorGUILayout.Space();

        // Paso 2
        EditorGUILayout.LabelField("Paso 2: Configurar ThirdPersonController", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. En el Inspector, busca 'ThirdPersonController'\n" +
            "2. Verifica que airControl = 0.5\n" +
            "3. Asigna audio clips:\n" +
            "   - jumpUpClip → JumpUp.mp3\n" +
            "   - jumpLandingClip → JumpLanding.mp3\n" +
            "   - boingClip → Boing.mp3\n" +
            "4. Configura footstepClips array (Size: 2)\n" +
            "   - Element 0: JumpUp.mp3\n" +
            "   - Element 1: JumpLanding.mp3",
            MessageType.None);
        completedSteps[1] = EditorGUILayout.Toggle("✅ Completado", completedSteps[1]);
        EditorGUILayout.Space();

        // Paso 3
        EditorGUILayout.LabelField("Paso 3: Verificar Componentes", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. En el Inspector de MC, scroll down\n" +
            "2. Verifica que existan:\n" +
            "   - AnimationController\n" +
            "   - PlayerMovementOptimizer\n" +
            "3. Si faltan, haz click en 'Add Component'",
            MessageType.None);
        completedSteps[2] = EditorGUILayout.Toggle("✅ Completado", completedSteps[2]);
        EditorGUILayout.Space();

        // Paso 4
        EditorGUILayout.LabelField("Paso 4: Configurar Animator (⚠️ CRÍTICO)", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "⚠️  Este es el paso más importante\n\n" +
            "1. Selecciona MC en Inspector\n" +
            "2. Busca componente 'Animator'\n" +
            "3. Doble-click en el valor 'Controller'\n" +
            "4. Se abre ventana del Animator\n" +
            "5. En 'Parameters', agrega dos TRIGGERS:\n" +
            "   - Haz click en '+'\n" +
            "   - Selecciona 'Trigger'\n" +
            "   - Nómbralo 'Jump'\n" +
            "   - Repite para 'Land'\n" +
            "6. Configura transiciones:\n" +
            "   - Idle/Walk → Jump (trigger: Jump)\n" +
            "   - Fall → Land (trigger: Land)\n" +
            "   - Land → Idle (isGrounded == true)",
            MessageType.Warning);
        completedSteps[3] = EditorGUILayout.Toggle("✅ Completado", completedSteps[3]);
        EditorGUILayout.Space();

        // Paso 5
        EditorGUILayout.LabelField("Paso 5: Configurar Plataformas Rebotadoras", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. En la Jerarquía, busca cada plataforma ROJA\n" +
            "2. Selecciónala\n" +
            "3. En Inspector, busca 'BouncyPlatform'\n" +
            "4. Asigna valores:\n" +
            "   - boingClip: Boing.mp3\n" +
            "   - boingVolume: 0.8\n" +
            "   - bouncePower: 3.5\n" +
            "   - bounceDuration: 0.15\n" +
            "5. Repite para TODAS las plataformas rojas",
            MessageType.None);
        completedSteps[4] = EditorGUILayout.Toggle("✅ Completado", completedSteps[4]);
        EditorGUILayout.Space();

        // Paso 6
        EditorGUILayout.LabelField("Paso 6: Testear", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. Haz click en PLAY (botón arriba)\n" +
            "2. Intenta:\n" +
            "   - Caminar (escuchar pasos)\n" +
            "   - Saltar (sonido + animación)\n" +
            "   - Aterrizar (sonido diferente)\n" +
            "   - Plataforma roja (sonido 'boing')\n" +
            "3. Haz click en STOP",
            MessageType.None);
        completedSteps[5] = EditorGUILayout.Toggle("✅ Completado", completedSteps[5]);
    }

    void DrawActionButtons()
    {
        EditorGUILayout.LabelField("🎬 HERRAMIENTAS DE AYUDA", EditorStyles.boldLabel);

        if (GUILayout.Button("📖 Abrir Guía de Configuración (Markdown)", GUILayout.Height(30)))
        {
            EditorUtility.RevealInFinder(Application.dataPath + "/../CONFIGURACION_PASO_A_PASO.md");
        }

        if (GUILayout.Button("🔍 Seleccionar MC en Jerarquía", GUILayout.Height(25)))
        {
            GameObject mc = GameObject.Find("MC");
            if (mc != null)
            {
                Selection.activeGameObject = mc;
                EditorGUIUtility.PingObject(mc);
                Debug.Log("✅ MC seleccionado en Jerarquía");
            }
            else
            {
                Debug.LogError("❌ No se encontró objeto 'MC' en la escena");
            }
        }

        if (GUILayout.Button("⚙️  Auto-Asignar Audio Clips (Parcial)", GUILayout.Height(25)))
        {
            AutoAssignAudioClips();
        }

        if (GUILayout.Button("✅ Verificar Estado de Setup", GUILayout.Height(25)))
        {
            VerifySetupStatus();
        }
    }

    void DrawChecklist()
    {
        EditorGUILayout.LabelField("📝 CHECKLIST DE PROGRESO", EditorStyles.boldLabel);

        int completed = 0;
        for (int i = 0; i < completedSteps.Length; i++)
        {
            if (completedSteps[i]) completed++;
        }

        float progress = (float)completed / completedSteps.Length;
        Rect progressRect = EditorGUILayout.GetRect(200, 20);
        EditorGUI.ProgressBar(progressRect, progress, $"{completed}/6 Completado ({(int)(progress * 100)}%)");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pasos completados:");
        for (int i = 0; i < completedSteps.Length; i++)
        {
            string[] stepNames = { "Seleccionar MC", "ThirdPersonController", "Verificar Componentes",
                                   "Configurar Animator", "Plataformas Rebotadoras", "Testear" };
            EditorGUILayout.LabelField($"  {(completedSteps[i] ? "✅" : "⬜")} {stepNames[i]}");
        }
    }

    void DrawTroubleshooting()
    {
        EditorGUILayout.LabelField("🐛 TROUBLESHOOTING", EditorStyles.boldLabel);

        showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Mostrar soluciones comunes");

        if (showAdvanced)
        {
            EditorGUILayout.HelpBox(
                "❌ No se escuchan pasos\n" +
                "→ Verificar footstepClips tiene elementos\n" +
                "→ Verificar footstepVolume > 0.1",
                MessageType.Warning);

            EditorGUILayout.HelpBox(
                "❌ Animación de salto no funciona\n" +
                "→ Verificar Animator tiene triggers Jump/Land\n" +
                "→ Verificar transiciones en Animator",
                MessageType.Warning);

            EditorGUILayout.HelpBox(
                "❌ Plataformas rojas no rebotan\n" +
                "→ Verificar cada una tiene BouncyPlatform\n" +
                "→ Verificar Rigidbody isKinematic = true",
                MessageType.Warning);

            EditorGUILayout.HelpBox(
                "✅ CONSEJO: Abre Window → General → Console\n" +
                "para ver mensajes de error detallados",
                MessageType.Info);
        }
    }

    void AutoAssignAudioClips()
    {
        GameObject mc = GameObject.Find("MC");
        if (mc == null)
        {
            Debug.LogError("❌ No se encontró MC");
            return;
        }

        ThirdPersonController ctrl = mc.GetComponent<ThirdPersonController>();
        if (ctrl == null)
        {
            Debug.LogError("❌ No se encontró ThirdPersonController");
            return;
        }

        // Buscar clips
        AudioClip jumpUp = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/JumpUp.mp3");
        AudioClip landing = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/JumpLanding.mp3");
        AudioClip boing = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/Boing.mp3");

        if (jumpUp != null) ctrl.jumpUpClip = jumpUp;
        if (landing != null) ctrl.jumpLandingClip = landing;
        if (boing != null) ctrl.boingClip = boing;

        if (jumpUp != null && landing != null)
        {
            ctrl.footstepClips = new AudioClip[] { jumpUp, landing };
        }

        EditorUtility.SetDirty(ctrl);
        Debug.Log("✅ Audio clips asignados automáticamente");
    }

    void VerifySetupStatus()
    {
        GameObject mc = GameObject.Find("MC");
        if (mc == null)
        {
            EditorUtility.DisplayDialog("Error", "No se encontró MC en la escena", "OK");
            return;
        }

        string status = "📊 ESTADO DE SETUP:\n\n";

        ThirdPersonController ctrl = mc.GetComponent<ThirdPersonController>();
        status += (ctrl != null ? "✅" : "❌") + " ThirdPersonController\n";

        PlayerRespawn respawn = mc.GetComponent<PlayerRespawn>();
        status += (respawn != null ? "✅" : "❌") + " PlayerRespawn\n";

        AnimationController anim = mc.GetComponent<AnimationController>();
        status += (anim != null ? "✅" : "❌") + " AnimationController\n";

        PlayerMovementOptimizer opt = mc.GetComponent<PlayerMovementOptimizer>();
        status += (opt != null ? "✅" : "❌") + " PlayerMovementOptimizer\n";

        Animator animator = mc.GetComponent<Animator>();
        if (animator != null)
        {
            status += $"\n✅ Animator encontrado\n";
            status += "   (Verifica triggers: Jump, Land en el Controller)\n";
        }

        EditorUtility.DisplayDialog("Setup Status", status, "OK");
    }
}
