using UnityEngine;
using Utils;

namespace game_scene.input {
public class InputListener : MonoBehaviour {
    static Log log = new(typeof(InputListener), false);

    public virtual void onTouchDown(Vector3 position) {
        log.log($"onTouchDown: {position}", name);
    }

    public virtual void onTouchUp() {
        log.log("onTouchUp", name);
    }

    public virtual void onClick() {
        log.log("onClick", name);
    }

    public virtual void onTouchDrag(Vector3 newPosition) {
        log.log($"onTouchDrag: {newPosition}", name);
    }
}
}