using game_scene.controllers;
using UnityEngine;
using Utils;
using Zenject;

namespace game_scene {
public class GameController : MonoBehaviour {
    [Inject] CubeProvider cubeProvider;
    [Inject] CubeController cubeController;
    
    Log log;

    void Awake() {
        log = new(GetType());
        Application.targetFrameRate = 60;
    }

    void Start() {
        log.log("start");
        cubeController.start();
    }

    public void onTouchStartLine() {
        log.log("game over");
        startNewGame();
    }

    void startNewGame() {
        cubeProvider.reset();
        cubeController.start();
    }
}
}