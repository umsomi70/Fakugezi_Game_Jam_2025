using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public GameObject player;

    [Header("Main Cameras")]
    public float transitionTime = 1f;
    public GameObject mainCamera;
    private Vector3 mainCameraPos;
    private Quaternion mainCameraRot;

    [Header("Camera Positions")]
    [SerializeField] private bool dynamicCamera;
    [SerializeField] private float radius;
    [SerializeField] private float height;
    [SerializeField] private float zoom;
    [SerializeField] private GameObject[] go_cameraPos = new GameObject[0];
    [SerializeField] public Vector3[] cameraPos = new Vector3[4];
    public int cameraIndex = 0;

    [Header("Input")]
    public KeyCode leftCamera = KeyCode.Q;
    public KeyCode rightCamera = KeyCode.E;

    [Header("-------------------")]

    [Header("Input Changes")]


    public KeyCode[] camPos1 = new KeyCode[4]
    {
        KeyCode.D,
        KeyCode.A,
        KeyCode.W,
        KeyCode.S
    };

    public KeyCode[] camPos2 = new KeyCode[]
    {
        KeyCode.S,
        KeyCode.W,
        KeyCode.D,
        KeyCode.A
    };

    public KeyCode[] camPos3 = new KeyCode[]
    {
        KeyCode.A,
        KeyCode.D,
        KeyCode.S,
        KeyCode.W
    };

    public KeyCode[] camPos4 = new KeyCode[4]
{
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D
};


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        zoom = mainCamera.GetComponent<Camera>().orthographicSize;


        mainCamera.transform.position = go_cameraPos[0].transform.position;
        mainCamera.transform.rotation = go_cameraPos[0].transform.rotation;
        cameraIndex = 0;

        CameraPosUpdate();

        //ChangeControls(camPos1);
    }
    private void Update()
    {
        CameraPosUpdate();

        if (Input.GetKeyDown(leftCamera))
        {
            mainCameraPos = mainCamera.transform.position;
            mainCameraRot = mainCamera.transform.rotation;

            ChangeCameraPos(-1);
        }
        else if (Input.GetKeyDown(rightCamera))
        {
            mainCameraPos = mainCamera.transform.position;
            mainCameraRot = mainCamera.transform.rotation;

            ChangeCameraPos(1);
        }

        MoveCamera();
    }

    private int ChangeCameraPos(int increaseOrDecrease)
    {
        cameraIndex += increaseOrDecrease;

        if (cameraIndex > 3) cameraIndex = 0;
        else if (cameraIndex < 0) cameraIndex = 3;

        switch (cameraIndex)
        {
            case 0: ChangeControls(camPos1); break;
            case 1: ChangeControls(camPos2); break;
            case 2: ChangeControls(camPos3); break;
            case 3: ChangeControls(camPos4); break;
            default:
                break;
        }

        return cameraIndex;
    }
    private void MoveCamera()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, go_cameraPos[cameraIndex].transform.position, transitionTime * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, go_cameraPos[cameraIndex].transform.rotation, transitionTime * Time.deltaTime);
    }

    private void CameraPosUpdate()
    {
        cameraPos[0] = new Vector3(radius, height, -radius);
        cameraPos[1] = new Vector3(radius, height, radius);
        cameraPos[2] = new Vector3(-radius, height, radius);
        cameraPos[3] = new Vector3(-radius, height, -radius);

        for (int i = 0; i < go_cameraPos.Length; i++)
        {
            if (dynamicCamera)
            {
                go_cameraPos[i].transform.LookAt(player.transform.position);
            }
            else
            {
                go_cameraPos[i].transform.LookAt(Vector3.zero);
            }
            go_cameraPos[i].transform.position = cameraPos[i];
        }


        if (mainCamera.GetComponent<Camera>().orthographicSize >= 0)
        {
            mainCamera.GetComponent<Camera>().orthographicSize += Input.mouseScrollDelta.y;
        }
        else if (mainCamera.GetComponent<Camera>().orthographicSize < 0)
        {
            mainCamera.GetComponent<Camera>().orthographicSize = 1;
        }
    }

    private void ChangeControls(KeyCode[] keyCodes)
    {
        PlayerMovement.Instance.moveInput[0] = keyCodes[0];
        PlayerMovement.Instance.moveInput[1] = keyCodes[1];
        PlayerMovement.Instance.moveInput[2] = keyCodes[2];
        PlayerMovement.Instance.moveInput[3] = keyCodes[3];
    }


}
