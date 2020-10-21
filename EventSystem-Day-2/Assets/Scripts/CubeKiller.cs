using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeKiller : MonoBehaviour {
    
    [SerializeField] private Camera _camera;
    
    void Update() {
        if (Input.GetMouseButtonDown(0) == false) return;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit) == false) {
            return;
        }

        var rig = hit.collider.attachedRigidbody;
        if (rig != null) {
            var direction = (hit.point - transform.position).normalized * 10;
            rig.AddForceAtPosition(direction, hit.point, ForceMode.Impulse);

            var tnt = rig.gameObject.GetComponent<TNT>();
            if (tnt != null) tnt.Badadum();
        }
    }
}
