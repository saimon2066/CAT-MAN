using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform ghostSpawn;
    [SerializeField] private GameObject[] ghosts;
    [Header("Items")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ItemData pellet;
    [SerializeField] private ItemData energizer;
    [SerializeField] private ItemData[] fruits;
    [Header("Item Spawns")]
    [SerializeField] private Transform[] pelletSpawns;
    [SerializeField] private Transform[] energizerSpawns;
    [SerializeField] private Transform fruitSpawn;
    [Header("References")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Movement[] movements;

    public event Action<int> LevelChanged;
    public event Action LevelFailed;
    public event Action LevelRespawnEveryone;

    [HideInInspector] public List<Item> SpawnedItems = new();

    private int _currentLevel;

    private WaitForSeconds _wait2sec = new(2f);
    private WaitForSeconds _wait2p5sec = new (2.5f);

    private void OnEnable()
    {
        playerManager.PlayerScoreChanged += OnPlayerScoreChanged;
        playerManager.PlayerDeath += OnPlayerDeath;
    }
    private void OnDisable()
    {
        playerManager.PlayerScoreChanged -= OnPlayerScoreChanged;
        playerManager.PlayerDeath -= OnPlayerDeath;
    }
    private void Start()
    {
        NextLevel();
    }

    private void NextLevel()
    {
        _currentLevel++;

        SpawnItems();
        RespawnEveryone();

        LevelChanged?.Invoke(_currentLevel);
    }
    private void LevelFail()
    {
        LevelFailed?.Invoke();
        _currentLevel = 0;
    }

    private void SetMovementPaused(bool isPaused)
    {
        foreach (Movement m in movements)
        {
            m.IsPaused = isPaused;
        }
    }
    private void SpawnItems()
    {
        foreach (Transform spawn in pelletSpawns)
        {
            GameObject obj = Instantiate(itemPrefab, spawn.position, Quaternion.identity, spawn);
            Item item = obj.GetComponent<Item>();

            obj.name = pellet.DisplayName + $"_{spawn.position}";
            item.Score = pellet.Score;
            item.spriteRenderer.sprite = pellet.Sprite;
            item.levelManager = this;

            SpawnedItems.Add(item);
        }
        foreach (Transform spawn in energizerSpawns)
        {
            GameObject obj = Instantiate(itemPrefab, spawn.position, Quaternion.identity, spawn);
            Item item = obj.GetComponent<Item>();

            obj.name = pellet.DisplayName + $"_{spawn.position}";
            item.Score = energizer.Score;
            item.spriteRenderer.sprite = energizer.Sprite;
            item.levelManager = this;
            item.DoesEnergize = energizer.DoesEnergize;

            SpawnedItems.Add(item);
        }
    }
    private void RespawnEveryone()
    {
        LevelRespawnEveryone?.Invoke();

        player.transform.position = playerSpawn.position;
        foreach (GameObject ghost in ghosts)
        {
            ghost.transform.position = ghostSpawn.position;
        }
    }
    private void OnPlayerDeath(int lives)
    {
        if (lives == 0)
        {
            StartCoroutine(LevelFailCorot());
        }
        else
        {
            StartCoroutine(PlayerDeathCorot());
        }
    }

    private IEnumerator NextLevelCorot()
    {
        SetMovementPaused(true);
        yield return _wait2p5sec;
        NextLevel();
        yield return _wait2sec;
        SetMovementPaused(false);
    }
    private IEnumerator PlayerDeathCorot()
    {
        SetMovementPaused(true);
        yield return _wait2p5sec;
        RespawnEveryone();
        yield return _wait2sec;
        SetMovementPaused(false);
        LevelChanged?.Invoke(_currentLevel);
    }

    private void OnPlayerScoreChanged(int score)
    {
        if (SpawnedItems.Count == 0)
        {
            StartCoroutine(NextLevelCorot());
        }
    }
    private IEnumerator LevelFailCorot()
    {
        SetMovementPaused(true);
        yield return _wait2p5sec;
        LevelFail();
        yield return _wait2sec;
        SetMovementPaused(false);
        NextLevel();
    }
}
