using System;

[Serializable]
public class WeaponsStats {
    public WeaponStat[] weapons;
}

[Serializable]
public class WeaponStat {
    public int id;
    public WeaponName name;
    public string tier;
    public int attack;
    public float attackSpeed;
    public float knockback;
    public float range;
    public int regeneration;
    public float speed;
    public float dropProbability;
}

[Serializable]
public class WeaponName {
    public string fr;
    public string en;
}