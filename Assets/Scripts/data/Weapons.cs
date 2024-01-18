using System;

[Serializable]
public class Weapons {
    public Weapon[] weaponStats;
}

[Serializable]
public class Weapon {
    public int id;
    public string name;
    public string tier;
    public int attack;
    public float range;
    public float attackSpeed;
    public float knockback;
}