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

    public void SetIcon(Sprite value) => icon.sprite = value;

    public void SetItemName(string value) => itemName.text = value;

    public void SetUsage(string value) => usage.text = value;

    public void SetDescription(string value) => description.text = value;

    public void SetOnClickAction(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
        button.onClick.AddListener(() =>
        {
            UIManager.ClearShopPanel();
            // GameplayManager.GetGoogleSender().SendMatrix4(itemName.text);
            GameplayManager.CloseShop();
        });
    }

    public static GameObject Instantiate()
    {
        return Instantiate(Resources.Load<GameObject>(prefabPath));
    }

    public static GameObject HPRecovery(int value)
    {
        GameObject shopOption = Instantiate();

        ShopOption script = shopOption.GetComponent<ShopOption>();
        script.SetIcon(Resources.Load<Sprite>("Sprites/HPRecovery"));
        script.SetItemName(value.ToString() + " HP Recovery");
        script.SetUsage("Instant");
        script.SetDescription("Recover " + value.ToString() + " HP.");
        script.SetOnClickAction(() =>
        {
            GameplayManager.getCharacter().GetComponent<Character>().Heal(value);
        });
        return shopOption;
    }

    public static GameObject SpeedBoost()
    {
        GameObject shopOption = Instantiate();

        ShopOption script = shopOption.GetComponent<ShopOption>();
        script.SetIcon(Resources.Load<Sprite>("Sprites/Items/Speed Boost"));
        script.SetItemName("Speed Boost");
        script.SetUsage("Instant");
        script.SetDescription("Boost up your speed.");
        script.SetOnClickAction(() =>
        {
            GameplayManager.getCharacter().GetComponent<Character>().SpeedBoost(1.5f);
        });
        return shopOption;
    }
}
