﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private Player _player;

    public override void OnStartAuthority()
    {
        CmdCreatePlayer();
    }

    public Character CreateCharacter()
    {
        UserAccount acc = AccountManager.GetAccount(connectionToClient);
        GameObject unit;
        Character tempCharacter;
        if (acc.Data.JustCreated)
        {
            unit = Instantiate(_unitPrefab, transform.position, Quaternion.identity, transform);
            acc.Data.JustCreated = false;
            acc.Data.PosCharacter = transform.position;
            acc.Data.SpawnPosition = transform.position;
        }
        else
        {
            unit = Instantiate(_unitPrefab, acc.Data.PosCharacter, Quaternion.identity, transform);
        }
        tempCharacter = unit.GetComponent<Character>();
        tempCharacter.SetRespawnPosition(acc.Data.SpawnPosition);
        NetworkServer.Spawn(unit);
        TargetLinkCharacter(connectionToClient, unit.GetComponent<NetworkIdentity>());
        return tempCharacter;

    }
    public override bool OnCheckObserver(NetworkConnection connection)
    {
        return false;
    }

    private void OnDestroy()
    {
        if (isServer && _player.Character != null)
        {
            UserAccount acc = AccountManager.GetAccount(connectionToClient);
            acc.Data.PosCharacter = _player.Character.transform.position;
            Destroy(_player.Character.gameObject);
            NetworkManager.singleton.StartCoroutine(acc.Quit());
        }
    }

    [Command]
    public void CmdCreatePlayer()
    {
        Character character = CreateCharacter();
        _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), isLocalPlayer);
        _controller.SetCharacter(character, isLocalPlayer);
    }

    [TargetRpc]
    void TargetLinkCharacter(NetworkConnection target, NetworkIdentity unit)
    {
        Character character = unit.GetComponent<Character>();
        _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
        _controller.SetCharacter(character, true);
    }
}
