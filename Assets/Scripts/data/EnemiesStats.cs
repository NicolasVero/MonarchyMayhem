using System;

[Serializable]
public class EnemiesStats {
    public EnemyStats[] enemiesStat;
}

[Serializable]
public class EnemyStats {
    public string type; 
    public int health;
    public int attack; 
    public float attackSpeed;
    public float speed;
    public float range; 
    public int xp;


    // public int peasant_health;
    // public int peasant_attack;
    // public float peasant_attackSpeed;
    // public float peasant_range;
    // public float peasant_speed;
    // public int peasant_xp;

    // public int bourgeois_health;
    // public int bourgeois_attack;
    // public float bourgeois_attackSpeed;
    // public float bourgeois_range;
    // public float bourgeois_speed;
    // public int bourgeois_xp;
}