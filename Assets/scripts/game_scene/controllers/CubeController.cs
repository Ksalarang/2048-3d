using game_scene.models;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace game_scene.controllers {
public class CubeController : MonoBehaviour {
    [Inject] CubeProvider cubeProvider;
    [Inject] GameController gameController;
    [Inject] CubeSettings settings;
    [Inject(Id = ObjectId.Floor)] GameObject floor;

    Log log;
    readonly int[] numbers = { 2, 4, 8, 16, 32, 64 };
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

    public void start() {
        createCube();
    }

    void createCube() {
        var number = nextNumber();
        var cube = cubeProvider.getCube(number);
        cube.transform.position = settings.initialPosition;
        currentCube = cube;
    }

    // int power;
    
    long nextNumber() {
        // return (long) Mathf.Pow(2, ++power);
        var max = RandomUtils.nextBool() ? numbers.Length / 2 : numbers.Length;
        var i = RandomUtils.nextInt(0, max);
        return numbers[i];
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

    public void onCubeCollision(Cube cube1, Cube cube2) {
        var midPoint = cube1.transform.position.midPoint(cube2.transform.position);
        var number = cube1.number * 2;
        var force = cube1.rigidBody.velocity + cube2.rigidBody.velocity;
        var torque = cube1.rigidBody.angularVelocity + cube2.rigidBody.angularVelocity;
        cubeProvider.releaseCube(cube1);
        cubeProvider.releaseCube(cube2);
        var cube = cubeProvider.getCube(number);
        cube.transform.position = midPoint;
        force += settings.throwForce;
        force = MathUtils.clamp(force, -settings.maxVelocity, settings.maxVelocity);
        cube.rigidBody.AddForce(force, ForceMode.VelocityChange);
        cube.rigidBody.AddTorque(torque);
    }

    public void onTouchStartLine() {
        gameController.onGameOver();
    }
}
}