using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameConstants gameConstants;
    float groundDistance = 5.0f;
    void spawnFromPooler(ObjectType i){
        // static method access
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);
        if (item != null) {

            //item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //item.transform.position = new Vector3(Random.Range(-4.5f, 4.5f), groundDistance + item.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            //set position, and other necessary states
            item.transform.position = new Vector3(Random.Range(gameConstants.enemySpawnPointCenterX - 2.0f, gameConstants.enemySpawnPointCenterX + 2.0f), gameConstants.enemySpawnPointCenterY+1, 0);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            
            item.SetActive(true);
        }
        else {
            Debug.Log("not enough items in the pool.");
        }
    }

    void spawnNewEnemy() {
        ObjectType i;
        i = ObjectType.goombaEnemy;
        GameObject item;
        item = ObjectPooler.SharedInstance.GetPooledObject(i);
        if (item != null) {
            spawnNewGoomba();
        }
        else {
            i = ObjectType.koopaEnemy;
            item = ObjectPooler.SharedInstance.GetPooledObject(i);
            if (item != null) {
                spawnNewKoopa();
            }
            else {
                Debug.Log("maximum enemies present!");
            }
        }
    }

    void spawnNewGoomba() {
        spawnFromPooler(ObjectType.goombaEnemy);
    }

    void spawnNewKoopa() {
        spawnFromPooler(ObjectType.koopaEnemy);
    }

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to brick coin break
        GameManager.OnBrickCoinBreak += spawnNewEnemy;

        GameManager.OnGoombaDeath += spawnNewGoomba;
        GameManager.OnKoopaDeath += spawnNewKoopa;
        // spawn two goombaEnemy
        for (int j = 0; j < 2; j++)
            spawnFromPooler(ObjectType.goombaEnemy);
        //spawn one koopaEnemy
        for (int j = 0; j < 1; j++)
            spawnFromPooler(ObjectType.koopaEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
