using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce = 200f;
    public float wallJumpUpForce = 7f;
    public float wallJumpSideForce = 12f;
    public float wallClimbSpeed = 3f;
    public float maxWallRunTime = 1f;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.None;
    public KeyCode downwardsRunKey = KeyCode.None;

    private bool upwardsRunning;
    private bool downwardsRunning;

    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance = 1.7f;
    public float minJumpHeight = 2f;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime = 0.2f;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity = true;
    public float gravityCounterForce = 6f;

    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(
            transform.position,
            orientation.right,
            out rightWallhit,
            wallCheckDistance,
            whatIsWall
        );
        wallLeft = Physics.Raycast(
            transform.position,
            -orientation.right,
            out leftWallhit,
            wallCheckDistance,
            whatIsWall
        );
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        //Carrera vertical
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();

            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if (Input.GetKeyDown(jumpKey))
                WallJump();
        }
        //Saliendo
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }
        //NO Carrera vertical
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (MomentumScript.Instance != null)
        {
            MomentumScript.Instance.Aumentar(MomentumScript.Instance.altoMmt);
            Debug.Log("Wallrun: +30 momentum");
        }
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if (
            (orientation.forward - wallForward).magnitude
            > (orientation.forward - -wallForward).magnitude
        )
            wallForward = -wallForward;

        //Recto
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        //Arriba y abajo
        if (upwardsRunning)
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                wallClimbSpeed,
                rb.linearVelocity.z
            );

        if (downwardsRunning)
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                -wallClimbSpeed,
                rb.linearVelocity.z
            );

        //Pegarte a la pared
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);

        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
