using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    
    private int randomTheme = -1;
    private int randomBossTheme = 0;
    private int randomMenuSFX = -1;
    
    [Header("Sound Effects")]
    [SerializeField] AudioSource[] mainTheme;
    [SerializeField] AudioSource[] menuSFX;
    [SerializeField] AudioSource[] slashSFX;
    [SerializeField] AudioSource[] bossTheme;
    [SerializeField] AudioSource pickUpSFX;
    [SerializeField] AudioSource deathSFX;
    [SerializeField] AudioSource pauseMenuSFX;
    [SerializeField] AudioSource lvlUpSFX;


    public void PlayThemeSFX() {
        this.randomTheme = GameController.Random(0, this.mainTheme.Length - 1);
        this.mainTheme[this.randomTheme].Play();
        Invoke(nameof(this.PlayThemeSFX), this.mainTheme[this.randomTheme].clip.length);
    }

    public void PlayMenuSFX() {
        this.randomMenuSFX = GameController.Random(0, this.menuSFX.Length - 1);
        this.menuSFX[this.randomMenuSFX].Play();
        Invoke(nameof(this.PlayMenuSFX), this.menuSFX[this.randomMenuSFX].clip.length);
    }

    public void PlayBossThemeSFX(int music) {
        this.randomBossTheme = music;
        this.bossTheme[this.randomBossTheme].Play();
        Invoke(nameof(this.PlayBossThemeSFX), this.bossTheme[this.randomBossTheme].clip.length);
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

    public void StopMenuSFX() { 
        this.menuSFX[this.randomMenuSFX].Stop();
        CancelInvoke(nameof(this.PlayMenuSFX));
    }

    public void StopBossThemeSFX() { 
        this.bossTheme[this.randomBossTheme].Stop();
        CancelInvoke(nameof(this.PlayBossThemeSFX));
    }

    public void PlayPickUpSFX() {
        this.pickUpSFX.Play();
    }
}
