using System;
using game_scene.models;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace game_scene.controllers {
public class CubeController : MonoBehaviour {
    [Inject] CubeProvider cubeProvider;
    [Inject] CubeSettings settings;
    [Inject(Id = ObjectId.Floor)] GameObject floor;

    Log log;
    readonly int[] numbers = { 2, 4, 8, 16 };
    Cube currentCube;
    float minX;
    float maxX;

    void Awake() {
        log = new(GetType());
        var cubeX = 0.5f;
        var floorHalfWidth = floor.transform.localScale.x * 10 / 2;
        maxX = floorHalfWidth - cubeX;
        minX = -maxX;
    }

    void Start() {
        createCube();
    }

    int power = 1;
    
    void createCube() {
        var number = (long) Mathf.Pow(2, power++);
        var cube = cubeProvider.getCube(number);
        cube.transform.position = settings.initialPosition;
        currentCube = cube;
    }

    public void onHorizontalShift(float deltaX) {
        if (currentCube is null) return;
        var cubeTransform = currentCube.transform;
        cubeTransform.Translate(deltaX, 0, 0);
        var x = cubeTransform.position.x;
        if (x < minX) {
            cubeTransform.setX(minX);
        } else if (x > maxX) {
            cubeTransform.setX(maxX);
        }
    }
    
    public void onCubeRelease() {
        if (currentCube is null) return;
        currentCube.launch(settings.launchForce);
        StartCoroutine(Coroutines.delayAction(settings.spawnDelay, () => {
            createCube();
        }));
        currentCube = null;
    }
}
}