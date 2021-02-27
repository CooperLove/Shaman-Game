using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCompanion : Companion
{
    
    [SerializeField] private float xDistance = 0;
    [SerializeField] private float velocity = 0;
    private float dist;

    // Start is called before the first frame update
    void Start()
    {
        Player = Player.Instance;
        SpellEffectMultiplier = 1;
        AddUniqueSpell();
        //DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    private void Update() {
        if (gameObject.layer == 0)
            FollowPlayer();
    }

    public override void FollowPlayer()
    {
        dist = Vector2.Distance(transform.position, Player.transform.position);
        //Debug.Log(dist);
        if (transform.position.x < Player.transform.position.x - xDistance){
            Follow (transform ,-xDistance);
        }else if (transform.position.x > Player.transform.position.x + xDistance){
            Follow (transform, xDistance);
        }
    }
    private void Follow (Transform transform, float xDist){
        Vector2 pos = Vector2.MoveTowards(transform.position, Player.transform.position, velocity * Time.deltaTime);
        pos.x = xDist < 0 ? 
                Mathf.Clamp(pos.x, Mathf.NegativeInfinity, Player.transform.position.x + xDist) :
                Mathf.Clamp(pos.x, Player.transform.position.x - xDist, Mathf.Infinity);
        transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
        //Debug.Log(pos+" "+transform.position+" "+transform.localPosition);
    }

    public override void OnGainEXP (int xp)
    {
        this.Xp += xp;
        if (Xp >= XpToLevelUp){
            OnLevelUp();
        }
    }
    public override void OnLevelUp()
    {
        Level += 1;
        MaxNumberOfSpells = Level % 10 == 0 ? MaxNumberOfSpells + 1 : MaxNumberOfSpells; 
        Xp -= XpToLevelUp;
        XpToLevelUp += 50;

        MaxMana += 5;
        Mana = MaxMana ;
        ManaRegen += 0.05f;

        AdditionalPoints += 5;
        SpecializationPoints += 3;
        CompanionUI.Instance?.UpdateUI(this);
        MP_Bar.Instance.CompUpdateManabar();
        if (Xp >= XpToLevelUp)
            OnLevelUp();
    }

    public override void AddUniqueSpell()
    {
        Spells.Add (UniqueSpell);
        //LearnedSpells.Instance.OnLearnSpell(UniqueSpell);
    }

    public override void ChangeCompanionStats(Companion c)
    {
        if (this.CompName.Equals(c.CompName))
            return;
            
        this.CompName = c.CompName;
        this.Type = c.Type;

        this.MaxMana = c.MaxMana;
        this.Mana = c.Mana;
        this.ManaRegen = c.ManaRegen;
        this.SpellEffectMultiplier = c.SpellEffectMultiplier;
        this.Level = c.Level;
        this.Xp = c.Xp;
        this.XpToLevelUp = c.XpToLevelUp;

        this.Intelligence = c.Intelligence;
        this.Intellect = c.Intellect;
        this.Wisdom = c.Wisdom;
        this.AdditionalPoints = c.AdditionalPoints;
        
        this.AdditionalForce = c.AdditionalForce;
        this.AdditionalInt = c.AdditionalInt;
        this.AdditionalDex = c.AdditionalDex;
        this.AdditionalCon = c.AdditionalCon;
        this.AdditionalVigor = c.AdditionalVigor;

        this.CrystalT1 = c.CrystalT1;
        this.CrystalT2 = c.CrystalT2;
        this.CrystalT3 = c.CrystalT3;

        this.Healing = c.Healing;
        this.Shielding = c.Shielding;
        this.Buffing = c.Buffing;
        this.SpecializationPoints = c.SpecializationPoints;

        this.SpellPoints = c.SpellPoints;
        this.UniqueSpell = c.UniqueSpell;
        this.Spells = c.Spells;

        this.companionHandler = c.companionHandler;

        this.CurrentEvoIndex = c.CurrentEvoIndex;
        this.FollowedPath = c.FollowedPath;
        this.ChosenMissions = c.ChosenMissions;
        this.InProgressMissions = c.InProgressMissions;

        this.MaxNumberOfSpells = c.MaxNumberOfSpells;

        ApplyBonusStats();
    }

    public override void ReactivateButtons (){

    }

    public override void RefillPaths (){
        if (ChosenMissions.Count > 0){
            Transform comp = EvolutionTree.Instance?.transform.GetChild(0);
            int t = comp.childCount;
            comp.GetChild(t -1).GetComponent<EvoTreeComponent>()?.FillPath(ChosenMissions[0]);
        }
        for (int i = 0; i < ChosenMissions.Count; i++)
        {
            if (i+1 >= ChosenMissions.Count)
                break;
            Transform comp = EvolutionTree.Instance?.transform.GetChild(FollowedPath[i]);
            int t = comp.childCount;
            comp.GetChild(t -1).GetComponent<EvoTreeComponent>()?.FillPath(ChosenMissions[i+1]);
            //Debug.Log(i+" "+FollowedPath[i]+" => "+ChosenMissions[i+1]);
        }
    }

    public void UnfillPaths (){
        if (ChosenMissions.Count > 0){
            Transform comp = EvolutionTree.Instance?.transform.GetChild(0);
            int t = comp.childCount;
            comp.GetChild(t -1).GetComponent<EvoTreeComponent>()?.UnfillPath(ChosenMissions[0]);
        }
        for (int i = 0; i < ChosenMissions.Count; i++)
        {
            if (i+1 >= ChosenMissions.Count)
                break;
            Transform comp = EvolutionTree.Instance?.transform.GetChild(FollowedPath[i]);
            int t = comp.childCount;
            comp.GetChild(t -1).GetComponent<EvoTreeComponent>()?.UnfillPath(ChosenMissions[i+1]);
            //Debug.Log(i+" "+FollowedPath[i]+" => "+ChosenMissions[i+1]);
        }
    }

    public override void UpdateSkillTreeUI()
    {
        foreach (GameObject s in Spells)
        {
            Spell spell = s.GetComponent<Spell>();
            SpellHandler.Instance.ChangeCurrentLevel (spell.Index, spell.CurrentLevel);
        }
    }

    public override void ApplyBonusStats()
    {
        Player p = Player.Instance;
        p.playerInfo.Bonus_force        = this.AdditionalForce;
        p.playerInfo.Bonus_intelligence = this.AdditionalInt;
        p.playerInfo.Bonus_dexterity    = this.AdditionalDex;
        p.playerInfo.Bonus_constitution = this.AdditionalCon;
        p.playerInfo.Bonus_vigor        = this.AdditionalVigor;
    }

    public override void UnapplyBonusStats()
    {
        throw new System.NotImplementedException();
    }
}
