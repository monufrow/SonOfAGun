using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public Slider masterVol, musicVol, sfxVol;
    
    public AudioMixer audioMixer;
    private AudioSource sfxSource;

        //SOUND EFFECTS
    public AudioClip hurt;
    public AudioClip jump;
    public AudioClip deathSound;
    public AudioClip deathMusic;
    public AudioClip gunshot;
    public AudioClip reload;
    public AudioClip scorpionNoises;
    public AudioClip scorpionCharge;
    public AudioClip winMusic;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // Add an AudioSource for SFX if not present
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void ChangeMasterVolume()
    {
        audioMixer.SetFloat("MasterVol", masterVol.value);
    }

    public void ChangeMusicVolume()
    {
        audioMixer.SetFloat("MusicVol", musicVol.value);
    }

    public void ChangeSfxVolume()
    {
        audioMixer.SetFloat("SfxVol", sfxVol.value);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
