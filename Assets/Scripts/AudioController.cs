using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    
    private int randomFS = -1;
    private int randomTheme = -1;
    
    [Header("Sound Effects")]
    [SerializeField] AudioSource[] mainTheme;
    [SerializeField] AudioSource[] slashSFX;
    [SerializeField] AudioSource pickUpSFX;
    [SerializeField] AudioSource deathSFX;
    [SerializeField] AudioSource pauseMenuSFX;
    [SerializeField] AudioSource lvlUpSFX;

    void Awake() { 
        this.PlayThemeSFX(); 
    }

    public void PlayThemeSFX() {
        this.randomTheme = GameController.Random(0, this.mainTheme.Length - 1);
        this.mainTheme[this.randomTheme].Play();
        Invoke(nameof(this.PlayThemeSFX), this.mainTheme[this.randomTheme].clip.length);
    }

    public void PlaySlashSFX() { 
        this.slashSFX[GameController.Random(0, this.slashSFX.Length - 1)].Play(); 
    }

    public void PlayDeathSFX() { 
        this.deathSFX.Play(); 
    }

    public void PlayPauseMenuSFX() { 
        this.pauseMenuSFX.Play(); 
    }

    public void PlayLevelUpSFX() { 
        this.lvlUpSFX.Play(); 
    }

    public void StopPauseMenuSFX() { 
        this.pauseMenuSFX.Stop(); 
    }
    
    public void StopThemeSFX() { 
        this.mainTheme[this.randomTheme].Stop();
        CancelInvoke(nameof(this.PlayThemeSFX));
    }

    public void PlayPickUpSFX() {
        this.pickUpSFX.Play();
    }
}
