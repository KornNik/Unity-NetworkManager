using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] LayerMask movementMask;

    private Camera _camera;
    private Character _character;
    private int _leftMouseBtn = (int)MouseButton.LeftButton;
    private int _rightMouseBtn = (int)MouseButton.RightButton;


    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (_character != null&& !EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(_leftMouseBtn))
                {
                    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, movementMask))
                    { CmdSetMovePoint(hit.point); }
                }
                if (Input.GetMouseButtonDown(_rightMouseBtn))
                {
                    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, ~(1 << LayerMask.NameToLayer("Player"))))
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            CmdSetFocus(interactable.GetComponent<NetworkIdentity>());
                        }
                    }
                }
            }
        }
    }

    public void SetCharacter(Character character, bool isLocalPlayer)
    {
        _character = character;
        if (isLocalPlayer) { _camera.GetComponent<CameraController>().target = character.transform; }
    }

    [Command]
    public void CmdSetMovePoint(Vector3 point)
    {
        _character.SetMovePoint(point);
    }

    [Command]
    public void CmdSetFocus(NetworkIdentity newFocus)
    {
        _character.SetNewFocus(newFocus.GetComponent<Interactable>());
    }
}
