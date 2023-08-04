using game_scene.controllers;
using UnityEngine;
using Utils;
using Zenject;

namespace game_scene.input {
public class InputController : MonoBehaviour {
    [Inject] new Camera camera;
    [Inject] CubeController cubeController;

    Log log;
    readonly bool isMobile = Application.isMobilePlatform;
    float cameraZ;

    TouchPhase phase;
    Vector3 currentPosition;
    // mouse input
    bool touching;
    bool touchedBefore;
    Vector3 prevPosition;

    void Awake() {
        log = new(GetType());
        cameraZ = camera.transform.position.z;
    }

    Vector3 startPosition;
    float deltaX;
    
    void Update() {
        #region determine touch phase
        if (isMobile) {
            if (Input.touchCount == 0) return;
            var touch = Input.GetTouch(0);
            phase = touch.phase;
            currentPosition = camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraZ));
        } else {
            touching = Input.GetMouseButton(0);
            currentPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZ));
            if (!touchedBefore && touching) {
                phase = TouchPhase.Began;
                touchedBefore = true;
                prevPosition = currentPosition;
            } else if (touchedBefore && touching) {
                if (prevPosition.x - currentPosition.x != 0
                    || prevPosition.y - currentPosition.y != 0
                    || prevPosition.z - currentPosition.z != 0) {
                    phase = TouchPhase.Moved;
                    prevPosition = currentPosition;
                } else {
                    phase = TouchPhase.Stationary;
                }
            } else if (touchedBefore && !touching) {
                phase = TouchPhase.Ended;
                touchedBefore = false;
            } else return;
        }
        #endregion

        switch (phase) {
            case TouchPhase.Began:
                startPosition = currentPosition;
                break;
            case TouchPhase.Moved:
                deltaX = startPosition.x - currentPosition.x;
                cubeController.onHorizontalShift(deltaX);
                startPosition = currentPosition;
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                cubeController.onCubeRelease();
                break;
        }
    }
}
}