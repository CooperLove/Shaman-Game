using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class ConsumableBar : MonoBehaviour
{
    public RectTransform consumableMenu;
    public GridLayoutGroup gridLayout;
    public Transform bar;
    public Inventory inventory;
    [FormerlySerializedAs("currentSlot")] public int slot;

    private static ConsumableBar instance;

    public static ConsumableBar Instance { get => instance; set => instance = value; }

    private ConsumableBar(){
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        ResizeMenu();
    }

    public void Open () {
        inventory.Stack();
        Player.Instance.StartCoroutine(DisplayConsumables());
        consumableMenu.gameObject.SetActive(true);
    }
    public void Open (int consumableSlot) {
        if (consumableMenu.gameObject.activeInHierarchy){
            Close();
            return;
        }
        inventory.Stack();
        consumableMenu.gameObject.SetActive(false);
        this.slot = consumableSlot;
        Player.Instance.StartCoroutine(DisplayConsumables());
        consumableMenu.gameObject.SetActive(true);
    }
    public void Close () => consumableMenu.gameObject.SetActive(false);

    private void ResizeMenu (){
        if (gridLayout.transform.childCount <= 0)
            return;
        var rows = (gridLayout.transform.childCount-1) / 6 ;
        //Debug.Log($"Exibindo {gridLayout.transform.childCount} consumíveis => {rows}");
        var height = 70 + ((gridLayout.cellSize.y + gridLayout.spacing.y) * rows);
        consumableMenu.sizeDelta = new Vector2(consumableMenu.sizeDelta.x, height);
    }

    private IEnumerator DisplayConsumables (){
        yield return new WaitForEndOfFrame();

        var consumables = inventory.GetConsumables();
        var tam = gridLayout.transform.childCount;
        for (var i = tam - 1; i >= 0; i--)
            Destroy(gridLayout.transform.GetChild(i).gameObject);
        
        var barItems = new List<string>();
        tam = bar.childCount;
        for (var i = 0; i < tam; i++)
        {
            var c = bar.GetChild(i);
            if (c.childCount > 1)
                barItems.Add(c.GetChild(1).GetComponent<Consumable_Bar_Item_Handler>().handler.Item.ItemName);
        }

        foreach (var h in consumables.Where(h => !barItems.Contains(h.Item.ItemName)))
            CreateConsumableObject(h);
    }

    private GameObject CreateConsumableObject (ConsumableHandler h){
        //Setup do objeto que irá conter a imagem do consumível
        var item = new GameObject {name = h.Item.ItemName, tag = "ConsumableInMenu"};
        item.transform.SetParent(gridLayout.transform);
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = Vector3.zero;

        //Seta a imagem
        var i = item.AddComponent<Image>();
        i.sprite = h.Item.Sprite;

        var bgText = new GameObject {name = "Text BG"};
        bgText.transform.SetParent(item.transform);
        var rect = bgText.AddComponent<RectTransform>();
        bgText.AddComponent<Image>().color = new Color(0.168f, 0.165f, 0.185f, 1);
        rect.localScale = Vector3.one;
        rect.localPosition = Vector3.zero;
        rect.anchorMin = new Vector2 (1,1);
        rect.anchorMax = new Vector2 (1,1);
        rect.pivot = new Vector2(1,1);
        rect.sizeDelta = new Vector2(10,12.5f);
        rect.anchoredPosition = new Vector2(2f, 2f);
        //rect.localPosition = new Vector3(47.5f, 47.5f, 0);

        //Setup do texto com a quantidade do conusmível
        var text = new GameObject {name = "Quantity Text"};
        text.transform.SetParent(bgText.transform);
        text.transform.localScale = Vector3.one;
        rect = text.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2 (0,0);
        rect.anchorMax = new Vector2 (1,1);
        rect.sizeDelta = new Vector2 (0,0);
        rect.anchoredPosition = new Vector2(0f, 0f);
        //rect.localPosition = Vector3.zero;

        //Configurações do texto da quantidade
        var t = text.AddComponent<TextMeshProUGUI>();
        t.text = h.Quantity.ToString();
        t.fontSize = 16;
        t.fontStyle = FontStyles.Bold;
        t.alignment = TextAlignmentOptions.Midline;

        var handler = item.AddComponent<Consumable_Bar_Item_Handler>();
        handler.handler = h;
        handler.qtdText = t;
        handler.consumableBar = this;
        handler.bar = bar;
        handler.menu = gridLayout.transform;

        return item;
    }


    public IEnumerator OnLoad (){
        yield return new WaitForEndOfFrame();
        var length = gridLayout.transform.childCount;
        Debug.Log($"{gridLayout} {length}");
        for (var i = 0; i < length; i++)
        {
            gridLayout.transform.GetChild(i).TryGetComponent<Consumable_Bar_Item_Handler>(out var handler);
            Debug.Log($"Handler {handler}");
            if (handler){
                handler.OnLoad();
            }
        }
    }   
}
