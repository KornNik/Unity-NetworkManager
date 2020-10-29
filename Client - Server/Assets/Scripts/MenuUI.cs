using UnityEngine;
using UnityEngine.Networking;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private GameObject _menuPanel;

    private void Start()
    {
        if ((NetworkManager.singleton as MyNetworkManager).serverMode) _menuPanel.SetActive(false);
    }

    public void Disconnect()
    {
        if (NetworkManager.singleton.IsClientConnected())
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
