using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float CameraTransitionSpeed = 0.01f;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineFramingTransposer _transposer;
    private Coroutine Up, Down,Right, Left,TimerCor;
    [HideInInspector] public bool Lock;

    public static CameraMove Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        if (!Lock)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Up = StartCoroutine(IncreaseScreen(_transposer.m_ScreenY, false));
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (Up != null) StopCoroutine(Up);
                StartCoroutine(DecreaseeScreen(_transposer.m_ScreenY, false, 0.5f, true));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Down = StartCoroutine(DecreaseeScreen(_transposer.m_ScreenY, false));
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (Down != null) StopCoroutine(Down);
                StartCoroutine(IncreaseScreen(_transposer.m_ScreenY, false, 0.5f, true));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Right = StartCoroutine(DecreaseeScreen(_transposer.m_ScreenX));
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (Right != null) StopCoroutine(Right);
                StartCoroutine(IncreaseScreen(_transposer.m_ScreenX, true, 0.5f, true));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Left = StartCoroutine(IncreaseScreen(_transposer.m_ScreenX));
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (Left != null) StopCoroutine(Left);
                StartCoroutine(DecreaseeScreen(_transposer.m_ScreenX, true, 0.5f, true));
            }
        }
    }
    private IEnumerator IncreaseScreen(float Screen,bool ItX = true,float ToNum = 1f, bool UnFreeze = false)
    {
        if (GroundCheck.Instance.GetOnGround())
        {
          //  Movement.Instance.Freeze();
            while (Screen < ToNum)
            {
                Screen += CameraTransitionSpeed;
                if (ItX)
                {
                    _transposer.m_ScreenX = Screen;
                }
                else
                {
                    _transposer.m_ScreenY = Screen;
                }
                yield return null;
            }
            if (UnFreeze) Player.Instance.UnFreeze();
            yield break;
        }

    }
    private IEnumerator DecreaseeScreen(float Screen, bool ItX = true, float ToNum = 0, bool UnFreeze = false)
    {
        if (GroundCheck.Instance.GetOnGround())
        {
           // Movement.Instance.Freeze();
            while (Screen > ToNum)
            {
                Screen -= CameraTransitionSpeed;
                if (ItX)
                {
                    _transposer.m_ScreenX = Screen;
                }
                else
                {
                    _transposer.m_ScreenY = Screen;
                }
                yield return null;
            }
            if (UnFreeze) Player.Instance.UnFreeze();
            yield break;
        }

    }
    public void MoveRight()
    {
        Right = StartCoroutine(DecreaseeScreen(_transposer.m_ScreenX));
    }
    public void ReturnRight()
    {
        if (Right != null) StopCoroutine(Right);
        StartCoroutine(IncreaseScreen(_transposer.m_ScreenX, true, 0.5f, true));
    }
}