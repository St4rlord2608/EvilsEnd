using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    [SerializeField] private float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    [SerializeField] private float SprintSpeed = 5.335f;

    [Tooltip("Crouch speed of the Character in m/s")]
    [SerializeField] private float CrouchSpeed = 1.5f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    [SerializeField] private float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    [SerializeField] private float SpeedChangeRate = 10.0f;

    //[Space(10)]
    //[Tooltip("The height the player can jump")]
    //[SerializeField] private float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [SerializeField] private float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField] private float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    [SerializeField] private float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField] private bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    [SerializeField] private float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    [SerializeField] private float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    [SerializeField] private LayerMask GroundLayers;

    [SerializeField] private float maxCrouchingNoise = 2f;
    [SerializeField] private float maxWalkingNoise = 5f;
    [SerializeField] private float maxSprintingNoise = 10f;
    [SerializeField] private float rbPushForce = 1f;

    public bool isHolsteredWalkingForward = false;
    public bool isAiming = false;
    public bool isHolsteredRunningForward = false;
    public bool isCrouching = false;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    private float currentMovementNoise;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private CharacterController _controller;
    private GameObject _mainCamera;

    private PlayerStamina playerStamina;


    private void Awake()
    {
        playerStamina = GetComponent<PlayerStamina>();
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        
    }

    private void GameInput_OnCrouch(object sender, System.EventArgs e)
    {
        isCrouching = !isCrouching;
    }

    private void Start()
    {
        GameInput.Instance.OnCrouch += GameInput_OnCrouch;
        _controller = GetComponent<CharacterController>();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    private void LateUpdate()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if(rb != null )
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0f;
            forceDirection.Normalize();

            rb.AddForceAtPosition(forceDirection * rbPushForce, transform.position, ForceMode.Impulse);
        }
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

    }

    public float GetGroundRadius()
    {
        return GroundedRadius;
    }

    public float GetGroundOffset()
    {
        return GroundedOffset;
    }


    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = 0;
        if(isCrouching)
        {
            targetSpeed = CrouchSpeed;
        }
        else if (IsSprinting())
        {
            targetSpeed = SprintSpeed;
        }
        else
        {
            targetSpeed = MoveSpeed;
        }

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (GameInput.Instance.GetMovementVector() == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        if(_speed != 0.0f && isCrouching)
        {
            currentMovementNoise = maxCrouchingNoise;
        }
        else if(_speed != 0.0f && IsSprinting())
        {
            currentMovementNoise = maxSprintingNoise;
        }else if(_speed != 0.0f)
        {
            currentMovementNoise = maxWalkingNoise;
        }
        else
        {
            currentMovementNoise = 0.0f;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(GameInput.Instance.GetMovementVector().x, 0.0f, GameInput.Instance.GetMovementVector().y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (GameInput.Instance.GetMovementVector() != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            if (!isAiming)
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                if (IsSprinting())
                {
                    isHolsteredRunningForward = true;
                    isHolsteredWalkingForward = false;
                }
                else
                {
                    isHolsteredRunningForward = false;
                    isHolsteredWalkingForward = true;
                }
                
            }
            else
            {
                isHolsteredWalkingForward = false;
                isHolsteredRunningForward = false;
            }
        }
        else
        {
            isHolsteredWalkingForward = false;
            isHolsteredRunningForward = false;
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

    }

    public bool IsSprinting()
    {
        return GameInput.Instance.GetSprint() == 1 && !isCrouching && playerStamina.HasStamina();
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

           /* // Jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }*/

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }

            // if we are not grounded, do not jump
            //_input.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        //Gizmos.DrawSphere(
        //    new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
        //    GroundedRadius);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {

    }

    private void OnLand(AnimationEvent animationEvent)
    {

    }

    public float GetCurrentMovementNoise()
    {
        return currentMovementNoise;
    }

    public float GetCurrentMovementSpeed()
    {
        return _speed;
    }
}
