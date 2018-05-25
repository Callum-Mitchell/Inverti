using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Desired functionality for this class:
 * 1. Spawn enemy waves as a whole
 * 2. Spawn individual elements of a wave at manually-set intervals
 * 3. Be able to add a horizontal position offset to the wave (based on "columns" of walls)
 * 4. Be able to add the same offset (random or controlled) to each individual wave element if desired
 * 5. Account for the wrap-around feature in both of these offsets
 * 6. Be able to randomly select between possible elements for a stage

 */
public class WaveSpawner : MonoBehaviour {

    //WaveElement can be used as the entire wave by making WaveObject contain the entire wave
    [System.Serializable]
    public struct WaveElement {
        public GameObject WaveObject;
        public Vector3 spawnPosition;
        public Vector3 spawnRotationEulerAngles;
        public int postSpawnDelay; //dictates how many FixedUpdate calls must pass after the element spawns before another can follow
        public int[] possibleSpawnIDs; //The value of each spawn ID contained dictates when the element can appear, i.e. 1 = first object, 2 = second object
    }

    public WaveElement[] EnemyWaveSelection; //contains all the potenial elements the wave can spawn
    private WaveElement[] EnemyWave; //actual generated wave as a whole, with elements obtained from EnemyWaveSelection
    private int PossibleWaveElementCount; //total number of possible elements to choose from
    public int WaveLength; //number of elements which will be spawned before the wave ends

    private int currentSpawnID;
    private bool bl_callingEnabled;

    IEnumerator spawnElement(WaveElement nextElement)
    {
        int int_framesToNextCall = nextElement.postSpawnDelay;

        Instantiate(nextElement.WaveObject, nextElement.spawnPosition, Quaternion.Euler(nextElement.spawnRotationEulerAngles));

        while (int_framesToNextCall > 0) {
            int_framesToNextCall--;
            yield return new WaitForFixedUpdate();
        }

        if(currentSpawnID > WaveLength){

        }

        currentSpawnID--;
        bl_callingEnabled = true;

        yield break;
    }

    WaveElement SelectElement() {

        /* TODO: add faster implementation (definitely possible; overall goal is to take 
         * the int arrays of possible spawnIDs for each wave element and transform them into
         * arrays/lists of possible wave elements that can be called for each spawnID. It
         * should be possible to do this in linear time. Alternatively, just have the desired
         * information be what gets entered into the inspector during wave designing.
         */
        int selectedID;
        int possibleIDCount = 0;
        List<int> possibleIDs = new List<int>();

        for (int scanID = 0; scanID < EnemyWaveSelection.Length; scanID++) {

            bool containsID = false;
            WaveElement scannedElement = EnemyWaveSelection[scanID];
            int possibleOccurrenceCount = scannedElement.possibleSpawnIDs.Length;

            for (int possibleSpawnIDsIndex = 0; possibleSpawnIDsIndex < possibleOccurrenceCount; possibleSpawnIDsIndex++) {

                if (scannedElement.possibleSpawnIDs[possibleSpawnIDsIndex] == currentSpawnID) {
                    containsID = true;
                }
            }

            if(containsID) {
                possibleIDs.Add(scanID);
            }
        }


        selectedID = possibleIDs[Random.Range(0, possibleIDs.Count - 1)];
        return EnemyWaveSelection[selectedID];
    }

    // Use this for initialization
    void Start () {
		
        currentSpawnID = 0;
        bl_callingEnabled = false;

        //TODO: generate enemy wave 
        while(currentSpawnID < WaveLength) {


        }

        //EnemyWave = EnemyWaveSelection; //PLACEHOLDER

        currentSpawnID = 0;
        bl_callingEnabled = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (bl_callingEnabled)
        {
            bl_callingEnabled = false;
            StartCoroutine(spawnElement(EnemyWave[currentSpawnID]));
        }
    }
}
