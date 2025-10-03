using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public Rigidbody rb;
    public GameObject playerGUI;

    [Header("Raycasts")]
    private float playerRadius = 0.25f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask water;
    [SerializeField] private LayerMask seed;
    [SerializeField] private LayerMask watermelon;

    [Header("Input")]
    public KeyCode[] moveInput =
    {
        KeyCode.A,
        KeyCode.D,
        KeyCode.W,
        KeyCode.S
    };
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Jump")]
    [SerializeField] private bool isJumping;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpMove = 0;

    [Header("Movement")]
    public bool canMove = true;

    [SerializeField]
    private Vector3[] direction =
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };
    public Vector3[] rotation =
    {
        new Vector3 (0, 270, 0), //forward
        new Vector3 (0, 90, 0), // back
        new Vector3 (0, 180, 0), // left
        new Vector3 (0, 0, 0) //right
    };
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        FallMassCheck();

        if (canMove)
        {
            if (Input.GetKeyDown(moveInput[0]))
            {
                Move(0);
                Turn(rotation[0]);
            }
            else if (Input.GetKeyDown(moveInput[1]))
            {
                Move(1);
                Turn(rotation[1]);
            }
            else if (Input.GetKeyDown(moveInput[2]))
            {
                Move(2);
                Turn(rotation[2]);
            }
            else if (Input.GetKeyDown(moveInput[3]))
            {
                Move(3);
                Turn(rotation[3]);
            }
        }

        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        if (GroundCheck() /*&& rb.velocity.y == 0*/)
        {
            isJumping = false;
            jumpMove = 0;
        }
        else if (!GroundCheck()) isJumping = true;
    }

    #region Move Pip
    private void Move(int moveDirection)
    {
        Vector3 _direction = direction[moveDirection];

        if (!GroundDetection(_direction, ground) && !ObjectBoxDetection(_direction, seed) && !GroundDetection(_direction, water)) // nothing forward
        {
            if (isJumping)
            {
                jumpMove += 1;

                if (jumpMove < 2)
                {
                    transform.position += _direction;
                }
            }
            else if (WatermelonCheck(moveDirection))
            {
                PushCheck(moveDirection);
            }
            else
            {
                transform.position += _direction;
            }
        }

    }

    private void Turn(Vector3 direction)
    {
        playerGUI.transform.rotation = Quaternion.Euler(direction);
    }

    private void Jump()
    {
        if (GroundCheck())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void PushCheck(int directionInt)
    {
        GameObject watermelon;

        if (WatermelonCheck(directionInt))
        {
            watermelon = WatermelonObject(directionInt);
            if (watermelon.GetComponent<WatermelonLogic>().moveDirection[directionInt] && Input.GetKeyDown(moveInput[directionInt])/* && !watermelon.GetComponent<WatermelonLogic>().falling*/)
            {
                Push(watermelon, directionInt);
            }
        }
    }

    private void Push(GameObject watermelon, int directionInt)
    {
        transform.position += direction[directionInt]; // move player
        watermelon.transform.position += direction[directionInt]; // push watermelon
    }
    #endregion

    private void FallMassCheck()
    {
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * jumpForce, ForceMode.Force);
        }
    }

    #region Raycasts

    private bool ObjectBoxDetection(Vector3 direction, LayerMask layerMask)
    {
        return Physics.BoxCast(transform.position + new Vector3(0, playerRadius, 0), new Vector3(0.5f, 0.5f, 0.5f), direction, Quaternion.identity, playerRadius + 0.5f, layerMask);
    }

    private bool GroundDetection(Vector3 direction, LayerMask layerMask)
    {
        return Physics.Raycast(transform.position, direction, playerRadius + 0.5f, layerMask);
    }

    private GameObject WatermelonObject(int _direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, direction[_direction]);
        Physics.Raycast(ray, out hit, 1f, watermelon);
        if (hit.collider.gameObject != null) return hit.collider.gameObject;
        else return null;
    }

    private bool GroundCheck()
    {
        if (WatermelonCheck()) return true;
        else if (GroundDetection(Vector3.down, seed)) return true;
        else if (GroundDetection(Vector3.down, water)) return true;
        else return Physics.Raycast(transform.position, Vector3.down, 0.3f, ground);
    }

    private bool WatermelonCheck(int _direction)
    {
        return Physics.Raycast(transform.position, direction[_direction], 1f, watermelon);
    }
    private bool WatermelonCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f, watermelon);
    }
    #endregion
}
