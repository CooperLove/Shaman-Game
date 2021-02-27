using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public const float CHANCE_UNCOMMON = 5.5f;
    public const float CHANCE_COMMON = 55.0f;
    public const float CHANCE_GREAT = 30.5f;
    public const float CHANCE_RARE = 5.0f;
    public const float CHANCE_EPIC = 2.5f;
    public const float CHANCE_LEGENDARY = 1.0f;
    public const float CHANCE_DIVINE = 0.5f;
    public enum Rarity{ Uncommon, Common, Great, Rare, Epic, Legendary, Divine }
    public enum ItemType { Consumable, Weapon, Boot, Helm, ChestArmor, Legging, Glove, Jewerly, Gem }
    public enum WeaponType { Staff, Claw, Mace, Bow }
    public enum GearType { Boots, Helmet, ChestArmor, Leggings, Gloves }
    public enum JewerlyType { Amullet, LeftRing, RightRing, Ornament }
    public enum PotionType  { Health, Mana, Stamina }
    public enum GemType { Enhancement, Enchantment, HP, Mana, Stamina }
    [Header("Art")]
    [SerializeField] private Sprite _sprite;

    [Header("Name")]
    [SerializeField] private string _name;

    [Header("How rare this item is")]
    [SerializeField] private Rarity _rarity;

    [Header("Adittional stats of an item")]
    [SerializeField] private List<KeyValuePair<string, float>> _enchantments;
    [SerializeField] private ItemType itemType;

    

    public string ItemName { get => _name; set => _name = value; }
    public Rarity ItemRarity { get => _rarity; set => _rarity = value; }
    public ItemType Type { get => itemType; set => itemType = value; }
    public Sprite Sprite { get => _sprite; set => _sprite = value; }

    public abstract string GetDescriptionText ();
    public virtual string GetEnchantmentsText (List<GemRarity> gems){
        if (gems.Count == 0)
        {
            return "Enchantments: \n"+
                   "\t -----"+
                   "\t -----"+
                   "\t -----";
        }

        return gems.Aggregate("<size=28><color=#E69913>Enchantments:</color></size> \n", (unknown, g) => 
            unknown + ($"\t <color=#E69913>+ {g.Enchantment.Value}</color> " + 
                       (g.Enchantment.IsFlat ? "" : "%") + 
                       $"{g.Enchantment.Name} \n"));
    }

    public virtual string GetGemsText(List<GemRarity> gems)
    {
        return gems == null ? "" : 
            gems.Aggregate("Gems: \n", (unknown, gem) => unknown +
                           $"\t <color=#{ColorUtility.ToHtmlStringRGB(ColorTable.ItemNameColor((int)gem.rarity))}>{gem.gem._name}</color>\n");
    }

    public virtual Type MyHandler (){
        return null;
    }

    public virtual Type MyType (){
        return GetType();
    }
    public abstract void NewItemIndicator (bool active);

    public Rarity GetRarity(){
         var chance = UnityEngine.Random.Range(0, 100f);
        
        var itemChance = CHANCE_UNCOMMON;
        if (chance <= itemChance)
            return Rarity.Uncommon;
        itemChance += CHANCE_COMMON;
        if (chance <= itemChance)
            return Rarity.Common;
        itemChance += CHANCE_GREAT;
        if (chance <= itemChance)
            return Rarity.Great;
        itemChance += CHANCE_RARE;
        if (chance <= itemChance)
            return Rarity.Rare;
        itemChance += CHANCE_EPIC;
        if (chance <= itemChance)
            return Rarity.Epic;
        itemChance += CHANCE_LEGENDARY;
        if (chance <= itemChance)
            return Rarity.Legendary;
        itemChance += CHANCE_DIVINE;
        if (chance <= itemChance)
            return Rarity.Divine;
        return Rarity.Uncommon;
    }
}
