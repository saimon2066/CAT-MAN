using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip eatSound;
    [SerializeField] private AudioClip energizeSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip finalDeathSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioSource audioSource;

    private int _currentLevel;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerManager.PlayerScoreChanged += OnPlayerScoreChanged;
        playerManager.PlayerDeath += OnPlayerDeath;
        playerManager.PlayerEnergize += OnPlayerEnergize;
        levelManager.LevelChanged += OnLevelChanged;
    }
    private void OnDisable()
    {
        playerManager.PlayerScoreChanged -= OnPlayerScoreChanged;
        playerManager.PlayerDeath -= OnPlayerDeath;
        playerManager.PlayerEnergize -= OnPlayerEnergize;
        levelManager.LevelChanged -= OnLevelChanged;
    }

    private void OnPlayerScoreChanged(int score)
    {
        audioSource.PlayOneShot(eatSound, 0.75f);
    }
    private void OnPlayerDeath(int lives)
    {
        if (lives == 0)
            audioSource.PlayOneShot(finalDeathSound, 0.2f);
        else
            audioSource.PlayOneShot(deathSound, 0.4f);
    }
    private void OnPlayerEnergize(bool energize)
    {
        if (energize)
        {
            audioSource.PlayOneShot(energizeSound, 0.65f);
        }   
    }
    private void OnLevelChanged(int level)
    {
        if (level != 1 && level != _currentLevel)
        {
            audioSource.PlayOneShot(winSound, 0.9f);
            _currentLevel = levelManager.CurrentLevel;
        }
    }
}
