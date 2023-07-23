using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopOption : MonoBehaviour
{
    private static string prefabPath = "Prefabs/ShopOption";
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI usage;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Button button;

    public void SetIcon(Sprite value)
    {
        icon.sprite = value;
    }

    public void SetItemName(string value)
    {
        itemName.text = value;
    }

    public void SetUsage(string value)
    {
        usage.text = value;
    }

    public void SetDescription(string value)
    {
        description.text = value;
    }

    public void SetOnClickAction(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public static GameObject Instantiate() {
        return Instantiate(Resources.Load<GameObject>(prefabPath));
    }
}
