using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Mover mover;

    private void Awake() {
        mover = GetComponent<Mover>();
    }

    private void Update() {
        if (Input.GetMouseButton(1)) {
            MoveToCursor();
        }
    }

    public void MoveToCursor() {
        Ray lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(lastRay, out hit);

        if (hasHit) {
            mover.MoveTo(hit.point);
        }
    }

}
