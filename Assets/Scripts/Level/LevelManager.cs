using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public event Action LevelRespawnEveryone;

    [HideInInspector] public List<Item> SpawnedItems = new();

    public int CurrentLevel;
    private int _levelEndScore;

    private int _fruitSpawned;
    private Item _currentFruit;

    private Coroutine _fruitCorot;
    
    private WaitForSeconds _wait2sec = new(2f);
    private WaitForSeconds _wait2p5sec = new (2.5f);
    private WaitForSeconds _wait9sec = new(9f);

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
        _fruitSpawned = 0;
        CurrentLevel++;

        SpawnItems();
        RespawnEveryone();

        LevelChanged?.Invoke(CurrentLevel);
    }
    private void LevelFail()
    {
        CurrentLevel = 0;
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
        if (SpawnedItems.Count > 0) for (int i = 0; i < SpawnedItems.Count; i++)
        {
            SpawnedItems[i].Pickup();
        }

        if (_fruitCorot != null) StopCoroutine(_fruitCorot);
        if (_currentFruit) _currentFruit.Pickup();
        _currentFruit = null;

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

            obj.name = energizer.DisplayName + $"_{spawn.position}";
            item.Score = energizer.Score;
            item.spriteRenderer.sprite = energizer.Sprite;
            item.levelManager = this;
            item.DoesEnergize = energizer.DoesEnergize;

            SpawnedItems.Add(item);
        }
    }
    private void SpawnFruit()
    {
        _fruitSpawned++;
        
        int i = Mathf.Clamp(CurrentLevel - 1, 0, fruits.Length);

        GameObject obj = Instantiate(itemPrefab, fruitSpawn.position, Quaternion.identity, fruitSpawn);
        Item item = obj.GetComponent<Item>();

        obj.name = fruits[i].DisplayName + $"_{fruitSpawn.position}";
        item.Score = fruits[i].Score;
        item.spriteRenderer.sprite = fruits[i].Sprite;
        item.levelManager = this;
        item.DoesEnergize = fruits[i].DoesEnergize;

        _currentFruit = item;
        _fruitCorot ??= StartCoroutine(FruitSpawnCorot(item));
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
    private void OnPlayerScoreChanged(int score)
    {
        if (SpawnedItems.Count == 0)
        {
            _levelEndScore = score;
            StartCoroutine(NextLevelCorot());
        }
        else
        {
            if (!SpawnedItems.Contains(_currentFruit))
            {
                if (score >= _levelEndScore + 700 && _fruitSpawned < 1)
                {
                    SpawnFruit();
                }
                else if (score >= _levelEndScore + 2000 && _fruitSpawned < 2)
                {
                    SpawnFruit();
                }
            }
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
        LevelChanged?.Invoke(CurrentLevel);
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
    private IEnumerator FruitSpawnCorot(Item fruit)
    {
        yield return _wait9sec;
        if (fruit) fruit.Pickup();
    }
}
