using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSpawner : MonoBehaviour {

    private static int int_framesUntilSpawn = 0;
    private static bool bl_isCallingWave = false;

    public struct waveSpawner {
        public GameObject spawner;
        public int int_startDelayFrames; //the number of FixedUpdate calls before the spawner will be called
        public int int_endDelayFrames; //the number of FixedUpdate calls after the wave (spawner) is instantiated before the next is called
        public float fl_callFrequency; //used to determine the relative probability of the particular wave (spawner) being called
    }

    public waveSpawner[] spawnerSet; //contains all of the spawners which will be used in the current level
    private waveSpawner nextSpawner; //dictates which wave (spawner) is to be called

    private float[] fl_spawnIndices;
    private float maxIndex; //the highest value
    

    IEnumerator spawnWave(waveSpawner nextWave) {

        int int_framesUntilCall = nextWave.int_startDelayFrames;
        while(int_framesUntilCall > 0) {
            int_framesUntilCall--;
            yield return new WaitForFixedUpdate();
        }
        Instantiate(nextWave.spawner, Vector3.zero, Quaternion.identity);
        int_framesUntilSpawn = nextWave.int_endDelayFrames;
        bl_isCallingWave = false;
        yield break;
    }

    /* getSpawnIndices() returns the sequenced spawn threshold indices for each
       element in spawnerSet based on the call frequency of each element. For a
       spawner to be called, some generated index must be less than or equal to
       the spawner's threshold index, but greater than that of the previous
       spawner. */
    float[] getSpawnIndices() {

        int spawnerCount = spawnerSet.Length;
        float[] spawnIndexSet = new float[spawnerCount];
        float frequencySum = 0.0f;

        for (int spawnerID = 0; spawnerID < spawnerSet.Length; spawnerID++) {
            frequencySum += spawnerSet[spawnerID].fl_callFrequency;
            spawnIndexSet[spawnerID] = frequencySum;
        }

        return spawnIndexSet;
    }

    /* getSpawnerFromIndex is used to determine which wave (spawner) to call
       based on a previously-generated index. */
    int getSpawnerFromIndex(float generatedIndex) {

        int spawnerCount = spawnerSet.Length;
        int spawnerID = Mathf.RoundToInt(Mathf.Ceil(spawnerCount / 2.0f));
        int stepDistance = Mathf.RoundToInt(Mathf.Ceil((spawnerID + 1) / 2.0f));

        float currentIndex = fl_spawnIndices[spawnerID];
        float prevIndex;
        if(spawnerID == 0) {
            prevIndex = 0.0f;
        }
        else {
            prevIndex = fl_spawnIndices[spawnerID - 1];
        }

        //perform a binary search here to determine which spawner to call
        do {
            if(prevIndex >= generatedIndex) {
                //step left and adjust values accordingly
                if(stepDistance >= spawnerID) {
                    spawnerID = 0;
                }
                else {
                    spawnerID -= stepDistance;
                }
            }
            else if(currentIndex < generatedIndex) {
                //step right and adjust values accordingly
                if (stepDistance >= spawnerCount - spawnerID)
                {
                    spawnerID = spawnerCount;
                }
                else
                {
                    spawnerID += stepDistance;
                }
            }

            //adjust stepping distance
            stepDistance = Mathf.RoundToInt(Mathf.Ceil(stepDistance / 2.0f));

            //Adjust current and previous index values for further comparison
            currentIndex = fl_spawnIndices[spawnerID];
            if (spawnerID == 0) {
                prevIndex = 0.0f;
            }
            else {
                prevIndex = fl_spawnIndices[spawnerID - 1];
            }


        } while (prevIndex >= generatedIndex || currentIndex < generatedIndex);

        return spawnerID;
    }

	// Use this for initialization
	void Start () {

        fl_spawnIndices = getSpawnIndices();
        maxIndex = fl_spawnIndices[spawnerSet.Length];

        int_framesUntilSpawn = 60; //gives a 0.5-second window upon starting a level before spawning anything
        bl_isCallingWave = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (int_framesUntilSpawn <= 0 && !bl_isCallingWave) {
            bl_isCallingWave = true;
            nextSpawner = spawnerSet[getSpawnerFromIndex(Random.Range(0.0f, maxIndex))];
            StartCoroutine(spawnWave(nextSpawner));
        }

        int_framesUntilSpawn--;
	}
}
