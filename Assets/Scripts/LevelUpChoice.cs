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
    [SerializeField] private AudioController _audio;

    private string[] bannersNames;
    private int[] bannersLevel;
    private int bannersLength = 7;

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

    public void UpdateStatsDisplay() {
        this.bannersLevel = new int[] {
            this.playerController.GetAttackLevel(),
            this.playerController.GetHealthLevel(),
            this.playerController.GetResistanceLevel(),
            this.playerController.GetRangeLevel(),
            this.playerController.GetAttackSpeedLevel(),
            this.playerController.GetSpeedLevel(),
            this.playerController.GetRegenerationLevel()
        };

        List<int> isMaxLevel = new List<int>();

        for(int i = 0; i < this.bannersLevel.Length; i++) 
            if(this.bannersLevel[i] > 5) 
                isMaxLevel.Add(i);


        int[] excludes = this.GenerateUniquesRandom(0, this.bannersLength - 1, isMaxLevel.ToArray());

        this._audio.PlayLevelUpSFX();

        GameController.SetGameState(false);
        GameController.SetPanelVisibility(levelUpPanel, true);
        this.HideBanners(excludes);
        GameController.SetCursorVisibility(true);
    }


    public int[] GenerateUniquesRandom(int min, int max, int[] excludes) {
        HashSet<int> uniqueNumbers = new HashSet<int>(excludes);

        while(uniqueNumbers.Count < Math.Max(this.bannersLength - 3, excludes.Length)) {
            int randomNumber = GameController.Random(min, max);

            if(!uniqueNumbers.Contains(randomNumber)) 
                uniqueNumbers.Add(randomNumber);     
        }

        return uniqueNumbers.ToArray();
    }

    private int[][] GetPositionArray() {
        int initialXPosition = 325;
        int initialYPosition = 250;
        int marginX = 250;
        int marginY = 0;

        int[][] positions = new int[3][];

        for(int i = 0; i < 3; i++) {
            int[] position = new int[2];
            position[0] = initialXPosition + i * marginX;
            position[1] = initialYPosition + i * marginY;
            positions[i] = position; 
        }

        return positions;
    }

    private void HideBanners(int[] excludes) {

        int[][] positions = this.GetPositionArray();

        for(int i = 0; i < this.bannersLength; i++) 
            GameController.SetPanelVisibility(this.banners[i], false);
        

        int cpt = 0;
        for (int i = 0; i < this.bannersLength; i++) {
            if(Array.IndexOf(excludes, i) == -1) {
        
                this.banners[i].transform.position = new Vector3(positions[cpt][0], positions[cpt][1], 0);
                RectTransform rectTransform = banners[i].GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(175, 350);

                this.banners[i].GetComponent<Image>().sprite = LoadBannerSprite(this.bannersNames[i], this.bannersLevel[i]);
                GameController.SetPanelVisibility(this.banners[i], true);

                cpt++;

                if(cpt >= excludes.Length) 
                    break;    
            }
        }
    }




    void ResumeGame() {
        this.playerController.SetCanResume(true);
        this.hudStats.UpdateStats();
        
        for(int i = 0; i < this.bannersLength; i++)
            GameController.SetPanelVisibility(banners[i], true);
        
        GameController.SetGameState(true);
        GameController.SetCursorVisibility(false);
        GameController.SetPanelVisibility(levelUpPanel, false);   
        GameController.SetCanvasVisibility(this.playerController.GetQuestCanvas(), this.playerController.IsQuestCanvasVisible());
    }


    public void ChoiceAttack() {
        this.playerController.UpdateAttack();
        this.ResumeGame();
    }

    public void ChoiceHealth() {
        this.playerController.UpdateHealth();
        this.ResumeGame();
    }

    public void ChoiceResistance() {
        this.playerController.UpdateResistance();
        this.ResumeGame();
    }

    public void ChoiceAttackSpeed() {
        this.playerController.UpdateAttackSpeed();
        this.ResumeGame();
    }

    public void ChoiceRange() {
        this.playerController.UpdateRange();
        this.ResumeGame();
    }

    public void ChoiceSpeed() {
        this.playerController.UpdateSpeed();
        this.ResumeGame();
    }

    public void ChoiceRegeneration() {
        this.playerController.UpdateRegeneration();
        this.ResumeGame();
    }

    private Sprite LoadBannerSprite(string spriteName, int level) {
        return Resources.Load<Sprite>("Interface/Banners/" + spriteName + level);
    }
}
