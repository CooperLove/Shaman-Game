using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Enhancement : MonoBehaviour
{
    private static Enhancement instance = null;
    [SerializeField] private Image itemImage = null, gemImage = null;
    private Image itemImageEmpty = null, gemImageEmpty = null;
    [SerializeField] private TMP_Text percentageText = null;
    [SerializeField] private EquipmentHandler equipmentHandler = null;
    [SerializeField] private GemHandler gemHandler = null;
    [SerializeField] private Button enhanceButton = null;
    public static Enhancement Instance { get => instance; set => instance = value; }
    public EquipmentHandler Handler { get => equipmentHandler; set => equipmentHandler = value; }
    public GemHandler GemHandler { get => gemHandler; set => gemHandler = value; }
    public Image ItemImage { get => ItemImage1; set => ItemImage1 = value; }
    public Image ItemImage1 { get => itemImage; set => itemImage = value; }

    public GameObject enhanceResult = null;

    public void Open () => gameObject.SetActive(true);
    public void Close () => gameObject.SetActive(false);
    private void Start() {
        itemImageEmpty = itemImage;
        gemImageEmpty = gemImage;
        itemImageEmpty.sprite = itemImage.sprite;
        gemImageEmpty.sprite = gemImage.sprite;
    }
    Enhancement(){
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TryToEnhance (){
        if (gemHandler == null || equipmentHandler == null || !(gemHandler.Item is Gem))
            return;
        
        bool res = ((Gem) gemHandler.Item).EnhanceItem(gemHandler, equipmentHandler);

        if (!res){
            if (gemHandler.Item is EnhancementGem){
                equipmentHandler.Enhancement -= 1;
                ((ItemHandler)equipmentHandler).ChangeText();
            }
        }

        UI_NotificationManager.Instance.ItemEnhancementNotification(res ? "Sucess" : "Fail", 
                                                                    res ? new Color (0.11f, 0.83f, 0.63f, 1) : 
                                                                    new Color (1f, 0.255f, 0.255f, 1));

        gemImage.sprite = gemImageEmpty.sprite;
        ((Gem)gemHandler.Item).DestroyGem(gemHandler);
        gemImage.color = ColorTable.SecondaryColor;
        percentageText.text = "0%";
        
    }

    public void ChangeImages (ItemHandler handler){
        if (handler == null || handler is ConsumableHandler || handler is CompanionHandler)
            return;
        if (handler is EquipmentHandler){
            Handler = (EquipmentHandler) handler;
            ItemImage.sprite = handler.Item.Sprite;
            itemImage.color = Color.white;
        }
        if (handler is GemHandler){
            GemHandler = (GemHandler) handler;
            gemImage.sprite = handler.Item.Sprite;
            gemImage.color = Color.white;
        }
        if (GemHandler != null && Handler != null){
            float p = Equipment.percEnhancement[Handler.Enhancement];
            float p2 = Gem.percentagePerRarity[(int)(this.GemHandler.Rarity)];
            float perc = p * p2;
            perc = Mathf.Clamp(perc, 0, 100);
            Debug.Log($"Item {Handler.Rarity} {p} Gema {GemHandler.Rarity} {p2} Perc {perc}");
            
            percentageText.text = gemHandler.Item is EnchantmentGem ? 
                                (equipmentHandler?.Gems.Count >= Equipment.MAX_NUMBER_OF_EQUIPMENT_ENCH) ? "0%" : "100%" : perc+"%";

           if (gemHandler.Item is EnchantmentGem)
                enhanceButton.interactable = equipmentHandler.Gems.Count >= Equipment.MAX_NUMBER_OF_EQUIPMENT_ENCH ? false : true;
            if (gemHandler.Item is EnhancementGem)
                enhanceButton.interactable = ((EquipmentHandler)equipmentHandler).Enhancement >= Equipment.MAX_ENHANCEMENT ? false : true;
        }
    }

    

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Camera.main.ViewportToWorldPoint(new Vector2(-1, 1)), Camera.main.ViewportToWorldPoint(new Vector2(1, 1)));
        Gizmos.DrawLine(Camera.main.ViewportToWorldPoint(new Vector2(-1, -1)), Camera.main.ViewportToWorldPoint(new Vector2(1, -1)));
        Gizmos.DrawLine(Camera.main.ViewportToWorldPoint(new Vector2(-1, -1)), Camera.main.ViewportToWorldPoint(new Vector2(-1, 1)));
        Gizmos.DrawLine(Camera.main.ViewportToWorldPoint(new Vector2(1, -1)), Camera.main.ViewportToWorldPoint(new Vector2(1, 1)));
        Gizmos.DrawLine(Camera.main.ViewportToWorldPoint(new Vector2(-1, 0)), Camera.main.ViewportToWorldPoint(new Vector2(1, 0)));
        Gizmos.DrawLine(Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0)), Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1)));
    }

}
