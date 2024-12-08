using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool pushPuzzle;
    public bool parkourPuzzle;
    public bool wordPuzzle;
    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        // door = GameObject.Find("door"); - this should be whatever the door variable is called
        pushPuzzle = false;
        parkourPuzzle = false;
        wordPuzzle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pushPuzzle && parkourPuzzle && wordPuzzle)
        {
            // in order for this to work, attach this script to the door
            door.SetActive(false);
        }
    }
}
