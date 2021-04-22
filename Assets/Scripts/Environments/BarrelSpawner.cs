using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> barrelPrefabs;
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private GameObject particles;

    private GameObject spawnedObject;

    public ExplosiveBarrel SpawnBarrel()
    {
        var randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        var randomPrefab = barrelPrefabs[Random.Range(0, barrelPrefabs.Count)];

        if(spawnedObject != null)
        {
            Destroy(spawnedObject);
        }

        spawnedObject = Instantiate(randomPrefab, randomSpawn.position, Quaternion.identity, transform.parent);

        var explosibeBarrelComp = spawnedObject.GetComponent<ExplosiveBarrel>();
        explosibeBarrelComp.particles[0] = Instantiate(particles).GetComponent<ParticleSystem>();
        explosibeBarrelComp.particles[1] = Instantiate(particles).GetComponent<ParticleSystem>();
        explosibeBarrelComp.particles[2] = Instantiate(particles).GetComponent<ParticleSystem>();

        return explosibeBarrelComp;
    }
}
