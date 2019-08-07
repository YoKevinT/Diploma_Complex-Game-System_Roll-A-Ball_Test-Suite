using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.TestTools;
using NUnit.Framework;

public class TestSuite
{
    private GameObject game;
    private GameManager gameManager;
    private Player player;

    [SetUp]
    public void Setup()
    {
        // Load and Spawn Game Prefab
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Game");
        game = Object.Instantiate(prefab);
        // Get GameManager
        gameManager = game.GetComponent<GameManager>();
        //player = Object.FindObjectOfType<Player>();
        player = game.GetComponentInChildren<Player>();
    }

    [UnityTest]
    public IEnumerator GamePrefabLoaded()
    {
        yield return new WaitForEndOfFrame();

        // Game Object should exist at this point in time
        Assert.NotNull(game, "Insert Message Here");
    }

    [UnityTest]
    public IEnumerator PlayerExists()
    {
        yield return new WaitForEndOfFrame();

        Assert.NotNull(player, "Insert Message Here2");
    }

    [UnityTest]
    public IEnumerator ItemCollidesWithPlayer()
    {
        //Item item = gameManager.itemManager.GetItem(0);
        //item.transform.position = player.transform.position;

        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Entities/Item");

        Vector3 playerPosition = player.transform.position;
        GameObject item = Object.Instantiate(itemPrefab, playerPosition, Quaternion.identity);

        //yield return new WaitForSeconds(.1f);
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();

        Assert.IsTrue(item == null);
    }

    [UnityTest]
    public IEnumerator ItemCollectedAndScoreAdded()
    {
        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Entities/Item");

        Vector3 playerPosition = player.transform.position;
        GameObject item = Object.Instantiate(itemPrefab, playerPosition, Quaternion.identity);

        int oldScore = gameManager.score;

        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();

        int newScore = gameManager.score;

        Assert.IsTrue(newScore > oldScore);
    }

    [UnityTest]
    public IEnumerator GameFinish()
    {
        gameManager.gameover = false;

        Item[] items = Object.FindObjectsOfType<Item>();
        foreach (var item in items)
        {
            item.Collect();
        }

        yield return new WaitForSeconds(.1f);

        Assert.IsTrue(gameManager.gameover);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(game);
    }
}