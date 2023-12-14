using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour {

    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject resistance;
    [SerializeField] private GameObject range;
    [SerializeField] private GameObject attackSpeed;
    [SerializeField] private GameObject speed;
    [SerializeField] private HUDStats   hudStats;
   
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject       levelUpPanel;

    private GameObject[] banners;
    private int bannersLength = 6;
    private System.Random random = new System.Random();

    private int choiceMade;

    public void Awake() {
        
    }

    public void updateStatsDisplay() {

        this.banners = new GameObject[] {
            this.attack, 
            this.health, 
            this.resistance, 
            this.range, 
            this.attackSpeed, 
            this.speed
        };

        int[] excludes = this.generateUniquesRandom();

        // for(int i = 0; i < excludes.Length; i++) {
        //     // Debug.Log(excludes[i]);
        // }

        GameController.setGameState(false);
        GameController.setPanelVisibility(levelUpPanel, true);
        // GameController.setPanelVisibility(banners[this.random.Next(this.bannersLength - 1)], false);
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

    void resumeGame() {
        this.playerController.setCanResume(true);
        this.hudStats.updateStats();
        
        for(int i = 0; i < this.bannersLength; i++)
            GameController.setPanelVisibility(banners[i], true);

        GameController.setGameState(true);
        GameController.setCursorVisibility(false);
        GameController.setPanelVisibility(levelUpPanel, false);   
    }

    void Update() {
        if(choiceMade >= 1) {
            this.attack.SetActive(false);
            this.health.SetActive(false);
            this.resistance.SetActive(false);
            this.range.SetActive(false);
        }
    }

    public int[] generateUniquesRandom(int min = 0, int max = 6) {        
        return Enumerable.Range(min, max)
            .OrderBy(_ => this.random.Next())
            .Take(3)
            .ToArray();
    }

    private void hideBanners(int[] excludes) {
        int[][] positions = {
            new int[] { 400, 350},
            new int[] { 700, 350},
            new int[] {1000, 350}
        };

        for(int i = 0; i < excludes.Length; i++) {
            GameController.setPanelVisibility(banners[excludes[i]], false);
            // Debug.Log(banners[excludes[i]].position.x);
            Transform bannerTransform = banners[excludes[i]].transform;
            // Debug.Log($"Position x de la bannière {excludes[i]} : {bannerTransform.position.x}");
        }

        int cpt = 0;
        // int[] includes = new int[3];
        // Sprite banner = Resources.Load<Sprite>("Interface/banners/banner");

        for(int i = 0; i < this.bannersLength; i++) {
            if(Array.IndexOf(excludes, i) == -1) {
                // includes[cpt] = i;
                banners[i].transform.position = new Vector3(positions[cpt][0], positions[cpt][1], 0);


                Debug.Log("banner img");
                Debug.Log(banners[i].gameObject.GetComponent<Image>().sprite);
                Debug.Log("sprite ");
                Debug.Log(LoadBannerSprite("spriteName"));

                banners[i].GetComponent<Image>().sprite = LoadBannerSprite("spriteName");
                cpt++;
            }
        }

        // Debug.Log("TEEEEST : " + includes[0]);
        // Debug.Log("TEEEEST : " + includes[1]);
        // Debug.Log("TEEEEST : " + includes[2]);

    }

    private Sprite LoadBannerSprite(string spriteName) {
        return Resources.Load<Sprite>("Interface/Banners/banner");
    }
}
