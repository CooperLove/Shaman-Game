
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DescriptionPanel : MonoBehaviour
{
    public Item item;
    private ItemHandler handler;
    public Scrollbar descScrollbar = null;
    [SerializeField] private TMP_Text itemName = null;
    [SerializeField] private TMP_Text desc = null;
    [SerializeField] private TMP_Text effects = null;
    [SerializeField] private TMP_Text enchantments = null;
    [SerializeField] private TMP_Text gems = null;
    [SerializeField] private RectTransform content  = null;
    [SerializeField] private float descHeight = 0, effectHeight = 0, enhanceHeight = 0;
    // [SerializeField] private float minPanelHeight = 0;

    private RectTransform descriptionRect = null;
    private RectTransform gemsRect = null;
    private RectTransform effectsRect = null;
    private RectTransform enhancementsRect = null;

    private bool resize = false;
    
    private static DescriptionPanel instance;

    public static DescriptionPanel Instance { get => instance; set => instance = value; }
    public ItemHandler Handler { get => handler; set => handler = value; }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("Initialize desc panel");
        desc = Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("DP - Description Text")).ToList()[0];
        effects = Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("DP - Effects Text")).ToList()[0];
        enchantments = Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("DP - Enchantments Text")).ToList()[0];
        gems = Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("DP - Gems Text")).ToList()[0];
        
        descriptionRect = desc.gameObject.GetComponent<RectTransform>();
        effectsRect = effects.gameObject.GetComponent<RectTransform>();
        enhancementsRect = enchantments.gameObject.GetComponent<RectTransform>();
        gemsRect = gems.gameObject.GetComponent<RectTransform>();
    }

    public void Open () {
        if (descScrollbar != null)
            descScrollbar.value = 1;
            
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!resize)
            return;

        ResizeContentRect ();
    }

    public void Close () => gameObject.SetActive(false);
    
    private DescriptionPanel(){
        if (instance == null)
            instance = this;
    }

    public void SetItem (Item item) => this.item = item;
    public void SetItem (ItemHandler h) => this.Handler = h;
    public void ChangeText (){
        if (item == null)
            return;

        if (!desc)
            Initialize();
        
        itemName.text     = item.ItemName;
        desc.text         = item.GetDescriptionText();
        effects.text      = ((Collectable) item).GetEffectsText();
        enchantments.text = item.GetEnchantmentsText(Handler.Gems);
        gems.text = item.GetGemsText(Handler.Gems);
        resize = true;
    }

    private void ResizeContentRect()
    {
        descriptionRect.sizeDelta = new Vector2(descriptionRect.sizeDelta.x, desc.preferredHeight > descHeight ? desc.preferredHeight : descHeight);

        effectsRect.sizeDelta = new Vector2(effectsRect.sizeDelta.x, effects.preferredHeight > effectHeight ? effects.preferredHeight : effectHeight);

        enhancementsRect.sizeDelta = new Vector2(enhancementsRect.sizeDelta.x, enchantments.preferredHeight > enhanceHeight ? enchantments.preferredHeight : enhanceHeight);

        gemsRect.sizeDelta = new Vector2(gemsRect.sizeDelta.x, enchantments.preferredHeight > enhanceHeight ? gems.preferredHeight : enhanceHeight);
        
        var height = 80 + descriptionRect.sizeDelta.y + effectsRect.sizeDelta.y + enhancementsRect.sizeDelta.y + gemsRect.sizeDelta.y;
        
        Debug.Log($"Altura: {height} [{desc.preferredHeight}] [{effects.preferredHeight}] [{enchantments.preferredHeight}] [{gems.preferredHeight}]");
        
        content.sizeDelta = new Vector2 (content.sizeDelta.x, height);

        resize = false;
    }
}
