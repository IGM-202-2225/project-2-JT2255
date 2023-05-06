using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgentManager : MonoBehaviour
{
    public static AgentManager Instance;

    public BettaFish bettaPrefab;
    public Goldfish goldPrefab;
    public GameObject fishFoodPrefab;
    //public int numFishFood = 10;
    public int numGoldfish = 10;
    public int numBettaFish = 10;
    public int numTagPlayers = 10;

    [HideInInspector] public List<BettaFish> bettaFishes = new List<BettaFish>();

    [HideInInspector] public List<Goldfish> goldfishes = new List<Goldfish>();

    [HideInInspector] public List<GameObject> fishFoodList = new List<GameObject>();

    [HideInInspector]
    public Vector2 maxPosition = Vector2.one;
    [HideInInspector]
    public Vector2 minPosition = -Vector2.one;

    public float edgePadding = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Camera cam = Camera.main;

        if (cam != null)
        {
            Vector3 camPosition = cam.transform.position;
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;

            maxPosition.x = camPosition.x + halfWidth - edgePadding;
            maxPosition.y = camPosition.y + halfHeight - edgePadding;
            minPosition.x = camPosition.x - halfWidth + edgePadding;
            minPosition.y = camPosition.y - halfHeight + edgePadding;
        }

        for (int i = 0; i < numGoldfish; i++)
        {
            goldfishes.Add(Spawn(goldPrefab));
        }

        for (int i = 0; i < numBettaFish; i++)
        {
            bettaFishes.Add(Spawn(bettaPrefab));
        }

        // for (int i = 0; i < numFishFood; i++)
        // {
        //     fishFoodList.Add(Spawn(fishFoodPrefab));
        // }

        // Mouse mouse = Mouse.current;
        //
        // if (mouse.leftButton.wasPressedThisFrame)
        // {
        //     fishFoodList.Add(Spawn(fishFoodPrefab));
        // }
    }

    public void OnPlayerClick(InputAction.CallbackContext context)
    {
        fishFoodList.Add(Spawn(fishFoodPrefab));
    }
    
    private T Spawn<T>(T prefabToSpawn) where T : Agent
    {
        float xPos = Random.Range(minPosition.x, maxPosition.x);
        float yPos = Random.Range(minPosition.y, maxPosition.y);

        return Instantiate(prefabToSpawn, new Vector3(xPos, yPos), Quaternion.identity);
    }

    private GameObject Spawn(GameObject prefab)
    {
        float xPos = Random.Range(minPosition.x, maxPosition.x);
        float yPos = Random.Range(minPosition.y, maxPosition.y);

        return Instantiate(prefab, new Vector3(xPos, yPos), Quaternion.identity);
    }

    public BettaFish GetClosestBettaFish(Agent sourceFish)
    {
        float minDistance = float.MaxValue;
        BettaFish closestFish = null;

        foreach (BettaFish fish in bettaFishes)
        {
            float sqrDistance =
                Vector3.SqrMagnitude(sourceFish.physicsObject.Position - fish.physicsObject.Position);

            if (sqrDistance < float.Epsilon)
            {
                continue;
            }

            if (sqrDistance < minDistance)
            {
                closestFish = fish;
                minDistance = sqrDistance;
            }
        }

        return closestFish;
    }

    public Goldfish GetClosestGoldfish(Agent sourceFish)
    {
        float minDistance = float.MaxValue;
        Goldfish closestFish = null;

        foreach (Goldfish fish in goldfishes)
        {
            float sqrDistance =
                Vector3.SqrMagnitude(sourceFish.physicsObject.Position - fish.physicsObject.Position);

            if (sqrDistance < float.Epsilon)
            {
                continue;
            }

            if (sqrDistance < minDistance)
            {
                closestFish = fish;
                minDistance = sqrDistance;
            }
        }

        return closestFish;
    }

    public GameObject GetClosestFood(Agent fish)
    {
        float minDistance = float.MaxValue;
        GameObject closestFood = null;

        foreach (GameObject food in fishFoodList)
        {
            float sqrDistance =
                Vector3.SqrMagnitude(fish.physicsObject.Position - food.transform.position);

            if (sqrDistance < float.Epsilon)
            {
                continue;
            }

            if (sqrDistance < minDistance)
            {
                closestFood = food;
                minDistance = sqrDistance;
            }
        }

        return closestFood;
    }
}
