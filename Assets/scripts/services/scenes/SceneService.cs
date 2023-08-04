using UnityEngine.SceneManagement;

namespace services.scenes {
public class SceneService: Service {

    public void loadGameScene() {
        SceneManager.LoadScene("GameScene");
    }
}
}