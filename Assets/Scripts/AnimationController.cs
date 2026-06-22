using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Estados de Animación")]
    [Range(0.1f, 0.5f)] public float transitionDuration = 0.15f;
    [Range(0.1f, 0.3f)] public float landingDuration = 0.2f;

    private Animator animator;
    private ThirdPersonController controller;
    private float lastAnimationStateTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<ThirdPersonController>();

        if (animator == null)
        {
            enabled = false;
            return;
        }

        // Configurar parámetros si no existen
        if (animator.HasParameter("Speed"))
            animator.SetFloat("Speed", 0f);
    }

    void LateUpdate()
    {
        if (animator == null) return;

        // Actualizar transiciones suaves
        UpdateAnimationTransitions();
    }

    void UpdateAnimationTransitions()
    {
        // Las transiciones se controlan a través de los triggers y bools
        // que establece ThirdPersonController en Update()

        // Mejorar la duración de transiciones basadas en estado
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Acelerar transiciones en aire
        if (!animator.GetBool("isGrounded"))
        {
            animator.speed = 1.2f;
        }
        else
        {
            animator.speed = 1f;
        }
    }

    public void SetMovementSpeed(float speed)
    {
        if (animator != null && animator.HasParameter("Speed"))
            animator.SetFloat("Speed", Mathf.Clamp01(speed / 10f), transitionDuration, Time.deltaTime);
    }

    public void ResetAnimationState()
    {
        if (animator == null) return;

        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Land");
    }
}
