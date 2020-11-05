using UnityEngine;
using UnityEngine.Networking;

public class NetUnitSetup : NetworkBehaviour
{
    [SerializeField] private MonoBehaviour[] _disableBihaviours;

    private void Awake()
    {
        for (int i = 0; i < _disableBihaviours.Length; i++)
        {
            _disableBihaviours[i].enabled = false;
        }
    }

    public override void OnStartServer()
    {
        for (int i = 0; i < _disableBihaviours.Length; i++)
        {
            _disableBihaviours[i].enabled = true;
        }
    }
}
