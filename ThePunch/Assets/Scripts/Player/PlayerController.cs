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
        if (virtualJoystick != null && virtualJoystick.IsActive)
        {
            input = virtualJoystick.Direction();
        }

        Vector3 move = new Vector3(input.x, 0, input.y);
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
        // (Removido: soco automático por OverlapSphere/punchRange)
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
            punchHitbox.ActivateHitbox(gameObject);
    }

    public void DeactivatePunchHitbox()
    {
        if (punchHitbox != null)
            punchHitbox.DeactivateHitbox();
    }
} 