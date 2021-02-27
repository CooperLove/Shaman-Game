[System.Serializable]
public class GemRarity {
    public Gem gem;
    public Item.Rarity rarity;
    private Enchantment enchantment;
    public Enchantment Enchantment { get => GetEnchantment() ; private set => enchantment = value; }

    public GemRarity(Gem gem, Item.Rarity rarity)
    {
        this.gem = gem;
        this.rarity = rarity;
        Enchantment = this.gem.GetEnchantment();
    }

    private Enchantment GetEnchantment() => enchantment ?? (enchantment = gem.GetEnchantment());
}