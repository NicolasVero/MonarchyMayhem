using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DanceWheelController : MonoBehaviour {

    [SerializeField] private Canvas danceMenu;
    [SerializeField] private GameObject danceText;
    private PlayerController playerController;
    private Animator animator;
    private bool danceWheelSelected;
    private bool canOpenWheelMenu = true;

    void Awake() {
        this.animator = GetComponent<Animator>();
        GameController.SetPanelVisibility(this.danceText, false);
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && this.canOpenWheelMenu) {
            this.danceWheelSelected = !this.danceWheelSelected;
            GameController.SetCursorVisibility(this.danceWheelSelected);
            this.ToggleWheelAnimation(this.danceWheelSelected);
        }
    }

    public void DanceAnimations(int danceID) {
        this.DisableCanOpenMenu();
        this.playerController.DisableAttack();

        this.danceWheelSelected = false;
        this.ToggleWheelAnimation(this.danceWheelSelected);
        GameController.SetCursorVisibility(this.danceWheelSelected);

        float[] weaponsAppearanceDurations = {0f, 6.5f, 8.5f, 8f, 9.5f, 3.5f, 1.5f};
        
        GameObject.Find("WeaponHolder").transform.localScale = new Vector3(0, 0, 0);
        this.playerController.Invoke("WeaponAppearance", weaponsAppearanceDurations[danceID]);
        
        Invoke("EnableCanOpenMenu", weaponsAppearanceDurations[danceID]);

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
    }

    private void ToggleWheelAnimation(bool state) { this.animator.SetBool("OpenDanceWheel", state); }
    private void EnableCanOpenMenu() { this.canOpenWheelMenu = true; }
    private void DisableCanOpenMenu() { this.canOpenWheelMenu = false; }

}
