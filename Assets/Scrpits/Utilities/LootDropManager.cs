using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropManager : MonoBehaviour
{
    public List<LootDrop> lootDrops = new List<LootDrop>();
    public bool multipleDrops = true;

    int roll => Random.Range(0, 100);

    IOnDeathEvents deathEvents;

    void Start()
    {
        if (gameObject.TryGetComponent<IOnDeathEvents>(out deathEvents))
        {
            deathEvents.OnDeath += SpawnLootOnDeath;
        } else 
        {
            Destroy(this);
        }
    }

    void SpawnLootOnDeath()
    {
        foreach( var loot in lootDrops )
        {
            if (loot is null)
            {
                continue;
            }

            if ( roll <= loot.spawnChancePercent )
            {
                GameObject.Instantiate(loot.prefab, transform.position, Quaternion.identity);
            }
        }
    }

    void OnDestroy()
    {
        deathEvents.OnDeath -= SpawnLootOnDeath;
    }
}

[System.Serializable]
public class LootDrop 
{
    public GameObject prefab;
    [Range(0, 100)] public float spawnChancePercent = 30;
}
 