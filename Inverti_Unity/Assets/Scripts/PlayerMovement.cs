using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Transform PlayerTransform;
    private static Vector3 playerTargetEulerAngles; //by having target rotation explicitly valued in script,
    //problems arising from auto-conversion between equivalent EulerAngles can be avoided.

    private float fl_playerSpeed;
    public float fl_speedMultiplier = 1.0f; //Player's horizontal speed varies by level - this adjusts it
    private const float fl_LEAN_MULTIPLIER = 10f; //adjusts how much the player leans into a turn

    private int int_flipFrames = 0;

    private const int int_FLIP_DURATION = 12; //The number of FixedUpdates over which a player's state swap will occur
    private static bool bl_isFlipping = false;
    private static bool bl_isPositiveState = true;
    private static bool bl_isFlipAxisInUse = false;


    IEnumerator flipPlayer(float fl_targetzRotation) {

        for (int_flipFrames = 0; int_flipFrames < int_FLIP_DURATION; int_flipFrames++) {

            playerTargetEulerAngles = new Vector3(playerTargetEulerAngles.x, playerTargetEulerAngles.y, playerTargetEulerAngles.z + 180.0f / int_FLIP_DURATION);
            yield return new WaitForFixedUpdate();
        }
        playerTargetEulerAngles = new Vector3(90, playerTargetEulerAngles.y, fl_targetzRotation);
        int_flipFrames = 0;
        bl_isFlipping = false;
        yield break; //ends the coroutine
    }

	// Use this for initialization
	void Start () {
        fl_playerSpeed = 0;

        PlayerTransform = transform; //gameObject.GetComponent<Transform>();
        playerTargetEulerAngles = new Vector3(90, 0, 0);
        PlayerTransform.eulerAngles = playerTargetEulerAngles;

        bl_isFlipping = false;
        bl_isPositiveState = true;
        bl_isFlipAxisInUse = false;
        int_flipFrames = 0;
    }

// Update is called once per frame
void FixedUpdate () {

        /*This code section calls the coroutine to flip to the opposite state
         * whenever the flip axis key is hit. It does not allow for
         * calls to the flip function while a flip is already occuring*/
        if (Input.GetAxisRaw("Flip") != 0)
        {
            if (bl_isFlipAxisInUse == false)
            {
                if (!bl_isFlipping)
                {

                    if (gameObject.tag == "Player")
                    {
                        bl_isPositiveState = !bl_isPositiveState;
                        bl_isFlipping = true;
                    }

                    if (!bl_isPositiveState) //Note: bl_isPositiveState has already been flipped to target by this point
                    {
                        StartCoroutine(flipPlayer(180));
                        Debug.Log("Switching to negative state");
                    }
                    else
                    {
                        StartCoroutine(flipPlayer(0));
                        Debug.Log("Switching to positive state");
                    }
                }

                bl_isFlipAxisInUse = true;
            }
        }
        if (Input.GetAxisRaw("Flip") == 0)
        {
            bl_isFlipAxisInUse = false;
        }

        //Applies player movement, as well as lean into the direction of movement
        fl_playerSpeed = Input.GetAxis("Horizontal");
        PlayerTransform.Translate(new Vector3(fl_playerSpeed * fl_speedMultiplier, 0, 0), Space.World);
        playerTargetEulerAngles = new Vector3(90, fl_playerSpeed * fl_LEAN_MULTIPLIER, playerTargetEulerAngles.z);
        
        //resets the player's world rotation to 0
        PlayerTransform.eulerAngles = new Vector3(0, 0, 0);

        //Finally, performs rotation around each global axis separately, in order (x, z, y)
        PlayerTransform.RotateAround(PlayerTransform.position, new Vector3(1, 0, 0), playerTargetEulerAngles.x);
        PlayerTransform.RotateAround(PlayerTransform.position, new Vector3(0, 0, 1), playerTargetEulerAngles.z);
        PlayerTransform.RotateAround(PlayerTransform.position, new Vector3(0, 1, 0), playerTargetEulerAngles.y);

    }
}
