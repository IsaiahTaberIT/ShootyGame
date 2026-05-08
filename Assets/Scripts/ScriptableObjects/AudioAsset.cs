using UnityEngine;
using static Unity.VisualScripting.Member;

[CreateAssetMenu(fileName = "AudioAsset", menuName = "Scriptable Objects/AudioAsset")]
public class AudioAsset : ScriptableObject
{
    public AudioClip[] clips = new AudioClip[1];


    public void PlayNew()
    {
        GameObject g = new GameObject();
        AudioSource source = g.AddComponent<AudioSource>();
        DecayScript d = g.AddComponent<DecayScript>();




        source.clip = clips[0];
        d.DecayTimer.EndTime = source.clip.length;
        
        source.Play();

    }
 
    [ContextMenu ("Play")]

    
    public void Play(AudioSource source)
    {
        source.clip = clips[0];
        source.Play();
    }

    public void Play()
    {
        if (!GameController.Controller || !GameController.Controller.GlobalAudioSource)
        {
            return;
        }

        Play(GameController.Controller.GlobalAudioSource);

    }



}
