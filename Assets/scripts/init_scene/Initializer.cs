using services.scenes;
using UnityEngine;
using Zenject;

namespace init_scene {
public class Initializer: MonoBehaviour {
    [Inject] SceneService sceneService;

    void Start() {
        sceneService.loadGameScene();
    }
}
}