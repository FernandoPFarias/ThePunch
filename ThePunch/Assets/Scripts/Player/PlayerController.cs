using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 5f;

    [Header("Joystick Virtual (opcional)")]
    public VirtualJoystick virtualJoystick;

    [Header("Soco Automático")]
    public float punchRange = 2f;
    public LayerMask enemyLayer;
    public float punchCooldown = 1f;
    private float lastPunchTime = -10f;

    [Header("Punch Hitbox")]
    public PunchHitbox punchHitbox; // arraste o filho PunchHitbox aqui

    private Vector2 moveInput;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private Animator animator;
    private float punchCheckTimer = 0f;
    private float punchCheckInterval = 0.1f;
    private bool wasMoving = false;
    private float stepTimer = 0f;
    private float stepInterval = 0.4f; // ajuste conforme o ritmo do passo

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        if (characterController == null)
        {
            Debug.LogError("[PlayerController] CharacterController não encontrado! Adicione o componente CharacterController ao GameObject.", this);
        }
        if (playerInput == null)
        {
            Debug.LogError("[PlayerController] PlayerInput não encontrado! Adicione o componente PlayerInput ao GameObject.", this);
        }
        if (animator == null)
        {
            Debug.LogWarning("[PlayerController] Animator não encontrado! O personagem não terá animações.", this);
        }
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed += OnMove;
            playerInput.actions["Move"].canceled += OnMove;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed -= OnMove;
            playerInput.actions["Move"].canceled -= OnMove;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (characterController == null) return;

        Vector2 input = moveInput;
        // Prioriza joystick virtual apenas se estiver ativo, senão usa teclado/controle
        if (virtualJoystick != null && virtualJoystick.IsActive)
        {
            input = virtualJoystick.Direction();
        }

        Vector3 move = new Vector3(-input.x, 0, -input.y);
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Rotaciona o player na direção do movimento, se houver movimento
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Atualiza o parâmetro de animação F_Speed
        if (animator != null)
        {
            animator.SetFloat("F_Speed", move.magnitude);
        }

        // --- PASSOS DO PLAYER ---
        bool isMoving = move.sqrMagnitude > 0.01f;
        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                stepTimer = 0f;
                if (MusicManager.Instance != null && MusicManager.Instance.stepClip != null)
                    MusicManager.Instance.PlaySFX(MusicManager.Instance.stepClip);
            }
        }
        else
        {
            stepTimer = stepInterval; // reinicia para tocar imediatamente ao voltar a andar
        }
        wasMoving = isMoving;

        // --- SOCAR AUTOMATICAMENTE INIMIGOS PRÓXIMOS ---
        punchCheckTimer += Time.deltaTime;
        if (punchCheckTimer >= punchCheckInterval && Time.time - lastPunchTime > punchCooldown)
        {
            punchCheckTimer = 0f;
            Collider[] hits = Physics.OverlapSphere(transform.position, punchRange, enemyLayer);
            if (hits.Length > 0)
            {
                if (animator != null)
                {
                    // Só soca se não estiver no estado de soco
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("T_Punch") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
                    {
                        animator.SetTrigger("T_Punch");
                        lastPunchTime = Time.time;
                    }
                }
            }
        }
    }

    // Visualização do alcance do soco no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, punchRange);  
    
    }

    // Métodos para Animation Events
    public void ActivatePunchHitbox()
    {
        if (punchHitbox != null)
        {
            bool acertou = punchHitbox.ActivateHitbox(gameObject);
            if (acertou)
            {
                PlayPunchSound();
            }
        }
    }

    public void DeactivatePunchHitbox()
    {
        if (punchHitbox != null)
            punchHitbox.DeactivateHitbox();
    }

    // PlayPunchSound agora só é chamado internamente
    private void PlayPunchSound()
    {
        if (MusicManager.Instance != null && MusicManager.Instance.punchClip != null)
            MusicManager.Instance.PlaySFX(MusicManager.Instance.punchClip);
    }
} 