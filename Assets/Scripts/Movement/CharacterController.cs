using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D) , typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour
{
	#region Inspector

	[Header("Movement")]
	[SerializeField] private float moveSpeed = 8f;
	[SerializeField] private float jumpForce = 14f;

	[Header("Acceleration")]
	[SerializeField] private float groundAcceleration = 80f;
	[SerializeField] private float groundDeceleration = 70f;
	[SerializeField] private float airAcceleration = 40f;
	[SerializeField] private float airDeceleration = 30f;

	[Header("Apex Boost")]
	[SerializeField] private float apexThreshold = 1f;
	[SerializeField] private float apexSpeedMultiplier = 1.15f;

	[Header("Jump Forgiveness")]
	[SerializeField] private float jumpBufferTime = 0.15f;
	[SerializeField] private float coyoteTime = 0.15f;

	[Header("Better Jump")]
	[SerializeField] private float fallMultiplier = 2.5f;
	[SerializeField] private float lowJumpMultiplier = 2f;
	[SerializeField] private float hangGravityMultiplier = 0.5f;
	[SerializeField] private float hangThreshold = 1f;

	[Header("Corner Correction")]
	[SerializeField] private float cornerCorrectionDistance = 0.2f;
	[SerializeField] private int cornerCorrectionSteps = 4;

	[Header("Ground Snap")]
	[SerializeField] private float groundSnapDistance = 0.15f;
	[SerializeField] private float maxSnapSpeed = -4f;

	[Header("Ground Check")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundRadius = 0.2f;
	[SerializeField] private LayerMask groundLayer;


	#endregion

	#region Private Fields

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private PlayerInput playerInput;

	private InputAction moveAction;
	private InputAction jumpAction;

	private Vector2 moveInput;
	private bool isGrounded;

	private float jumpBufferCounter;
	private float coyoteCounter;

	float apexPoint;
	float apexBonus;

	public bool IsFacingRight = true;


	#endregion

	#region Unity Lifecycle

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
		playerInput = GetComponent<PlayerInput>();

		moveAction = playerInput.actions["Move"];
		jumpAction = playerInput.actions["Jump"];
	}

	private void Update()
	{
		ReadInput();
		UpdateTimers();
	}

	private void FixedUpdate()
	{
		CheckGround();

		HandleMovement();
		HandleJump();

		ApplyBetterJumpPhysics();
		ApplyGroundSnap();
		ApplyCornerCorrection();

		HandleFlip();
	}

	#endregion

	#region Input

	private void ReadInput()
	{
		moveInput = moveAction.ReadValue<Vector2>();

		if (jumpAction.WasPressedThisFrame())
			jumpBufferCounter = jumpBufferTime;
	}

	private void UpdateTimers()
	{
		jumpBufferCounter -= Time.deltaTime;
		coyoteCounter -= Time.deltaTime;
	}

	#endregion

	#region Ground

	private void CheckGround()
	{
		isGrounded = Physics2D.OverlapCircle(
			groundCheck.position ,
			groundRadius ,
			groundLayer
		);

		if (isGrounded)
			coyoteCounter = coyoteTime;
	}

	#endregion

	#region Movement

	private void HandleMovement()
	{
		float targetSpeed = moveInput.x * moveSpeed;
		float speedDiff = targetSpeed - rb.linearVelocity.x;

		// Apex boost (smooth version)
		float speedMultiplier = 1f;

		if (!isGrounded && Mathf.Abs(rb.linearVelocity.y) < apexThreshold)
		{
			apexPoint = Mathf.InverseLerp(apexThreshold , 0 , Mathf.Abs(rb.linearVelocity.y));
			speedMultiplier = 1 + ( apexSpeedMultiplier - 1 ) * apexPoint;
		}

		targetSpeed *= speedMultiplier;
		speedDiff = targetSpeed - rb.linearVelocity.x;

		// Choose accel rate
		float accelRate;

		if (isGrounded)
			accelRate = ( Mathf.Abs(targetSpeed) > 0.01f ) ? groundAcceleration : groundDeceleration;
		else
			accelRate = ( Mathf.Abs(targetSpeed) > 0.01f ) ? airAcceleration : airDeceleration;

		// Apply acceleration
		float movement = speedDiff * accelRate * Time.fixedDeltaTime;

		rb.linearVelocity = new Vector2(
			rb.linearVelocity.x + movement ,
			rb.linearVelocity.y
		);
	}

	private void HandleJump()
	{
		if (jumpBufferCounter > 0 && coyoteCounter > 0)
		{
			rb.linearVelocity = new Vector2(
				rb.linearVelocity.x ,
				jumpForce
			);

			jumpBufferCounter = 0;
			coyoteCounter = 0;
		}
	}

	#endregion

	#region Better Jump Physics

	private void ApplyBetterJumpPhysics()
	{
		float yVel = rb.linearVelocity.y;

		// Fast fall
		if (yVel < 0)
		{
			rb.linearVelocity += Vector2.up *
				Physics2D.gravity.y *
				( fallMultiplier - 1 ) *
				Time.fixedDeltaTime;
		}
		// Short hop
		else if (yVel > 0 && !jumpAction.IsPressed())
		{
			rb.linearVelocity += Vector2.up *
				Physics2D.gravity.y *
				( lowJumpMultiplier - 1 ) *
				Time.fixedDeltaTime;
		}

		// Apex hang
		if (Mathf.Abs(yVel) < hangThreshold)
		{
			rb.linearVelocity += Vector2.up *
				Physics2D.gravity.y *
				( hangGravityMultiplier - 1 ) *
				Time.fixedDeltaTime;
		}
	}

	#endregion

	#region Ground Snap

	private void ApplyGroundSnap()
	{
		if (isGrounded) return;
		if (rb.linearVelocity.y > 0) return;
		if (rb.linearVelocity.y < maxSnapSpeed) return;

		RaycastHit2D hit = Physics2D.BoxCast(
			boxCollider.bounds.center ,
			boxCollider.bounds.size ,
			0f ,
			Vector2.down ,
			groundSnapDistance ,
			groundLayer
		);

		if (hit)
		{
			transform.position -= new Vector3(0 , hit.distance , 0);
			rb.linearVelocity = new Vector2(rb.linearVelocity.x , 0f);
		}
	}

	#endregion

	#region Corner Correction

	private void ApplyCornerCorrection()
	{
		if (rb.linearVelocity.y <= 0) return;

		Bounds bounds = boxCollider.bounds;

		bool blockedAbove = Physics2D.BoxCast(
			bounds.center ,
			bounds.size ,
			0f ,
			Vector2.up ,
			0.05f ,
			groundLayer
		);

		if (!blockedAbove) return;

		for (int i = 1; i <= cornerCorrectionSteps; i++)
		{
			float offset = ( cornerCorrectionDistance / cornerCorrectionSteps ) * i;

			if (!IsBlocked(bounds.center + Vector3.right * offset))
			{
				transform.position += Vector3.right * offset;
				return;
			}

			if (!IsBlocked(bounds.center + Vector3.left * offset))
			{
				transform.position += Vector3.left * offset;
				return;
			}
		}
	}

	private bool IsBlocked( Vector3 position )
	{
		return Physics2D.BoxCast(
			position ,
			boxCollider.bounds.size ,
			0f ,
			Vector2.up ,
			0.05f ,
			groundLayer
		);
	}

	#endregion

	#region Facing

	private void HandleFlip()
	{
		if (moveInput.x > 0 && !IsFacingRight)
			Flip();
		else if (moveInput.x < 0 && IsFacingRight)
			Flip();
	}

	private void Flip()
	{
		IsFacingRight = !IsFacingRight;

		Vector3 rot = transform.eulerAngles;
		rot.y = IsFacingRight ? 0f : 180f;
		transform.eulerAngles = rot;
	}

	#endregion
}
