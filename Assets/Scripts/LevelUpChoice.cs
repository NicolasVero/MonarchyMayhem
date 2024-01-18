using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour {

    [Header("Attributs")]
    [SerializeField] private GameObject[] banners;
    [SerializeField] private GameObject levelUpPanel;

    [Header("Classes utilitaires")]
    [SerializeField] private HUDStats hudStats;
    [SerializeField] private PlayerController playerController;

    private string[] bannersNames;
    private int[] bannersLevel;
    private int bannersLength = 7;
    private System.Random random = new System.Random();

    public void Awake() {


        this.bannersNames = new string[] {
            "attack_",
            "health_",
            "resistance_",
            "range_",
            "attack_speed_",
            "speed_",
            "regeneration_"
        };
    }

    public void updateStatsDisplay() {

        this.bannersLevel = new int[] {
            this.playerController.getAttackLevel(),
            this.playerController.getHealthLevel(),
            this.playerController.getResistanceLevel(),
            this.playerController.getRangeLevel(),
            this.playerController.getAttackSpeedLevel(),
            this.playerController.getSpeedLevel(),
            this.playerController.getRegenerationLevel()
        };

        List<int> isMaxLevel = new List<int>();

        for(int i = 0; i < this.bannersLevel.Length; i++) 
            if(this.bannersLevel[i] > 5) 
                isMaxLevel.Add(i);
            
        int[] excludes = this.generateUniquesRandom(0, this.bannersLength, isMaxLevel.ToArray());


        GameController.setGameState(false);
        GameController.setPanelVisibility(levelUpPanel, true);
        this.hideBanners(excludes);
        GameController.setCursorVisibility(true);
    }

    public void choiceAttack() {
        this.playerController.updateAttack();
        this.resumeGame();
    }

    public void choiceHealth() {
        this.playerController.updateHealth();
        this.resumeGame();
    }

    public void choiceResistance() {
        this.playerController.updateResistance();
        this.resumeGame();
    }

    public void choiceAttackSpeed() {
        this.playerController.updateAttackSpeed();
        this.resumeGame();
    }

    public void choiceRange() {
        this.playerController.updateRange();
        this.resumeGame();
    }

    public void choiceSpeed() {
        this.playerController.updateSpeed();
        this.resumeGame();
    }

    public void choiceRegeneration() {
        this.playerController.updateRegeneration();
        this.resumeGame();
    }

    void resumeGame() {
        this.playerController.setCanResume(true);
        this.hudStats.updateStats();
        
        for(int i = 0; i < this.bannersLength; i++)
            GameController.setPanelVisibility(banners[i], true);

        GameController.setGameState(true);
        GameController.setCursorVisibility(false);
        GameController.setPanelVisibility(levelUpPanel, false);   
    }

    public int[] generateUniquesRandom(int min, int max, int[] excludes) {
        HashSet<int> uniqueNumbers = new HashSet<int>(excludes);

        while(uniqueNumbers.Count < Math.Max(3, excludes.Length)) {
            int randomNumber = random.Next(min, max + 1);

            if(!uniqueNumbers.Contains(randomNumber)) 
                uniqueNumbers.Add(randomNumber);     
        }

        return uniqueNumbers.ToArray();
    }


    private void hideBanners(int[] excludes) {
        int[][] positions = {
            new int[] { 400, 225},
            new int[] { 700, 225},
            new int[] {1000, 225}
        };

        for(int i = 0; i < this.bannersLength; i++) 
            GameController.setPanelVisibility(this.banners[i], false);
        
        int cpt = 0;

        for (int i = 0; i < this.bannersLength; i++) {
            if(Array.IndexOf(excludes, i) == -1) {
                this.banners[i].transform.position = new Vector3(positions[cpt][0], positions[cpt][1], 0);
                RectTransform rectTransform = banners[i].GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(175, 350);

                this.banners[i].GetComponent<Image>().sprite = LoadBannerSprite(this.bannersNames[i], this.bannersLevel[i]);
                GameController.setPanelVisibility(this.banners[i], true);

                cpt++;

                if(cpt >= excludes.Length) 
                    break;    
            }
        }
    }

    private Sprite LoadBannerSprite(string spriteName, int level) {
        return Resources.Load<Sprite>("Interface/Banners/" + spriteName + level);
    }
}
