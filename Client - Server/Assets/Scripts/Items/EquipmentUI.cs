using UnityEngine;

public class EquipmentUI : MonoBehaviour {

    #region Singleton
    public static EquipmentUI instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one instance of InventoryUI found!");
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] private GameObject _equipmentUI;
    [Space]
    [SerializeField] private EquipmentSlot _headSlot;
    [SerializeField] private EquipmentSlot _chestSlot;
    [SerializeField] private EquipmentSlot _legsSlot;
    [SerializeField] private EquipmentSlot _righHandSlot;
    [SerializeField] private EquipmentSlot _leftHandSlot;

    private Equipment _equipment;
    private EquipmentSlot[] _slots;
    
    private void Start () {
        _equipmentUI.SetActive(false);
        _slots = new EquipmentSlot[System.Enum.GetValues(typeof(EquipmentSlotType)).Length];
        _slots[(int)EquipmentSlotType.Chest] = _chestSlot;
        _slots[(int)EquipmentSlotType.Head] = _headSlot;
        _slots[(int)EquipmentSlotType.LeftHand] = _leftHandSlot;
        _slots[(int)EquipmentSlotType.Legs] = _legsSlot;
        _slots[(int)EquipmentSlotType.RighHand] = _righHandSlot;
    }

    private void Update() {
        if (Input.GetButtonDown("Equipment")) {
            _equipmentUI.SetActive(!_equipmentUI.activeSelf);
        }
    }

    public void SetEquipment(Equipment newEquipment) {
        _equipment = newEquipment;
        _equipment.OnItemChanged += ItemChanged;
        for (int i = 0; i < _slots.Length; i++) if (_slots[i] != null) _slots[i].Equipment = _equipment;
        ItemChanged(0, 0);
    }

    private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex) {
        for (int i = 0; i < _slots.Length; i++) _slots[i].ClearSlot();
        for (int i = 0; i < _equipment.Items.Count; i++) {
            _slots[(int)((EquipmentItem)_equipment.Items[i]).EquipSlot].SetItem(_equipment.Items[i]);
        }
    }
}
