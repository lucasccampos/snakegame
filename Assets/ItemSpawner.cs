using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner instance;
    [SerializeField] private BoxCollider2D gridArea;
    public List<GameObject> itensPrefab;

    private float spawnCounter = 0;
    public float spawnThreshold = 5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GameManager.OnGameStart += GameStart;
    }

    private Vector3 RandomItemPosition()
    {
        Bounds bounds = this.gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(Mathf.Round(x), Mathf.Round(y), 0f);
    }

    void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        spawnCounter += Time.deltaTime;

        if (spawnCounter < spawnThreshold)
        {
            return;
        }

        spawnCounter -= spawnThreshold;

        SpawnRandomItem();

    }

    void SpawnRandomItem()
    {
        Vector3 spawn_pos = RandomItemPosition();

        Instantiate(this.itensPrefab[Random.Range(0, this.itensPrefab.Count)], spawn_pos, Quaternion.identity);
    }


    void GameStart()
    {
        ClearItens();
        SpawnRandomItem();
    }

    void ClearItens()
    {
        Item[] itensSpawned = Object.FindObjectsOfType<Item>();

        foreach (Item item in itensSpawned)
        {
            Destroy(item.gameObject);
        }
    }


}
