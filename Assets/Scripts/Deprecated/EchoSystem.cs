using System;
using UnityEngine;

public class EchoSystem : MonoBehaviour
{
    private int Echoes;
    public GameObject _echo;
    public GameObject[] _prevEchoes = new GameObject[100];

    public static EchoSystem Instance { get; private set; }
    public static event Action<int> DisplayEcho;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DropEchoes();
        }
    }
    private void Awake()
    {
            Instance = this;
    }

    public void AddEcho(int echo)
    {
        Echoes += echo;
        DisplayEcho(echo);
    }
    public void RemoveEcho(int echo)
    {
        Echoes -= echo;
        DisplayEcho(echo);
    }
    public int GetEcho()
    {
        return Echoes;
    }
    public void DropEchoes()
    {
        if (_prevEchoes[0] != null)
        {
            for (int i = 0; i < _prevEchoes.Length; i++)
            {
                Destroy(_prevEchoes[i]);
            }
        }
        if(Echoes != 0)
        {
            for (int i = Echoes; i > 0; i--)
            {
              _prevEchoes[i-1] =  Instantiate(_echo, Player.Instance.transform.position, Quaternion.identity);
                Echoes--;
                DisplayEcho(Echoes);
            }
        }
    }
}
