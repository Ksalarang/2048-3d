using System;
using UnityEngine;

namespace game_scene {
[Serializable]
public class GameSettings : MonoBehaviour {
    public CubeSettings cubeSettings;
}

[Serializable]
public class CubeSettings {
    public Vector3 initialPosition;
    public Vector3 launchForce;
    public Vector3 throwForce;
    public Vector3 maxVelocity;
    public float spawnDelay;
}
}