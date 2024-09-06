using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField]
    private GameObject afterImageprefab;

    private Queue<GameObject> availableobjects=new Queue<GameObject>();

    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
         Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for(int i=0;i<10;i++)
        {
            var instanceToAdd=Instantiate(afterImageprefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableobjects.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if(availableobjects.Count == 0)
        {
            GrowPool();
        }

        var instance=availableobjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }


}
