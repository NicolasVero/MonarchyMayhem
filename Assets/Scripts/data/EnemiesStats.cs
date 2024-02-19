using System;

[Serializable]
public class EnemiesStats {
    public DifficultyStats easy;
    public DifficultyStats medium;
    public DifficultyStats hard;
}

[Serializable]
public class DifficultyStats {
    public EnemyStats[] enemiesStat;
}

[Serializable]
public class EnemyStats {
    public string type; 
    public float chanceToDrop; 
    public int health;
    public int attack; 
    public float attackSpeed;
    public float speed;
    public float range; 
    public int xp;
}



// using System;

// [Serializable]
// public class EnemiesStats {
//     public EnemyStats[] enemiesStat;
// }

// [Serializable]
// public class EnemyStats {
//     public string type; 
//     public float chanceToDrop; 
//     public int health;
//     public int attack; 
//     public float attackSpeed;
//     public float speed;
//     public float range; 
//     public int xp;
// }