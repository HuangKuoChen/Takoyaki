using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour {

    public Image MusicBtn, SoundBtn;
    public Sprite[] MusicSprite = new Sprite[2], SoundSprite = new Sprite[2];
    public AudioSource BackgroundMusic, MenuMusic, EndMusic, ButtonSound, YakiSound, CoinSound, TrashSound;
    private int MusicState, SoundState;

	// Use this for initialization
	void Start () {
        // Check Music and Sound Setting
        if (PlayerPrefs.HasKey("Music") && PlayerPrefs.HasKey("Sound"))
        {
            MusicState = PlayerPrefs.GetInt("Music");
            SoundState = PlayerPrefs.GetInt("Sound");
        } else
        {
            MusicState = 1;
            PlayerPrefs.SetInt("Music", 1);
            SoundState = 1;
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.Save();
        }
        SetMusic();
        SetSound();
    }

    public void ChangeMusicState()
    {
        MusicState = (MusicState + 1) % 2;
        SetMusic();
        PlayerPrefs.SetInt("Music", MusicState);
        PlayerPrefs.Save();
    }

    public void ChangeSoundState()
    {
        SoundState = (SoundState + 1) % 2;
        SetSound();
        PlayerPrefs.SetInt("Sound", SoundState);
        PlayerPrefs.Save();
    }

    public void SetMusic()
    {
        if(MusicState == 0)
        {
            BackgroundMusic.mute = true;
            MenuMusic.mute = true;
            EndMusic.mute = true;
        }
        else
        {
            BackgroundMusic.mute = false;
            MenuMusic.mute = false;
            EndMusic.mute = false;
        }
        MusicBtn.sprite = MusicSprite[MusicState];
    }

    public void SetSound()
    {
        if (SoundState == 0)
        {
            ButtonSound.mute = true;
            YakiSound.mute = true;
            CoinSound.mute = true;
            TrashSound.mute = true;
        }
        else
        {
            ButtonSound.mute = false;
            YakiSound.mute = false;
            CoinSound.mute = false;
            TrashSound.mute = false;
        }
        SoundBtn.sprite = SoundSprite[SoundState];
    }
}
