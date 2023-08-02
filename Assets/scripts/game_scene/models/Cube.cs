using System;
using UnityEngine;
using Utils;

namespace game_scene.models {
public class Cube : MonoBehaviour {
    Rigidbody rigidBody;

    public CubeState state = CubeState.NotLaunched;

    public int number { get; private set; }
    
    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    public void setNumber(int number) {
        this.number = number;
    }

    void Update() {
    }

    public void launch(Vector3 force) {
        state = CubeState.Launched;
        rigidBody.AddForce(force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Plane")) return;
        state = CubeState.Collided;
        rigidBody.freezeRotation = false;
    }
}

public enum CubeState {
    NotLaunched,
    Launched,
    Collided,
}
}