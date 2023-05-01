using UnityEngine;
using UnityEngine.UI;

public class MinionSlot : MonoBehaviour
{
    private MinionManager minionManager;
    private Interface interfaceManager;
    public Image image;
    public Text text;
    
    public int id;

    bool loaded;

    public void AddGraphics()
    {
        if (!loaded)
        {
            loaded = true;
            interfaceManager = GameObject.Find("INTERFACE").GetComponent<Interface>();
            minionManager = GameObject.Find("ClickerManager").GetComponent<MinionManager>();
            image.sprite = minionManager.minionsDataBase[id].Preview;
            text.text = $"{minionManager.minionsDataBase[id].name}.\n{minionManager.minionsDataBase[id].DamageCoef} from your damage.";
            transform.localScale = new(1, 1, 1);
        }
    }

    public void Equip()
    {
        minionManager.EquipMinion(minionManager.slotid, id);
        interfaceManager.CloseMinionSelect();
    }
}
