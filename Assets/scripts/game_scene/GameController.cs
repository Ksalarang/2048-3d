using UnityEngine;
using Utils;
using Zenject;

namespace game_scene {
public class GameController : MonoBehaviour {
    Log log;

    void Awake() {
        log = new(GetType());
        Application.targetFrameRate = 60;
    }

    void Start() {
        log.log("start");
    }
}
}