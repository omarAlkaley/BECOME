using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CharacterController : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed = 8f;
	public float jumpForce = 14f;

	[Header("Coyote Jump")]
	public float coyoteTime = 0.15f; // grace time after leaving ground
	private float coyoteTimeCounter;

	[Header("Check Player Direction")]
	public bool IsFacingRight = true;
	
	[Header("Ground Check")]
	public Transform groundCheck;
	public float groundRadius = 0.2f;
	public LayerMask groundLayer;

	[Header("Camera Setup")]
	[SerializeField] private GameObject _cameraFollowGO;
	private CameraFollowObject _cameraFollowObject;
	private float _fallSpeedYDampingChangeThreshold;

	private Rigidbody2D rb;
	private Vector2 moveInput;
	private bool isGrounded;
	private bool jumpPressed;


	// Input System
	private PlayerInput playerInput;
	private InputAction moveAction;
	private InputAction jumpAction;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		playerInput = GetComponent<PlayerInput>();
		
		moveAction = playerInput.actions["Move"];
		jumpAction = playerInput.actions["Jump"];
	}

	private void Start()
	{
		_cameraFollowObject= _cameraFollowGO.GetComponent<CameraFollowObject>();
		_fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
	}

	void Update()
	{
		moveInput = moveAction.ReadValue<Vector2>();

		if (jumpAction.WasPressedThisFrame())
			jumpPressed = true;

		if (rb.linearVelocityY < _fallSpeedYDampingChangeThreshold &&  !CameraManager.instance.LerpedFromPlayerFalling)
		{
			CameraManager.instance.LerpYDamping(true);
		}
		if (rb.linearVelocityY >= 0f && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
		{
			CameraManager.instance.LerpedFromPlayerFalling = false;
			CameraManager.instance.LerpYDamping(true);
		}
	}

	void FixedUpdate()
	{
		Move();
		Jump();
		TurnCheck();
	}

	void Move()
	{
		rb.linearVelocity = new Vector2(moveInput.x * moveSpeed , rb.linearVelocity.y);
	}

	void Jump()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position , groundRadius , groundLayer);

		// Coyote time logic
		if (isGrounded)
			coyoteTimeCounter = coyoteTime;
		else
			coyoteTimeCounter -= Time.fixedDeltaTime;

		if (jumpPressed && coyoteTimeCounter > 0f)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x , jumpForce);
			coyoteTimeCounter = 0f; // prevent double jump
		}

		jumpPressed = false;
	}

	private void TurnCheck()
	{
		if (moveInput.x > 0 && !IsFacingRight)
		{
			Turn();
		}
		else if (moveInput.x < 0 && IsFacingRight)
		{
			Turn();
		}
	}

	private void Turn()
	{
		if (IsFacingRight)
		{
			Vector3 rotator = new Vector3(transform.rotation.x , 180f , transform.rotation.z);
			transform.rotation = Quaternion.Euler(rotator);
			IsFacingRight = !IsFacingRight;

			_cameraFollowObject.CallTurn();
		}
		else
		{
			Vector3 rotator = new Vector3(transform.rotation.x , 0f , transform.rotation.z);
			transform.rotation = Quaternion.Euler(rotator);
			IsFacingRight = !IsFacingRight;

			_cameraFollowObject.CallTurn();
		}
	}
}