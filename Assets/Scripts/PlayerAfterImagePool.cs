using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour {
    public GameObject afterImagePrefab;
    private readonly Queue<GameObject> availableObjects = new Queue<GameObject>();
    private const int MaxAfterImages = 10;

    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            GrowPool();
        }
        else {
            Destroy(this);
        }
    }

    private void GrowPool() {
        for (int i = 0; i < MaxAfterImages; i++) {
            GameObject instanceToAdd = Instantiate(afterImagePrefab, transform, true);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance) {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool() {
        if (availableObjects.Count == 0) {
            GrowPool();
        }

        GameObject instance = availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}