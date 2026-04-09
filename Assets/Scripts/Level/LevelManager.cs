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
    [HideInInspector] public List<Item> SpawnedItems = new();

    private int _currentLevel;

    private void OnEnable()
    {
        playerManager.PlayerScoreChanged += OnPlayerScoreChanged;
    }
    private void OnDisable()
    {
        playerManager.PlayerScoreChanged -= OnPlayerScoreChanged;
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
        player.transform.position = playerSpawn.position;
        foreach (GameObject ghost in ghosts)
        {
            ghost.transform.position = ghostSpawn.position;
        }
    }
    private void ToggleMovement(bool toggle)
    {
        foreach (Movement m in movements)
        {
            m.IsPaused = !toggle;
        }
    }

    private void OnPlayerScoreChanged(int score)
    {
        if (SpawnedItems.Count == 0)
        {
            StartCoroutine(NextLevelCorot());
        }
    }

    private IEnumerator NextLevelCorot()
    {
        ToggleMovement(false);
        yield return new WaitForSeconds(2.5f);
        NextLevel();
        yield return new WaitForSeconds(2);
        ToggleMovement(true);
    }
}
