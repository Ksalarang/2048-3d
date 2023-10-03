using System;
using System.Collections.Generic;
using game_scene.controllers;
using TMPro;
using UnityEngine;
using Utils.Extensions;
using utils.structs;
using Zenject;

namespace game_scene.models {
public class Cube : MonoBehaviour {
    const float MaxFontSize = 0.7f;
    const float FontSizeStep = 0.1f;
    static int count;
    readonly string[] suffixes = { "", "k", "m", "b" };

    [SerializeField] TMP_Text backLabel;
    [SerializeField] TMP_Text frontLabel;
    [SerializeField] TMP_Text leftLabel;
    [SerializeField] TMP_Text rightLabel;
    [SerializeField] TMP_Text topLabel;
    [SerializeField] TMP_Text bottomLabel;

    [Inject] CubeController cubeController;

    new MeshRenderer renderer;
    new Transform transform;
    List<TMP_Text> labels;
    bool gameOver;
    FloatRange zRange;

    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public CubeState state = CubeState.NotLaunched;

    public long number;
    
    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
        transform = base.transform;
        addLabels();
        count++;
    }

    void addLabels() {
        labels = new List<TMP_Text>();
        labels.Add(backLabel);
        labels.Add(frontLabel);
        labels.Add(leftLabel);
        labels.Add(rightLabel);
        labels.Add(topLabel);
        labels.Add(bottomLabel);
    }

    void Update() {
        var velocity = rigidBody.velocity;
        var positionZ = transform.position.z;
        if (velocity.z < 0 && positionZ > zRange.min) {
            var proximityFactor = 1 - Mathf.Abs(Mathf.Clamp01((positionZ - zRange.min) / (zRange.max - zRange.min)));
            rigidBody.velocity = new Vector3(velocity.x, velocity.y, velocity.z + proximityFactor);
        }
    }

    public void setZRange(FloatRange range) {
        zRange = range;
    }

    public void setNumber(long number) {
        this.number = number;
        updateLabels();
    }

    void updateLabels() {
        var n = number;
        var magnitude = 0;
        while (n > 1000 && magnitude < suffixes.Length - 1) {
            magnitude++;
            n /= 1000;
        }
        var sNumber = number.ToString();
        var text = sNumber.Length > 4 ? $"{n}{suffixes[magnitude]}" : sNumber;
        var fontSize = MaxFontSize - (text.Length - 1) * FontSizeStep;
        foreach (var label in labels) {
            label.text = text;
            label.fontSize = fontSize;
        }
    }

    public void setColor(Color color) => renderer.material.color = color;

    public void launch(Vector3 force) {
        state = CubeState.Launched;
        rigidBody.AddForce(force, ForceMode.Impulse);
    }

    public void reset() {
        transform.rotation = rigidBody.rotation = Quaternion.identity;
        rigidBody.velocity = rigidBody.angularVelocity = Vector3.zero;
        rigidBody.freezeRotation = true;
        state = CubeState.NotLaunched;
        gameOver = false;
    }

    public override string ToString() => $"cube{count}_{number}";

    void OnCollisionEnter(Collision other) {
        if (gameOver) return;
        var otherCube = other.gameObject.GetComponent<Cube>();
        if (!otherCube) return;
        rigidBody.freezeRotation = false;
        if (number == otherCube.number) {
            cubeController.onCubeCollision(this, otherCube);
        }
    }

    void OnTriggerStay(Collider other) {
        if (gameOver) return;
        if (other.gameObject.CompareTag("StartLine") && rigidBody.velocity.magnitude < 0.1f) {
            gameOver = true;
            cubeController.onTouchStartLine();
        }
    }
}

public enum CubeState {
    NotLaunched,
    Launched,
    Destroyed,
}
}