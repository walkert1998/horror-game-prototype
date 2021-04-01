using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    public NPCAI monster;
    public int originalMonsterVisionAngle;
    public float originalMonsterVisionDistance;
    public PlayerIllumination playerIllumination;
    public PlayerAudioLevel playerAudioLevel;
    public static AIDirector instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        originalMonsterVisionAngle = monster.visionAngle;
        originalMonsterVisionDistance = monster.visionDistance;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Debug.Log(playerIllumination.lightLevel / Vector3.Distance(playerIllumination.transform.position, monster.transform.position));
    //}

    public static void SetPlayerFlashight(bool on)
    {
        if (on)
        {
            instance.monster.visionAngle = 360;
            instance.monster.visionDistance = instance.originalMonsterVisionDistance * 3;
        }
        else
        {
            instance.monster.visionAngle = instance.originalMonsterVisionAngle;
            instance.monster.visionDistance = instance.originalMonsterVisionDistance;
        }
    }

    public static void GenerateSound(Vector3 position, float volume)
    {
        instance.monster.HeardSound(position, volume);
    }
}
