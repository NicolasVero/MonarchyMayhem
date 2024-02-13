using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour {
    
    [Header("HUD")] 
    [SerializeField] private Slider xpBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject levelUpPanel;

    [Header("Scripts")]
    [SerializeField] private LevelUpChoice levelUpChoice;
    [SerializeField] private HUDStats hudStats;
    [SerializeField] private new CameraController camera;
    [SerializeField] private new AudioController audio;

    [Header("Canvas")]
    [SerializeField] private Canvas deathScreen;
    [SerializeField] private Canvas hudScreen;
    [SerializeField] private Canvas questScreen;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject questMenu;
    [SerializeField] private ProgressiveDarkeningController progressiveDarkening;

    [Header("Weapons")]
    [SerializeField] GameObject weapon;
    [SerializeField] private WeaponsDropper weaponsDropper;
    [SerializeField] private GameObject weaponHolder;


    private const float sensitivity = 10;
    private int enemyKillCounter, sprint = 0;
    private bool goingAttack = false, canAttack = false, canResume = true, isAlive = true, inPause = false, isSprinting = false;

    private float timeSinceLastAttack = 0f;
    private float timeSinceLastRegeneration = 0f;
    private float regenerationDelay = 10.0f;

    // attributs
    private int totalXP;
    private int xp;
    private int xpToNext;
    private int level;
    private int health;
    private int maxActualHealth;
    private int resistance;
    private int attack;
    private float attackSpeed;
    private float range;
    private float speed;
    private float knockback;
    private int regeneration;

    // controlleur attributs
    private int xpRequired = 0;

    private int maxHealth;
    private int maxLevel;
    private int maxResistance;
    private int maxAttack;
    private float maxRange;
    private float minAttackSpeed;
    private float maxSpeed;
    private float maxKnockback;
    private int maxRegeneration;

    private int[] increaseHealth;
    private int[] increaseResistance;
    private int[] increaseAttack;
    private float[] increaseAttackSpeed;
    private float[] increaseRange;
    private float[] increaseSpeed;
    private float[] increaseKnockback;
    private int[] increaseRegeneration;


    private int healthLevel = 1;
    private int resistanceLevel = 1;
    private int attackLevel = 1;
    private int attackSpeedLevel = 1;
    private int rangeLevel = 1;
    private int speedLevel = 1;
    private int regenerationLevel = 1;

    private int weaponID;
    private string weaponName = "";
    private int   weaponAttack;
    private float weaponAttackSpeed;
    private float weaponKnockback;
    private float weaponRange;
    private int weaponRegeneration;
    private float weaponSpeed;

    private SphereCollider rangeCollider;
    private Animator animator;
    private Vector3 moveDirection;
    private Dictionary<KeyCode, Action> keyActions = new Dictionary<KeyCode, Action>();



    void Awake() {

        this.keyActions.Add(KeyCode.E, TakeWeapon);
        this.keyActions.Add(KeyCode.R, ToggleQuestMenu);
        this.keyActions.Add(KeyCode.P, TooglePauseMenu);


        DontDestroyOnLoad(this.gameObject);
        this.camera.DisableBlackAndWhiteEffect();

        GameController.SetCanvasVisibility(deathScreen, false);
        GameController.HidePauseMenu(pauseMenu);
        GameController.SetPanelVisibility(this.levelUpPanel, false);

        this.SetHealthBarMax(this.maxHealth);
        this.rangeCollider = GetComponent<SphereCollider>();
        this.animator = GetComponentInChildren<Animator>();
        this.LoadAttributes();

        this.xpBar.maxValue = 1;
        this.xpBar.value = 0;

        this.healthBar.maxValue = this.GetHealth();
        this.healthBar.value = this.GetHealth();

        this.DisableWeapons();
    }

    void Update() {

        this.animator.SetFloat("AttackSpeed", (1/(this.GetAttackSpeed() + this.GetWeaponAttackSpeed())*2));

        foreach (var kvp in keyActions) {
            if (Input.GetKeyDown(kvp.Key)) {
                kvp.Value.Invoke();
            }
        }

        if(Input.GetKey(KeyCode.LeftShift)) {
            this.isSprinting = true;
            this.sprint = 4;
        } else {
            this.isSprinting = false;
            this.sprint = 0;
        }


        // if(Input.GetKeyDown(KeyCode.R) && this.canResume) {
        //     questMenu.SetActive(!questMenu.activeSelf);
        // } 

        // if(Input.GetKeyDown(KeyCode.P) && this.canResume && this.isAlive) {
        //     GameController.SetGameState(false);
        //     this.SetInPause(true);
        //     this.ManagePauseMenu();
        // } 

        //! a supprimer
        if(Input.GetKeyDown(KeyCode.U)) 
            this.XPGain(1);

        if(Input.GetMouseButtonDown(0) && this.canResume && !GameController.GameIsFreeze() && this.canAttack) {
            this.goingAttack = true;
            this.hudStats.ChangeEnableAttackIcon(false);
            this.animator.SetTrigger("Attack");
            StartCoroutine(DisableGoingAttack());
        }
    }

    private void TooglePauseMenu() {
        if(this.canResume && this.isAlive) {
            GameController.SetGameState(false);
            this.SetInPause(true);
            this.ManagePauseMenu();
        }
    }

    private void ToggleQuestMenu() {
        if(this.canResume) 
            this.questMenu.SetActive(!this.questMenu.activeSelf);
    }

    private IEnumerator DisableGoingAttack() {
        yield return new WaitForSeconds(0.2f);
        this.goingAttack = false;
    }

    public void ManagePauseMenu() {
        
        if(this.inPause) {
            GameController.ShowPauseMenu(pauseMenu);
            this.audio.PlayPauseMenuSFX();
            this.audio.StopThemeSFX();
        } else {
            GameController.HidePauseMenu(pauseMenu);
            this.audio.StopPauseMenuSFX();
            this.audio.PlayThemeSFX();
        }
    }

    void FixedUpdate() {

        this.Move();

        if(this.isAlive) {
            float y = Input.GetAxis("Mouse X") * PlayerController.sensitivity;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);
        }

        this.TimerAttack();
        this.TimerRegeneration();
    }

    private void LoadAttributes() {
        TextAsset baseStats     = Resources.Load<TextAsset>("Data/PlayerBaseStats");
        TextAsset maxStats      = Resources.Load<TextAsset>("Data/PlayerMaxStats");
        TextAsset increaseStats = Resources.Load<TextAsset>("Data/PlayerIncreaseStats");

        if(baseStats != null && maxStats != null && increaseStats != null) {
            PlayerBaseStats playerBaseStats = JsonUtility.FromJson<PlayerBaseStats>(baseStats.text);
            PlayerMaxStats playerMaxStats = JsonUtility.FromJson<PlayerMaxStats>(maxStats.text);
            PlayerIncreaseStats playerIncreaseStats = JsonUtility.FromJson<PlayerIncreaseStats>(increaseStats.text);


            this.totalXP              = playerBaseStats.totalXP;
            this.xp                   = playerBaseStats.xp;
            this.xpToNext             = playerBaseStats.xpToNext;
            this.level                = playerBaseStats.level;
            this.health               = playerBaseStats.health;
            this.maxActualHealth      = playerBaseStats.health;
            this.resistance           = playerBaseStats.resistance;
            this.attack               = playerBaseStats.attack;
            this.attackSpeed          = playerBaseStats.attackSpeed;
            this.range                = playerBaseStats.range;
            this.rangeCollider.radius = playerBaseStats.range;
            this.speed                = playerBaseStats.speed;
            this.knockback            = playerBaseStats.knockback;
            this.regeneration         = playerBaseStats.regeneration;
     
            this.maxHealth            = playerMaxStats.maxHealth;
            this.maxLevel             = playerMaxStats.maxLevel;
            this.maxResistance        = playerMaxStats.maxResistance;
            this.maxAttack            = playerMaxStats.maxAttack;
            this.maxRange             = playerMaxStats.maxRange;
            this.minAttackSpeed       = playerMaxStats.minAttackSpeed;
            this.maxSpeed             = playerMaxStats.maxSpeed;
            this.maxKnockback         = playerMaxStats.maxKnockback;
            this.maxRegeneration      = playerMaxStats.maxRegeneration;

            this.increaseHealth       = playerIncreaseStats.increaseHealth;
            this.increaseResistance   = playerIncreaseStats.increaseResistance;
            this.increaseAttack       = playerIncreaseStats.increaseAttack;
            this.increaseAttackSpeed  = playerIncreaseStats.increaseAttackSpeed;
            this.increaseRange        = playerIncreaseStats.increaseRange;
            this.increaseSpeed        = playerIncreaseStats.increaseSpeed;
            this.increaseKnockback    = playerIncreaseStats.increaseKnockback;
            this.increaseRegeneration = playerIncreaseStats.increaseRegeneration;
        }
    }

    public void TimerAttack() {
        if(this.timeSinceLastAttack >= this.attackSpeed + this.weaponAttackSpeed) {  
 
            this.canAttack = true;
            this.hudStats.ChangeEnableAttackIcon(true);
            this.timeSinceLastAttack = 0f;
        }

        this.timeSinceLastAttack += Time.fixedDeltaTime;
    }

    public void TimerRegeneration() {
        if(this.timeSinceLastRegeneration >= this.regenerationDelay) {  
            this.timeSinceLastRegeneration = 0f;
            this.Heal(this.regeneration + this.weaponRegeneration);
        }

        this.timeSinceLastRegeneration += Time.fixedDeltaTime;
    }

    public void TakeDamage(int damage) {

        this.health -= Mathf.RoundToInt(damage * 1.0f - (float) this.resistance / 100.0f);
        this.hudStats.UpdateHealth();
        
        if(this.health <= 0) {
            this.Death();
        } else {
            this.SetHealthBar(this.health);
        }
    }

    private void Death() {
        this.isAlive = false;
        Invoke("CameraDeathAnimation", 0.5f);
        this.audio.PlayDeathSFX();
        this.audio.StopThemeSFX();
        this.DanceTriggered();
    }

    private void CameraDeathAnimation() {
        GameController.SetCanvasVisibility(hudScreen, false);
        GameController.SetCanvasVisibility(questScreen, false);
        
        this.animator.SetInteger("Death", GameController.Random(1, 3));
        this.camera.EnableBlackAndWhiteEffect();
        GameController.SetGameState(0.3f);
        Invoke("DeathScreen", 0.55f);
    }

    private void DeathScreen() {
        GameController.SetCanvasVisibility(deathScreen, true);
        progressiveDarkening.StartFading();
    }

    private void DanceTriggered() {
        GameObject enemiesParent = GameObject.Find("Enemies");
        EnemyController[] enemies = enemiesParent.GetComponentsInChildren<EnemyController>();
        
        foreach (EnemyController enemy in enemies)
            enemy.Dance();
    }

    public void SetCanResume(bool statut) {
        this.canResume = statut;
    }

    private int XPRequired() {
        // return (int)(5 * Math.Pow(1.2, this.level - 1));
        return 3;
    }

    // Updates / Increments
    public void UpdateResistance() {
        this.resistance += this.increaseResistance[this.resistanceLevel - 1];
        this.resistanceLevel++;
        if(this.resistance > this.maxResistance) this.resistance = this.maxResistance;
        if(this.resistanceLevel > 5) this.hudStats.MaxResistance();
    }

    public void UpdateAttackSpeed() {
        this.attackSpeed += this.increaseAttackSpeed[this.attackSpeedLevel - 1];
        this.attackSpeedLevel++;
        if(this.attackSpeed < this.minAttackSpeed) this.attackSpeed = this.minAttackSpeed;
        if(this.attackSpeedLevel > 5) this.hudStats.MaxAttackSpeed();
    }

    public void UpdateRange() {
        this.range += this.increaseRange[this.rangeLevel - 1];
        this.rangeLevel++;
        if(this.range > this.maxRange) this.range = this.maxRange;
        if(this.rangeLevel > 5) this.hudStats.MaxRange();

        this.rangeCollider.radius = this.range + this.weaponRange;
    }

    public void UpdateHealth() {
        this.health += this.increaseHealth[this.healthLevel - 1];
        this.maxActualHealth += this.increaseHealth[this.healthLevel - 1];
        this.SetMaxHealthBar(this.maxActualHealth);

        this.healthLevel++;
        if(this.health > this.maxHealth) 
            this.health = this.maxHealth;
            
        this.SetHealthBar(this.health);
        this.hudStats.UpdateHealth();
    }

    public void UpdateAttack() {
        this.attack += this.increaseAttack[this.attackLevel - 1];
        this.knockback += this.increaseKnockback[this.attackLevel - 1];
        this.attackLevel++;
        if(this.attack > this.maxAttack) this.attack = this.maxAttack;
        if(this.knockback > this.maxKnockback) this.knockback = this.maxKnockback;
        if(this.attackLevel > 5) {
            this.hudStats.MaxAttack();
            this.hudStats.MaxKnockback();
        }
    }

    public void UpdateSpeed() {
        this.speed += this.increaseSpeed[this.speedLevel - 1];
        this.speedLevel++;
        if(this.speed > this.maxSpeed) this.speed = this.maxSpeed;
        if(this.speedLevel > 5) this.hudStats.MaxSpeed();
    }

    public void UpdateRegeneration() {
        this.regeneration += this.increaseRegeneration[this.regenerationLevel - 1];
        this.regenerationLevel++;
        if(this.regeneration > this.maxRegeneration) this.regeneration = this.maxRegeneration;
        if(this.regenerationLevel > 5) this.hudStats.MaxRegeneration();
    }

    public void IncrementKillCounter() {
        this.enemyKillCounter++;
    }

    public void IncrementStatCounter() {
        this.hudStats.UpdateStats();
    }


    // Animations
    private void Move() {
        transform.Translate(2 * Vector3.forward * (this.GetSpeed() + this.GetWeaponSpeed() + this.sprint) * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        transform.Translate(2 * Vector3.right   * (this.GetSpeed() + this.GetWeaponSpeed() + this.sprint) * Time.fixedDeltaTime * Input.GetAxis("Horizontal"));
        float moveZ = Input.GetAxis("Vertical");
        this.moveDirection = new Vector3(0, 0, moveZ);
        this.moveDirection = this.transform.TransformDirection(this.moveDirection);

        MoveAnims();
    }

    private void ResetAnims() {
        this.animator.SetInteger("Dance", 0);
        this.animator.SetBool("Idle", false);
        this.animator.SetInteger("Walk", 0);
        this.animator.SetInteger("Strafe", 0);
        this.animator.SetInteger("Strafe_Forward", 0);
        this.animator.SetInteger("Strafe_Backward", 0);
        this.animator.SetInteger("Sprint", 0);
        this.animator.SetInteger("Sprint_Strafe", 0);
        this.animator.SetInteger("Sprint_Forward_Strafe", 0);
    }

    private void MoveAnims() {
        ResetAnims();
        
        if (Input.GetAxis("Vertical") > 0) {
            if(Input.GetAxis("Horizontal") > 0) {
                if (this.isSprinting)
                    this.animator.SetInteger("Sprint_Forward_Strafe", 1);
                else
                    this.animator.SetInteger("Strafe_Forward", 1);
            }
            else if(Input.GetAxis("Horizontal") < 0) {
                if (this.isSprinting)
                    this.animator.SetInteger("Sprint_Forward_Strafe", -1);
                else
                    this.animator.SetInteger("Strafe_Forward", -1);
            }
            else {
                if (this.isSprinting) 
                    this.animator.SetInteger("Sprint", 1);
                else
                    this.animator.SetInteger("Walk", 1);
            }
        } else if(Input.GetAxis("Vertical") < 0) {
            if(Input.GetAxis("Horizontal") > 0)
                this.animator.SetInteger("Strafe_Backward", 1);
            else if (Input.GetAxis("Horizontal") < 0)
                this.animator.SetInteger("Strafe_Backward", -1);
            else
                this.animator.SetInteger("Walk", -1);
        } else {
            if(Input.GetAxis("Horizontal") > 0){
                if (this.isSprinting)
                    this.animator.SetInteger("Sprint_Strafe", 1);
                else
                    this.animator.SetInteger("Strafe", 1);
            }
            else if (Input.GetAxis("Horizontal") < 0){
                if (this.isSprinting)
                    this.animator.SetInteger("Sprint_Strafe", -1);
                else
                    this.animator.SetInteger("Strafe", -1);
            }
            else
                this.animator.SetBool("Idle", true);
        }
    }

    public void WeaponAppearance() {
        GameObject.Find("WeaponHolder").transform.localScale = new Vector3(0.1295791f, 0.1295791f, 0.1295791f);
    }


    private void OnTriggerStay(Collider other) {

        if(this.canAttack) {
            if(this.goingAttack && this.isAlive && !this.inPause && other.CompareTag(Names.BaseEnemy)) {
                EnemyController enemy = other.GetComponent<EnemyController>();
                enemy.TakeDamage(this.attack + this.weaponAttack);
                // enemy.ApplyKnockback();
                this.audio.PlaySlashSFX();
                Invoke("DisableAttack", 0.1f);
            }
        }
    }

    public void DisableAttack() {
        this.goingAttack = false;
        this.canAttack = false;
    }

    private void TakeWeapon() {
        var weapon = GetTheNearestWeapon();
        GetTheWeaponDatas(weapon);
    }

    private void GetTheWeaponDatas(Weapon weapon) {

        if(weapon != null) {

            if(this.weaponName != "") {
                this.weaponsDropper.CreateWeapon(this.weaponID - 1, transform.position);
            }

            this.weaponID = weapon.id;
            this.weaponName = weapon.weaponName;
            this.weaponAttack = weapon.attack;
            this.weaponAttackSpeed = weapon.attackSpeed;
            this.weaponKnockback = weapon.knockback;
            this.weaponRange = weapon.range;
            this.weaponRegeneration = weapon.regeneration;
            this.weaponSpeed = weapon.speed;
            this.rangeCollider.radius = this.range + this.weaponRange;

            GameController.DestroyWeapon(weapon);
            this.hudStats.UpdateStats();

            string weaponNameInHand = "weapon_" + this.weaponID;

            foreach(Transform child in weaponHolder.transform) {
                if(child.gameObject.name == weaponNameInHand) {
                    child.gameObject.SetActive(true);
                } else {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void DisableWeapons() {
        foreach(Transform child in weaponHolder.transform) {
            child.gameObject.SetActive(false);
        }
    }

    private Weapon GetTheNearestWeapon() {

        GameObject weaponsObject = GameObject.Find("Weapons");

        if(weaponsObject != null) {

            Weapon[] weapons = weaponsObject.GetComponentsInChildren<Weapon>();
            
            float minimalDistance = 5.0f;
            Weapon nearestWeapon = null;

            foreach(var weapon in weapons) {
                float distance = Vector3.Distance(transform.position, weapon.transform.position);

                if(distance < minimalDistance) {
                    minimalDistance = distance;
                    nearestWeapon = weapon;
                }
            }

            if(nearestWeapon != null) 
                return nearestWeapon;
        }
        
        return null;
    }


    // Getters
    public bool IsAlive()                { return this.isAlive;            }
    public bool GetCanResume()           { return this.canResume && this.isAlive; }
    public int GetKillCounter()          { return this.enemyKillCounter;   }
 
    public int GetResistance()           { return this.resistance;         }
    public int GetAttack()               { return this.attack;             }
    public int GetHealth()               { return this.health;             }
    public int GetMaxActualHealth()      { return this.maxActualHealth;    }
    public int GetMaxHealth()            { return this.maxHealth;          }
    public int GetLevel()                { return this.level;              }
    public float GetAttackSpeed()        { return this.attackSpeed;        }
    public float GetRange()              { return this.range;              }
    public float GetSpeed()              { return this.speed;              }
    public float GetRegeneration()       { return this.regeneration;       }
    public float GetKnockback()          { return this.knockback;          }
    
    public int GetHealthLevel()          { return this.healthLevel;        }
    public int GetResistanceLevel()      { return this.resistanceLevel;    }
    public int GetAttackLevel()          { return this.attackLevel;        }
    public int GetAttackSpeedLevel()     { return this.attackSpeedLevel;   }
    public int GetRangeLevel()           { return this.rangeLevel;         }
    public int GetSpeedLevel()           { return this.speedLevel;         }
    public int GetRegenerationLevel()    { return this.regenerationLevel;  }
    
    public int GetWeaponAttack()         { return this.weaponAttack;       }
    public string GetWeaponName()        { return this.weaponName;         }
    public float GetWeaponRange()        { return this.weaponRange;        }
    public float GetWeaponAttackSpeed()  { return this.weaponAttackSpeed;  }
    public float GetWeaponKnockback()    { return this.weaponKnockback;    }
    public float GetWeaponSpeed()        { return this.weaponSpeed;        }
    public float GetWeaponRegeneration() { return this.weaponRegeneration; }
    public Animator GetAnimator()        { return this.animator;           }

    public Canvas GetQuestCanvas() { return this.questScreen; }
    public bool IsQuestCanvasVisible() { return this.questMenu.activeSelf; }

    // Setters

    public void SetInPause(bool state) {
        this.inPause = state;
    } 

    private void SetXPBar(int xp) {
        this.xpBar.value = xp;
    }

    private void AddXPBar(int xp) {
        this.xpBar.value += xp;
    } 

    private void SetXPBarMax(int max) {
        this.xpBar.maxValue = max;
    }

    private void SetHealthBar(int hp) {
        this.healthBar.value = hp;
    }

    private void SetMaxHealthBar(int hpMax) {
        this.healthBar.maxValue = hpMax;
    }

    private void SetHealthBarMax(int max) {
        this.healthBar.maxValue = max;
    }

    public void Heal(int healAmount) {
        this.health += healAmount;
        if(this.health >= this.maxActualHealth) 
            this.health = this.maxActualHealth;
        
        this.SetHealthBar(this.health);
        this.hudStats.UpdateHealth();
    }

    public void XPGain(int xpAmount) {
        this.totalXP += xpAmount;
        this.AddXPBar(xpAmount);
        
        if(this.totalXP >= this.xpRequired && this.level < this.maxLevel) {
            this.level++;
            this.xpToNext = XPRequired();
            this.xpRequired += this.xpToNext;
            this.xp = 0;

            this.SetXPBarMax(this.xpToNext);
            this.SetXPBar(0);

            this.SetCanResume(false);
            GameController.SetCanvasVisibility(this.questScreen, false);
            this.levelUpChoice.UpdateStatsDisplay();
        }
    }
}
