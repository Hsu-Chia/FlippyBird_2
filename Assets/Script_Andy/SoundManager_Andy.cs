using CodeMonkey.Utils;
using UnityEngine;

public static class SoundManager_Andy 
{
    public enum Sound
    {
        birdJump,
        Score,
        Lose,
        ButtonOver,
        ButtonClick,
    }

    public static void PlaySound(Sound sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        
        AudioClip clip = GetAudioClip(sound);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
            Object.Destroy(gameObject, clip.length);
        }
        else
        {
            Debug.LogWarning("❌ 找不到對應音效: " + sound);
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (var soundAudioClip in GameAssets_Andy.GetInstance().soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        return null;
    }

    public static void AddButtonSounds(this Button_UI buttonUI)
    {
        var originalClick = buttonUI.ClickFunc;
        buttonUI.ClickFunc = () =>
        {
            PlaySound(Sound.ButtonClick);
            originalClick?.Invoke();
        };

        buttonUI.MouseOverOnceFunc = () => PlaySound(Sound.ButtonOver);

    }

}
