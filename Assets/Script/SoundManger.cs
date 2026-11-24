using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public static SoundManger Instance { get; private set; }
    // Start is called before the first frame update

    [SerializeField]private AudioSource audioSourceShot;
    [SerializeField]private AudioClip[] clips;

    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one SoundManger in the scene");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayShotSound(int index)
    {
        audioSourceShot.PlayOneShot(clips[index]);
    }

}
