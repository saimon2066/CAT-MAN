using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform playerSpawn;
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

    public event Action<int> LevelChanged;
    public List<Item> SpawnedItems = new();

    private int _currentLevel;

    private void OnEnable()
    {
        playerManager.ScoreChanged += OnScoreChanged;
    }
    private void OnDisable()
    {
        playerManager.ScoreChanged -= OnScoreChanged;
    }
    private void Start()
    {
        NextLevel();
    }

    private void NextLevel()
    {
        _currentLevel++;

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
        /*foreach (Transform spawn in energizerSpawns)
        {
            GameObject obj = Instantiate(itemPrefab, spawn.position, Quaternion.identity, spawn);
            Item item = obj.GetComponent<Item>();

            obj.name = pellet.DisplayName + $"_{spawn.position}";
            item.Score = energizer.Score;
            item.spriteRenderer.sprite = energizer.Sprite;
            item.levelManager = this;

            SpawnedItems.Add(item);
        }*/

        playerManager.Player.transform.position = playerSpawn.position;

        LevelChanged?.Invoke(_currentLevel);
    }
    private void OnScoreChanged(int score)
    {
        if (SpawnedItems.Count == 0)
        {
            StartCoroutine(NextLevelCorot());
        }
    }

    private IEnumerator NextLevelCorot()
    {
        playerManager.SetMovement(false);
        yield return new WaitForSeconds(2.5f);
        NextLevel();
        yield return new WaitForSeconds(2);
        playerManager.SetMovement(true);
    }
}
