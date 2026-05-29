using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip eatSound;
    [SerializeField] private AudioClip energizeSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip finalDeathSound;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerManager.PlayerScoreChanged += OnPlayerScoreChanged;
        playerManager.PlayerDeath += OnPlayerDeath;
        playerManager.PlayerEnergize += OnPlayerEnergize;
    }
    private void OnDisable()
    {
        playerManager.PlayerScoreChanged -= OnPlayerScoreChanged;
        playerManager.PlayerDeath -= OnPlayerDeath;
        playerManager.PlayerEnergize -= OnPlayerEnergize;
    }

    private void OnPlayerScoreChanged(int score)
    {
        audioSource.PlayOneShot(eatSound, 0.45f);
    }
    private void OnPlayerDeath(int lives)
    {
        if (lives == 0)
            audioSource.PlayOneShot(finalDeathSound, 0.3f);
        else
            audioSource.PlayOneShot(deathSound, 0.3f);
    }
    private void OnPlayerEnergize(bool energize)
    {
        if (energize)
        {
            audioSource.PlayOneShot(energizeSound, 0.4f);
        }   
    }
}
