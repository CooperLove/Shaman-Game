using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AddPoints : MonoBehaviour
{
    [Header("Force")]
    [SerializeField] private List<TMP_Text> forceTexts = new List<TMP_Text>();
    [Header("Intelligence")]
    [SerializeField] private List<TMP_Text> intTexts = new List<TMP_Text>();
    [Header("Dexterity")]
    [SerializeField] private List<TMP_Text> dexTexts = new List<TMP_Text>();
    [Header("Constitution")]
    [SerializeField] private List<TMP_Text> conTexts = new List<TMP_Text>();
    [Header("Vigor")]
    [SerializeField] private List<TMP_Text> vigorTexts = new List<TMP_Text>();
    [Header("Other texts")]
    [SerializeField] private TMP_Text avPoints= null;
    [SerializeField] private List<TMP_Text> otherTexts = new List<TMP_Text>();
    Player player;

    private static AddPoints instance;
    public List<TMP_Text> OtherTexts { get => otherTexts; set => otherTexts = value; }
    public static AddPoints Instance { get => instance; set => instance = value; }

    private AddPoints (){
        if (Instance == null)
            Instance = this;
    }

    private void Awake() {
        player = Player.Instance;
        int size = transform.childCount;
        for (int i = 1; i < size; i++)
        {
            if (i != size-1)
                transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>();
            else
                transform.GetChild(i).GetComponent<TMP_Text>();
        }
        
        //Initialize();
    }


    public void Add (string stat){
        if (Player.Instance.playerInfo.StatsPoints == 0)
            return;
        Invoke(stat, 0);
        for (int i = 0; i < OtherTexts.Count; i++)
        {
            OtherTexts[i].GetComponent<CharacterMenu>().GetVariable(OtherTexts[i].name);
        }
    }

    public void Force(){
        if (Player.Instance.playerInfo.StatsPoints == 0)
            return;
        Debug.Log("Force: "+gameObject.name);
        player.playerInfo.StatsPoints        -=1; 
        player.playerInfo.Force              +=1;
        player.playerInfo.MinPhysicalDamage  +=4;
        player.playerInfo.MaxPhysicalDamage  +=8;
        player.playerInfo.Armor              +=2;
        UpdateForceTexts();
    }
    public void Dexterity(){
        if (Player.Instance.playerInfo.StatsPoints == 0)
            return;
        player.playerInfo.StatsPoints        -=1; 
        player.playerInfo.Dexterity          +=1;
        player.playerInfo.MinPhysicalDamage  +=3;
        player.playerInfo.MaxPhysicalDamage  +=3;
        player.playerInfo.CriticalChance     += player.playerInfo.Dexterity % 12 == 0 ? 1 : 0;
        UpdateDexTexts();
    }
    public void Intelligence(){
        if (Player.Instance.playerInfo.StatsPoints == 0)
            return;
        player.playerInfo.StatsPoints        -=1; 
        player.playerInfo.Intelligence       +=1;
        player.playerInfo.MinMagicDamage     +=5;
        player.playerInfo.MaxMagicDamage     +=8;
        player.playerInfo.MagicResistance    +=2;
        player.playerInfo.MPRegen         += 0.35f;
        
        player.playerInfo.FireResist      +=0.20f;
        player.playerInfo.WaterResist     +=0.20f;
        player.playerInfo.EarthResist     +=0.20f;
        player.playerInfo.WindResist      +=0.20f;
        player.playerInfo.DarkResist      +=0.20f;
        UpdateIntTexts();
    }
    public void Constitution(){
        if (Player.Instance.playerInfo.StatsPoints == 0)
            return;
        player.playerInfo.StatsPoints     -=1; 
        player.playerInfo.Constitution    +=1;
        player.playerInfo.Max_HP          +=15;
        player.playerInfo.Armor           +=4;
        player.playerInfo.MagicResistance +=2;
        player.playerInfo.HPRegen         += 0.35f;
        UpdateConTexts();
    }
    public void Vigor(){
        if (Player.Instance.playerInfo.StatsPoints == 0)
            return;
        player.playerInfo.StatsPoints     -=1; 
        player.playerInfo.Vigor           +=1;
        player.playerInfo.Stamina              +=5;
        player.playerInfo.SPRegen         += 0.35f;
        UpdateVigorTexts();
    }

    public void Initialize (){
        
        var offense = GameObject.Find("Grid Layout Offense").GetComponent<Transform>();
        var defense = GameObject.Find("Grid Layout Defense").GetComponent<Transform>();
        var elementalResist = GameObject.Find("Grid Layout Elemental resist").GetComponent<Transform>();
        var regen = GameObject.Find("Grid Layout Regen").GetComponent<Transform>();
        var other1 = GameObject.Find("Grid Layout Other 1").GetComponent<Transform>();
        var other2 = GameObject.Find("Grid Layout Other 2").GetComponent<Transform>();

    //Pega os textos relacionados ao atributo de Força
        forceTexts.Add(GameObject.Find("Force Value CMUI").GetComponent<TMP_Text>());
        forceTexts.Add(offense.Find("Offense - Physical damage").GetChild(0).GetComponent<TMP_Text>());
        forceTexts.Add(defense.Find("Defense - Armor").GetChild(0).GetComponent<TMP_Text>());
        
    //Pega os textos relacionados ao atributo de Inteligencia
        intTexts.Add(GameObject.Find("Intelligence Value CMUI").GetComponent<TMP_Text>());
        intTexts.Add(offense.Find("Offense - Magic damage").GetChild(0).GetComponent<TMP_Text>());
        intTexts.Add(defense.Find("Defense - Magic resist").GetChild(0).GetComponent<TMP_Text>());
        intTexts.Add(regen.Find("Regen - Mana").GetChild(0).GetComponent<TMP_Text>());
        for (int i = 0; i < elementalResist.childCount; i++)
            intTexts.Add(elementalResist.GetChild(i).GetChild(0).GetComponent<TMP_Text>());
        
    //Pega os textos relacionados ao atributo de Destreza
        dexTexts.Add(GameObject.Find("Dexterity Value CMUI").GetComponent<TMP_Text>());
        dexTexts.Add(offense.Find("Offense - Physical damage").GetChild(0).GetComponent<TMP_Text>());
        dexTexts.Add(offense.Find("Offense - Critical chance").GetChild(0).GetComponent<TMP_Text>());
        
    //Pega os textos relacionados ao atributo de Constituição
        conTexts.Add(GameObject.Find("Constitution Value CMUI").GetComponent<TMP_Text>());
        conTexts.Add(defense.Find("Defense - Armor").GetChild(0).GetComponent<TMP_Text>());
        conTexts.Add(defense.Find("Defense - Magic resist").GetChild(0).GetComponent<TMP_Text>());
        conTexts.Add(regen.Find("Regen - Health").GetChild(0).GetComponent<TMP_Text>());
        
    //Pega os textos relacionados ao atributo de Vigor
        vigorTexts.Add(GameObject.Find("Vigor Value CMUI").GetComponent<TMP_Text>());
        vigorTexts.Add(regen.Find("Regen - Stamina").GetChild(0).GetComponent<TMP_Text>());

    //Texto dos pontos disponiveis
        avPoints = GameObject.Find("Available Points CMUI").GetComponent<TMP_Text>();

    //Demais textos
        otherTexts.Add(defense.Find("Defense - Shield energy").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(defense.Find("Defense - Bleeding resist").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(defense.Find("Defense - Poison resist").GetChild(0).GetComponent<TMP_Text>());
        
        otherTexts.Add(other1.Find("Other - Lifesteal").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(other1.Find("Other - Mana cost").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(other1.Find("Other - Potion effectiveness").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(other1.Find("Other - Potion duration").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(other1.Find("Other - Buff duration").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(other1.Find("Other - Spell damage").GetChild(0).GetComponent<TMP_Text>());
        
        otherTexts.Add(other2.Find("Other - CC reduction").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(other2.Find("Other - Heal effectiveness").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(other2.Find("Other - Poison damage").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(other2.Find("Other - Bleeding damage").GetChild(0).GetComponent<TMP_Text>());
        
        otherTexts.Add(offense.Find("Offense - Armor penetration").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(offense.Find("Offense - Magic penetration").GetChild(0).GetComponent<TMP_Text>());
        otherTexts.Add(offense.Find("Offense - Critical damage").GetChild(0).GetComponent<TMP_Text>());
        
        UpdateAllTexts();
    }

    private void UpdateForceTexts () {
        PlayerInfo pi = Player.Instance.playerInfo;
        forceTexts[0].text = pi.Force+"";
        forceTexts[1].text = $"{pi.MinPhysicalDamage}-{pi.MaxPhysicalDamage}";
        forceTexts[2].text = pi.Armor+"";
        avPoints.text = $"Av Points: {pi.StatsPoints}";
    }
    private void UpdateIntTexts () {
        PlayerInfo pi = Player.Instance.playerInfo;
        intTexts[0].text = pi.Intelligence+"";
        intTexts[1].text = $"{pi.MinMagicDamage}-{pi.MaxMagicDamage}";
        intTexts[2].text = pi.MagicResistance+"";
        intTexts[3].text = pi.MPRegen+"";

        intTexts[4].text = pi.FireResist+"%";
        intTexts[5].text = pi.WaterResist+"%";
        intTexts[6].text = pi.EarthResist+"%";
        intTexts[7].text = pi.DarkResist+"%";
        intTexts[8].text = pi.WindResist+"%";
        avPoints.text = $"Av Points: {pi.StatsPoints}";
    }
    private void UpdateDexTexts () {
        PlayerInfo pi = Player.Instance.playerInfo;

        dexTexts[0].text = pi.Dexterity+"";
        dexTexts[1].text = $"{pi.MinPhysicalDamage}-{pi.MaxPhysicalDamage}";
        dexTexts[2].text = pi.CriticalChance+"%";
        avPoints.text = $"Av Points: {pi.StatsPoints}";
    }
    private void UpdateConTexts () {
        PlayerInfo pi = Player.Instance.playerInfo;

        conTexts[0].text = pi.Constitution+"";
        conTexts[1].text = pi.Armor+"";
        conTexts[2].text = pi.MagicResistance+"";
        conTexts[3].text = pi.HPRegen+"";
        avPoints.text = $"Av Points: {pi.StatsPoints}";
    }
    private void UpdateVigorTexts () {
        PlayerInfo pi = Player.Instance.playerInfo;

        vigorTexts[0].text = pi.Vigor+"";
        vigorTexts[1].text = pi.SPRegen+"";
        avPoints.text = $"Av Points: {pi.StatsPoints}";
    }

    public void UpdateAllTexts (){
        if (forceTexts == null || forceTexts.Count == 0)
            Initialize();

        UpdateForceTexts();
        UpdateIntTexts();
        UpdateDexTexts();
        UpdateConTexts();
        UpdateVigorTexts();

        PlayerInfo pi = Player.Instance.playerInfo;
        otherTexts[0].text = pi.Shield+"";
        otherTexts[1].text = pi.BleedingResistance+"%";
        otherTexts[2].text = pi.PoisonResistance+"%";
        otherTexts[3].text = pi.Lifesteal+"%";
        otherTexts[4].text = pi.ManaCostReduction+"%";
        otherTexts[5].text = pi.PotionEffectiveness+"%";
        otherTexts[6].text = pi.PotionDuration+"%";
        otherTexts[7].text = pi.BuffDuration+"%";
        otherTexts[8].text = (pi.SkillDamage*100)+"%";
        otherTexts[9].text = pi.CcDurationReduction+"%";
        otherTexts[10].text = pi.HealEffectiveness+"%";
        otherTexts[11].text = pi.PoisonDamage+"%";
        otherTexts[12].text = pi.BleedingDamage+"%";
        otherTexts[13].text = pi.ArmorPenetration+"%";
        otherTexts[14].text = pi.MagicPenetration+"%";
        otherTexts[15].text = pi.CriticalDamage+"%";
    }
}


