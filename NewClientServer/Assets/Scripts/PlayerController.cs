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

                if (Input.GetButtonDown("Skill1")) { CmdUseSkill(0); }

                if (Input.GetButtonDown("Skill2")) { CmdUseSkill(1); }

                if (Input.GetButtonDown("Skill3")) { CmdUseSkill(2); }
            }
        }
    }

    public void SetCharacter(Character character, bool isLocalPlayer)
    {
        _character = character;
        if (isLocalPlayer)
        {
            _camera.GetComponent<CameraController>().target = character.transform;
            SkillsPanel.Instance.SetSkills(character.UnitSkills);
        }
    }

    [Command]
    void CmdSetMovePoint(Vector3 point)
    {
        if (!_character.UnitSkills.InCast) _character.SetMovePoint(point);
    }

    [Command]
    void CmdSetFocus(NetworkIdentity newFocus)
    {
        if (!_character.UnitSkills.InCast)
            _character.SetNewFocus(newFocus.GetComponent<Interactable>());
    }

    [Command]
    void CmdUseSkill(int skillNum)
    {
        if (!_character.UnitSkills.InCast) _character.UseSkill(skillNum);
    }
}
