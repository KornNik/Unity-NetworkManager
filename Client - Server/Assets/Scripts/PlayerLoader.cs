using UnityEngine;
using UnityEngine.Networking;

public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private Player _player;

    [SyncVar(hook = "HookUnitIdentity")] private NetworkIdentity _unitIdentity;

    public override void OnStartAuthority()
    {
        if (isServer)
        {
            Character character = CreateCharacter();
            _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
            _controller.SetCharacter(character, true);
        }
        else { CmdCreatePlayer(); }
    }

    public Character CreateCharacter()
    {
        GameObject unit = Instantiate(_unitPrefab);
        NetworkServer.Spawn(unit);
        _unitIdentity = unit.GetComponent<NetworkIdentity>();
        return unit.GetComponent<Character>();

    }
    public override bool OnCheckObserver(NetworkConnection connection)
    {
        return false;
    }

    [Command]
    public void CmdCreatePlayer()
    {
        Character character = CreateCharacter();
        _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), false);
        _controller.SetCharacter(character, false);
    }

    [ClientCallback]
    private void HookUnitIdentity(NetworkIdentity unit)
    {
        if(isLocalPlayer)
        {
            _unitIdentity = unit;
            Character character = unit.GetComponent<Character>();
            _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
            _controller.SetCharacter(character, true);
        }
    }
}
