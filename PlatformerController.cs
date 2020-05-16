using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformerController : MonoBehaviour
{
    private float horizontalInput;

    private bool isGrounded;
    private bool isPressed;
    private bool isJumping;
    private bool isCoyoteJumping;
    private bool hadCoyoteChance;
    private bool isCrouching;
    private bool hasLanded;

    public Rigidbody2D objectRigidbody;
    public Transform objectTransform;
    public Transform feetTransform;
    public Transform headTransform;
    public ParticleSystem landParticles;
    public ParticleSystem moveParticles;

    private Vector3 scaleChange, positionChange;
    public LayerMask jumpSurface;

    public float moveSpeed;
    public float jumpForce;
    public float feetRadius;
    public float headRadius;
    public float coyoteTime;
    public float moveDeceleration;
    public float jumpMemoryTime;

    private float jumpForceDecrement;
    private float originJumpForce;

    private float transformScaleX;
    private float transformScaleY;

    private float moveSpeedCurrent;
    private float moveSpeedInitial;
    private float coyoteTimer;
    private float moveDecay;
    private float jumpMemoryTimer;
    private float impactForceTimer;

    private Transform currentTransform;
    private float lastYTransform;
    private float lastXTransform;

    // Start is called before the first frame update
    private void Start()
    {
        originJumpForce = jumpForce;

        moveSpeedCurrent = 4.0f;
        moveSpeedInitial = (moveSpeed / 1.5f);
        moveDecay = 0.0f;

        moveDeceleration = 32.0f;

        currentTransform = transform;
        lastYTransform = currentTransform.position.y;
        lastXTransform = currentTransform.position.x;

        hasLanded = true;

        landParticles.Pause();
        moveParticles.Pause();
    }

    //Update is called once per client-side frame (used for input)
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && isCoyoteJumping == false && isCrouching == false)
        {
            objectRigidbody.velocity = new Vector2(objectRigidbody.velocity.x, jumpForce);
            isJumping = true;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true && jumpForce > 0.0f && isCoyoteJumping == false
            && isCrouching == false)
        {
            objectRigidbody.velocity = new Vector2(objectRigidbody.velocity.x, jumpForce);
            if (jumpForce > 0.0f)
            {
                jumpForceDecrement += (Time.deltaTime / 2.7f);
                jumpForce -= jumpForceDecrement;
            }
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            isCrouching = true;
        }
        else isCrouching = false;

        if (Input.GetKeyDown(KeyCode.S))
        {
            CrouchPush();
        }

        if(Input.GetKeyUp(KeyCode.S))
        {
            CrouchPull();
        }

        if(isPressed == true)
        {
            isJumping = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (coyoteTimer > 0.0f)
        {
            coyoteTimer -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Space) && isCoyoteJumping == false && isCrouching == false)
            {
                coyoteTimer = coyoteTime;
                isCoyoteJumping = true;
                objectRigidbody.velocity = new Vector2(objectRigidbody.velocity.x, jumpForce);
                isJumping = true;
            }

            if (Input.GetKey(KeyCode.Space) && isJumping == true && jumpForce > 0.0f && isCrouching == false)
            {
                objectRigidbody.velocity = new Vector2(objectRigidbody.velocity.x, jumpForce);
                isCoyoteJumping = true;
                if (jumpForce > 0.0f)
                {
                    jumpForceDecrement += (Time.deltaTime / 2.7f);
                    jumpForce -= jumpForceDecrement;
                }
            }
        }

        if (jumpForce < 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded == false && isJumping == false && jumpForce == originJumpForce && hadCoyoteChance == false)
        {
            if (coyoteTimer <= 0.0f)
            {
                coyoteTimer = coyoteTime;
            }

            if (coyoteTimer == coyoteTime)
            {
                hadCoyoteChance = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == false)
        {
            jumpMemoryTimer = jumpMemoryTime;
        }

        if (((Input.GetKeyUp(KeyCode.A) && Input.GetKey(KeyCode.D) == false) || 
            (Input.GetKeyUp(KeyCode.D) && Input.GetKey(KeyCode.A) == false)))
        {
            MoveDecay();
        }
    }

    //Fixed update is called once per fixed frame within the program (used for physics)
    private void FixedUpdate()
    {
        objectRigidbody.velocity = new Vector2((horizontalInput * moveSpeedCurrent), objectRigidbody.velocity.y);

        if (moveSpeedCurrent < moveSpeed && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            moveSpeedCurrent += (Time.deltaTime * (moveSpeed / 2));
        }

        if ((Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false) && isCrouching == false)
        {
            moveSpeedCurrent = moveSpeedInitial;
        }

        if (moveDecay > 1.0f)
            moveDecay -= (Time.deltaTime * moveDeceleration);
        else if (moveDecay < -1.0f)
            moveDecay += (Time.deltaTime * moveDeceleration);
        else
            moveDecay = 0.0f;

        if (isCrouching == true)
        {
            scaleChange = new Vector3(1.5f, 1.0f, 1.0f);
            objectTransform.localScale = scaleChange;
            moveSpeedCurrent = moveSpeedInitial;
        }
        else
        {
            scaleChange = new Vector3(1.0f, 1.5f, 1.0f);
            objectTransform.localScale = scaleChange;
        }

        isGrounded = Physics2D.OverlapCircle(feetTransform.position, feetRadius, jumpSurface);

        isPressed = Physics2D.OverlapCircle(headTransform.position, headRadius, jumpSurface);

        if (isGrounded == true && Input.GetKey(KeyCode.Space) == false)
        {
            jumpForce = originJumpForce;
            jumpForceDecrement = 0.0f;
            isCoyoteJumping = false;
            hadCoyoteChance = false;
        }

        if ((moveDecay > 0.0f || moveDecay < 0.0f) && Input.GetAxisRaw("Horizontal") == 0.0f)
            objectRigidbody.velocity += new Vector2(moveDecay, 0.0f);

        if (jumpMemoryTimer > 0.0f)
            jumpMemoryTimer -= Time.deltaTime;

        if (jumpMemoryTimer > 0.0f && isGrounded == true && isCoyoteJumping == false && isCrouching == false)
        {
            objectRigidbody.velocity = new Vector2(objectRigidbody.velocity.x, jumpForce);
            isJumping = true;
        }

        if((isJumping == true && isCrouching == false))
        {
            JumpStretch();
        }

        if(impactForceTimer > 0.0f && isGrounded == true && isCrouching == false)
        {
            LandSquash();
            impactForceTimer -= Time.deltaTime;
        }

        if (isGrounded == false)
        {
            hasLanded = false;
        }

        if(isGrounded == true && hasLanded == false)
        {
            Land();
            hasLanded = true;
            landParticles.Play();
        }

        if (isGrounded == true && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            && lastXTransform != currentTransform.position.x)
        {
            moveParticles.Play();
        }
        else
            moveParticles.Pause();

        lastYTransform = currentTransform.position.y;
        lastXTransform = currentTransform.position.x;
    }

    public void CrouchPush()
    {
        objectTransform.position += new Vector3(0.0f, -0.25f, 0.0f);
    }
    public void CrouchPull()
    {
        objectTransform.position += new Vector3(0.0f, 0.25f, 0.0f);
    }
    public void MoveDecay()
    {
        if(objectRigidbody.velocity.x > 0.0f)
            moveDecay = moveSpeedCurrent / 2;
        else
            moveDecay = -(moveSpeedCurrent / 2);
    }
    public void JumpStretch()
    {
        transformScaleY = (jumpForce / 30);
        transformScaleX = -(jumpForce / 60);
        scaleChange = new Vector3(transformScaleX, transformScaleY, 1.0f);
        objectTransform.localScale = objectTransform.localScale + scaleChange;
    }
    public void LandSquash()
    {
        transformScaleY = -(impactForceTimer);
        transformScaleX = (impactForceTimer / 2);
        scaleChange = new Vector3(transformScaleX, transformScaleY, 1.0f);
        objectTransform.localScale = objectTransform.localScale + scaleChange;
    }
    public void Land()
    {
        impactForceTimer = Mathf.Abs(currentTransform.position.y - lastYTransform);
    }
}
