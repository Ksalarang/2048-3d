﻿using System;
using System.Collections.Generic;
using game_scene.controllers;
using TMPro;
using UnityEngine;
using Utils;
using Zenject;

namespace game_scene.models {
public class Cube : MonoBehaviour {
    const float MaxFontSize = 0.7f;
    const float FontSizeStep = 0.1f;
    readonly string[] suffixes = { "", "k", "m", "b" };

    [SerializeField] TMP_Text backLabel;
    [SerializeField] TMP_Text frontLabel;
    [SerializeField] TMP_Text leftLabel;
    [SerializeField] TMP_Text rightLabel;
    [SerializeField] TMP_Text topLabel;
    [SerializeField] TMP_Text bottomLabel;

    [Inject] CubeController cubeController;
    
    Rigidbody rigidBody;
    List<TMP_Text> labels;

    [HideInInspector] public CubeState state = CubeState.NotLaunched;

    public long number { get; private set; }
    
    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        addLabels();
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

    public void launch(Vector3 force) {
        state = CubeState.Launched;
        rigidBody.AddForce(force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other) {
        if (state is CubeState.Collided or CubeState.Destroyed) return;
        var otherCube = other.gameObject.GetComponent<Cube>();
        if (!otherCube) return;
        rigidBody.freezeRotation = false;
        if (otherCube.state is CubeState.Collided or CubeState.Destroyed) return;
        if (number == otherCube.number) {
            cubeController.onCubeCollision(this, otherCube);
            state = otherCube.state = CubeState.Collided;
        }
    }
}

public enum CubeState {
    NotLaunched,
    Launched,
    Collided,
    Destroyed,
}
}