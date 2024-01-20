using System;

[Serializable]
public class WeaponsStats {
    public WeaponStat[] weaponStats;
}

[Serializable]
public class WeaponStat {
    public int id;
    public string name;
    public string tier;
    public int attack;
    public float range;
    public float attackSpeed;
    public float knockback;
}