
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentDescriptionPanel : MonoBehaviour
{
    public Scrollbar descScrollbar = null;
    [SerializeField] private TMP_Text weaponNameText = null;
    [SerializeField] private TMP_Text attributesText = null;
    [SerializeField] private TMP_Text requirementsText = null;
    [SerializeField] private TMP_Text enchantmensText = null;
    [SerializeField] private TMP_Text gemsText = null;
    [SerializeField] private TMP_Text phraseText = null;
    [SerializeField] private RectTransform contentRect = null;

    [SerializeField] private Equipment equipment;
    [SerializeField] private EquipmentHandler handler;
    public float minTextHeight = 0;
    public float minPanelHeight = 0;
    public float nonTextComponentsHeight = 600f;

    private RectTransform attributesRect = null;
    private RectTransform requirementsRect = null;
    private RectTransform enchantmentsRect = null;
    private RectTransform gemsRect = null;
    private RectTransform phraseRect = null;

    private bool resize = false;
    
    private static EquipmentDescriptionPanel instance;

    public static EquipmentDescriptionPanel Instance { get => instance; set => instance = value; }
    public EquipmentHandler Handler { get => handler; set => handler = value; }

    EquipmentDescriptionPanel (){
        if (instance == null)
            instance = this;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        attributesText =
            Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("EDP - Attributes Text")).ToList()[0];
        requirementsText =
            Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("EDP - Requirements Text")).ToList()[0];
        enchantmensText =
            Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("EDP - Enchantments Text")).ToList()[0];
        gemsText =
            Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("EDP - Gems Text")).ToList()[0];
        phraseText =
            Resources.FindObjectsOfTypeAll<TMP_Text>().Where(x => x.gameObject.name.Equals("EDP - Phrase Text")).ToList()[0];

        attributesRect = attributesText.GetComponent<RectTransform>();
        requirementsRect = requirementsText.GetComponent<RectTransform>();
        enchantmentsRect = enchantmensText.GetComponent<RectTransform>();
        gemsRect = gemsText.GetComponent<RectTransform>();
        phraseRect = phraseText.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!resize)
            return;
        
        ResizeTextsRect();
    }

    public void Open () {
        if (descScrollbar != null)
            descScrollbar.value = 1;
            
        gameObject.SetActive(true);
    }
    
    public void Close () => gameObject.SetActive(false);
    
    public void SetEquipment (Equipment e) => equipment = e;
    
    public void SetEquipment (EquipmentHandler h) => Handler = h;
    
    public void ChangeText (){
        if (equipment == null)
            return;
        
        if (!attributesText)
            Initialize();

        resize = true;
        
        weaponNameText.color  = ColorTable.ItemNameColor((int)equipment.ItemRarity);
        weaponNameText.text   = equipment.ItemName;
        attributesText.text   = equipment.GetAttributesText(handler);
        requirementsText.text = equipment.GetRequirementsText();
        enchantmensText.text  = equipment.GetEnchantmentsText(Handler?.Gems);
        gemsText.text         = equipment.GetGemsText(Handler?.Gems);
        phraseText.text       = equipment.Phrase;

        resize = false;
    }

    private void ResizeTextsRect (){
        var contentSize = 0f;
        attributesRect.sizeDelta  = new Vector2(attributesRect.sizeDelta.x, attributesText.preferredHeight);
        contentSize += attributesText.preferredHeight;

        requirementsRect.sizeDelta  = new Vector2(requirementsRect.sizeDelta.x, requirementsText.preferredHeight );
        contentSize += requirementsText.preferredHeight;
        
        enchantmentsRect.sizeDelta  = new Vector2(enchantmentsRect.sizeDelta.x, enchantmensText.preferredHeight);
        contentSize += enchantmensText.preferredHeight;

        gemsRect.sizeDelta  = new Vector2(gemsRect.sizeDelta.x, gemsText.preferredHeight);
        contentSize += gemsText.preferredHeight;

        phraseRect.sizeDelta = new Vector2(phraseRect.sizeDelta.x, phraseText.preferredHeight);
        contentSize += phraseText.preferredHeight;

        contentRect.sizeDelta = new Vector2(0, 80 + contentSize);
    }

    public void EnhanceItem (){
        
    }
}
