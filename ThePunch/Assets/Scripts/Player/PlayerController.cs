// ===============================
// PlayerController.cs
// Controla o movimento, rotação, soco automático e integração com joystick virtual do jogador.
// ===============================
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade de movimento do jogador")] public float moveSpeed = 5f;

    [Header("Joystick Virtual (opcional)")]
    [Tooltip("Referência ao joystick virtual, se usado")] public VirtualJoystick virtualJoystick;

    [Header("Soco Automático")]
    [Tooltip("Alcance do soco automático")] public float punchRange = 2f;
    [Tooltip("Layer dos inimigos para detectar soco")] public LayerMask enemyLayer;
    [Tooltip("Tempo de recarga entre socos")] public float punchCooldown = 1f;
    private float lastPunchTime = -10f;

    [Header("Punch Hitbox")]
    [Tooltip("Referência ao PunchHitbox filho")] public PunchHitbox punchHitbox; // arraste o filho PunchHitbox aqui

    private Vector2 moveInput;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private Animator animator;
    private float punchCheckInterval = 0.1f;
    private bool wasMoving = false;
    private float stepTimer = 0f;
    private float stepInterval = 0.4f; // ajuste conforme o ritmo do passo

    private void Awake()
    {
        // Inicializa referências e valida componentes essenciais
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
        // Assina eventos de input
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed += OnMove;
            playerInput.actions["Move"].canceled += OnMove;
        }
    }

    private void OnDisable()
    {
        // Remove eventos de input
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed -= OnMove;
            playerInput.actions["Move"].canceled -= OnMove;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Atualiza o input de movimento
        moveInput = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        // Inicia a rotina de soco automático
        StartCoroutine(AutoPunchRoutine());
    }

    private IEnumerator AutoPunchRoutine()
    {
        // Rotina que verifica periodicamente se há inimigos para socar
        while (true)
        {
            if (animator != null && characterController != null && Time.time - lastPunchTime > punchCooldown)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, punchRange, enemyLayer);
                if (hits.Length > 0)
                {
                    // Só soca se não estiver no estado de soco
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("T_Punch") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
                    {
                        animator.SetTrigger("T_Punch");
                        lastPunchTime = Time.time;
                    }
                }
            }
            yield return new WaitForSeconds(punchCheckInterval);
        }
    }

    private void Update()
    {
        // Controla movimento, rotação, animação e passos do jogador
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
        // Removido: checagem de soco automático daqui
    }

    // Visualização do alcance do soco no editor
    private void OnDrawGizmosSelected()
    {
        // Visualiza o alcance do soco no editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, punchRange);  
    
    }

    // Métodos para Animation Events
    public void ActivatePunchHitbox()
    {
        // Ativa a hitbox do soco via Animation Event
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
        // Desativa a hitbox do soco via Animation Event
        if (punchHitbox != null)
            punchHitbox.DeactivateHitbox();
    }

    // PlayPunchSound agora só é chamado internamente
    private void PlayPunchSound()
    {
        // Toca o som do soco
        if (MusicManager.Instance != null && MusicManager.Instance.punchClip != null)
            MusicManager.Instance.PlaySFX(MusicManager.Instance.punchClip);
    }
} 