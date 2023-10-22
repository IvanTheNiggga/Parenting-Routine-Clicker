using UnityEngine;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour
{
    private UnitManager unitManager;
    private InterfaceManager interfaceManager;
    public Image image;
    public Text text;
    
    public int id;

    bool loaded;

    public void AddGraphics()
    {
        if (!loaded)
        {
            loaded = true;
            interfaceManager = GameObject.Find("INTERFACE").GetComponent<InterfaceManager>();
            unitManager = GameObject.Find("ClickerManager").GetComponent<UnitManager>();
            image.sprite = unitManager.unitsDataBase[id].Preview;
            text.text = $"{unitManager.unitsDataBase[id].name}.\nx{unitManager.unitsDataBase[id].DamageCoef} from your damage.";
            transform.localScale = new(1, 1, 1);
        }
    }

    public void Equip()
    {
        unitManager.EquipUnit(unitManager.slotid, id);
        interfaceManager.CloseUnitsSelect();
    }
}
