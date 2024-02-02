using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    
    private int randomFS = -1;
    private int randomTheme = -1;
    
    [Header("Sound Effects")]
    [SerializeField] AudioSource[] mainTheme;
    [SerializeField] AudioSource[] slashSFX;
    [SerializeField] AudioSource[] footstepsSFX;
    [SerializeField] AudioSource deathSFX;
    [SerializeField] AudioSource pauseMenuSFX;
    [SerializeField] AudioSource lvlUpSFX;
    [SerializeField] AudioSource lvlUpPanelSFX;

    void Awake() { 
        this.PlayThemeSFX(); 
    }


    public void PlayThemeSFX() {
        this.randomTheme++;
        this.mainTheme[this.randomTheme%3].Play();
        Invoke(nameof(this.PlayThemeSFX), this.mainTheme[this.randomTheme%3].clip.length);
    }

    public void PlayFootstepSFX() {
        this.randomFS++;
        this.footstepsSFX[this.randomFS%2].Play();
    }

    public void PlaySlashSFX() { 
        this.slashSFX[UnityEngine.Random.Range(1, this.slashSFX.Length)].Play(); 
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

    public void PlayLevelUpPanelSFX() { 
        this.lvlUpPanelSFX.Play();
    }

    public void StopPauseMenuSFX() { 
        this.pauseMenuSFX.Stop(); 
    }
    
    public void StopThemeSFX() { 
        this.mainTheme[this.randomTheme%3].Stop();
        CancelInvoke(nameof(this.PlayThemeSFX));
    }

    public void StopLevelUpPanelSFX() { 
        this.lvlUpPanelSFX.Stop(); 
    }
}
