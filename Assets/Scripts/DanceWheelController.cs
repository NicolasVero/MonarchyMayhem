using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanceWheelController : MonoBehaviour {

    [SerializeField] private Canvas danceMenu;
    [SerializeField] private GameObject danceText;
    private List<string> listDances = new List<string> { "Fortnite_Dance", "ElectroShuffle", "RobotDance", "BreakDance", "Wave", "IBreakYou" };
    private PlayerController playerController;
    private Animator animator;
    private bool danceWheelSelected;
    private bool canOpenWheelMenu = true;
    private QuestController questController;

    void Awake() {
        this.animator = GetComponent<Animator>();
        GameController.SetPanelVisibility(this.danceText, false);
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
    }

    void Update() {

        if(this.danceWheelSelected)
            this.playerController.DisableAttack();

        if (Input.GetKeyDown(KeyCode.F) && this.canOpenWheelMenu) {
            this.playerController.SetRotation(this.danceWheelSelected);
            this.danceWheelSelected = !this.danceWheelSelected;
            GameController.SetCursorVisibility(this.danceWheelSelected);
            this.ToggleWheelAnimation(this.danceWheelSelected);
        }
        try {
            if (listDances.IndexOf(this.playerController.GetAnimator().GetCurrentAnimatorClipInfo(this.playerController.GetAnimator().GetLayerIndex("Movement Layer"))[0].clip.name) != -1) {
                this.DisableCanOpenMenu();
                GameObject.Find("WeaponHolder").transform.localScale = new Vector3(0, 0, 0);
            }
            else if (listDances.IndexOf(this.playerController.GetAnimator().GetCurrentAnimatorClipInfo(this.playerController.GetAnimator().GetLayerIndex("UpperBody Layer"))[0].clip.name) != -1) {
                this.DisableCanOpenMenu(); 
                GameObject.Find("WeaponHolder").transform.localScale = new Vector3(0, 0, 0);
            }
        }
        catch { 
            this.EnableCanOpenMenu();
            this.playerController.WeaponAppearance();
        }
    }

    public void InitQuestController(){
        this.questController = GameObject.FindGameObjectWithTag("QuestCanvas").GetComponent<QuestController>();
    }

    public void DanceAnimations(int danceID) {

        if (danceID >= 1 && danceID <= 4) {
            this.playerController.IncrementDanceCounter();
            this.questController.UpdateQuestText();
        }

        this.danceWheelSelected = false;
        this.ToggleWheelAnimation(this.danceWheelSelected);
        GameController.SetCursorVisibility(this.danceWheelSelected);

        switch (danceID){
            case 0:
                // Quitter la roue
                this.danceWheelSelected = false;
                break;
            case 1:
                // Fortnite
                this.playerController.GetAnimator().SetInteger("Dance", danceID);
                break;
            case 2:
                // Electroshuffle                
                this.playerController.GetAnimator().SetInteger("Dance", danceID);
                break;
            case 3:
                // Breakdance
                this.playerController.GetAnimator().SetInteger("Dance", danceID);
                break;
            case 4:
                // Robot Dance
                this.playerController.GetAnimator().SetInteger("Dance", danceID);
                break;
            case 5:
                this.playerController.GetAnimator().SetTrigger("Ibreakyou");
                break;
            case 6:
                this.playerController.GetAnimator().SetTrigger("Wave");
                break;
        }

        this.playerController.SetRotation(true);
    }

    private void ToggleWheelAnimation(bool state) { 
        this.animator.SetBool("OpenDanceWheel", state);
    }

    private void EnableCanOpenMenu() { 
        this.canOpenWheelMenu = true; 
    }

    private void DisableCanOpenMenu() { 
        this.canOpenWheelMenu = false; 
    }
}
