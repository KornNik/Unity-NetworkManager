using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginIU : MonoBehaviour
{

    [SerializeField] private GameObject _curPanel;
    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private GameObject _registerPanel;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private InputField _loginLogin;
    [SerializeField] private InputField _loginPass;
    [SerializeField] private InputField _registerLogin;
    [SerializeField] private InputField _registerPass;
    [SerializeField] private InputField _registerConfirm;

    MyNetworkManager mgr;

    private void Start()
    {
        mgr = NetworkManager.singleton as MyNetworkManager;
        if (mgr.ServerMode)
        {
            _loginPanel.SetActive(false);
        }
        else
        {
            mgr.loginResponse = LoginResponse;
            mgr.registerResponse = RegisterResponse;
        }
    }

    void ClearInputs()
    {
        _loginLogin.text = "";
        _loginPass.text = "";
        _registerLogin.text = "";
        _registerPass.text = "";
        _registerConfirm.text = "";
    }

    public void Login()
    {
        mgr.Login(_loginLogin.text, _loginPass.text);
        _curPanel.SetActive(false);
        _loadingPanel.SetActive(true);
    }

    public void Register()
    {
        if (_registerPass.text != "" && _registerPass.text == _registerConfirm.text)
        {
            mgr.Register(_registerLogin.text, _registerPass.text);
            _curPanel.SetActive(false);
            _loadingPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Error: Password Incorrect");
            ClearInputs();
        }
    }

    public void LoginResponse(string response)
    {
        switch (response)
        {
            case "UserError": Debug.Log("Error: Username not Found"); break;
            case "PassError": Debug.Log("Error: Password Incorrect"); break;
            default: Debug.Log("Error: Unknown Error. Please try again later."); break;
        }
        _loadingPanel.SetActive(false);
        _curPanel.SetActive(true);
        ClearInputs();
    }

    public void RegisterResponse(string response)
    {
        switch (response)
        {
            case "Success": Debug.Log("User registered"); break;
            case "UserError": Debug.Log("Error: Username Already Taken"); break;
            default: Debug.Log("Error: Unknown Error. Please try again later."); break;
        }
        _loadingPanel.SetActive(false);
        _curPanel.SetActive(true);
        ClearInputs();
    }

    public void SetPanel(GameObject panel)
    {
        _curPanel.SetActive(false);
        _curPanel = panel;
        _curPanel.SetActive(true);
        ClearInputs();
    }
}
