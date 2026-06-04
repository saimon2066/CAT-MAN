using UnityEngine;

public class ButtonSoundPlay : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioSource audioSource;

    public void PlaySound()
    {
        audioSource.PlayOneShot(clickSound, 0.7f);
    }
}
