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

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
        _audioSource.PlayOneShot(eatSound, 0.2f);
    }
    private void OnPlayerDeath(int lives)
    {
        if (lives == 0)
            _audioSource.PlayOneShot(finalDeathSound, 0.1f);
        else
            _audioSource.PlayOneShot(deathSound, 0.1f);
    }
    private void OnPlayerEnergize(bool energize)
    {
        if (energize)
        {
            _audioSource.PlayOneShot(energizeSound, 0.2f);
        }   
    }
}
