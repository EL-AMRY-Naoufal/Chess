using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeMusic : MonoBehaviour
{
    [SerializeField] Slider VolumeSlider;

    public Sprite MuteIcone;
    public Sprite UnmuteIcone;
    public Button MuteBtn;

    private bool isMuted;
    void Start()
    {
        isMuted = PlayerPrefs.GetInt("MUTED") == 1;
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Laod();
        }
        else
        {
            Laod();
        }    
    }

    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        Save();
    }

    private void Laod()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", VolumeSlider.value);
    }

    public void MuteMusic()
    {
        Debug.Log(isMuted);
        if (isMuted)
            MuteBtn.image.sprite = UnmuteIcone;
        else
            MuteBtn.image.sprite = MuteIcone;

        isMuted = !isMuted;
        AudioListener.pause = isMuted;
        PlayerPrefs.GetInt("MUTED", isMuted ? 1 : 0);

    }

}
