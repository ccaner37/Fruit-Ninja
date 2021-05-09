using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] fruits;
    public Transform bombprefab;
    public Transform explosionEffect;
    public float fruitSpeed;
    public Transform spawnedBomb;

    void Start()
    {
        InvokeRepeating("LaunchFruits", 2.0f, 1.3f);
    }

    void LaunchFruits()
    {

        var fruit = Instantiate(fruits[Random.Range(0, fruits.Length)], new Vector3(Random.Range(-5, 5), transform.position.y, transform.position.z), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
        Rigidbody fruitrb = fruit.GetComponent<Rigidbody>();
        fruitrb.AddForce(Vector3.up * 1100);
    }
    public void LaunchBomb()
    {
        BombGenerator();
        if (Random.Range(1,2) == 2)
        {
            RandomBomb();
        }
    }

    IEnumerator RandomBomb()
    {
        yield return new WaitForSeconds(Random.Range(1,5));
        BombGenerator();
    }

    void BombGenerator()
    {
        var bomb = Instantiate(bombprefab, new Vector3(Random.Range(-5, 5), transform.position.y, transform.position.z), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
        spawnedBomb = bomb;
        Rigidbody bombrb = bomb.GetComponent<Rigidbody>();
        bombrb.AddForce(Vector3.up * 1100);
        Destroy(bomb, 10);
    }

    public void EffectSpawner()
    {
        Instantiate(explosionEffect, spawnedBomb.position, Quaternion.identity);
    }

    public void FasterFasterFaster()
    {
        CancelInvoke();
        fruitSpeed = 20 / (GameManager.Instance.score / 10);
        

        float OriginalBaseCost = 0.1f;

        fruitSpeed = Mathf.Lerp(OriginalBaseCost * 20f, OriginalBaseCost, (GameManager.Instance.score + 1) / 400f);

        InvokeRepeating("LaunchFruits", fruitSpeed + 0.1f, fruitSpeed);


    }
}
