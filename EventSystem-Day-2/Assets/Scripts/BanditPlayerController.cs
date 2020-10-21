using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BanditPlayerController : MonoBehaviour {

    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private LayerMask _explosionMask;
    private bool isInMove;

    private void Update() {
        if (isInMove) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { MoveTo(Vector2.left); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { MoveTo(Vector2.right); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { MoveTo(Vector2.up); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { MoveTo(Vector2.down); }

        if (Input.GetMouseButtonDown(0)) {
            var obj = RaycastFromCamera();
            if (obj != null && obj.CompareTag("Explosive")) {
                Destroy(obj);

                var colliders = Physics2D.OverlapCircleAll(obj.transform.position, 1f, _explosionMask);
                foreach (var col in colliders) { Destroy(col.gameObject); }
            }
        }
    }

    private void MoveTo(Vector2 direction) {
        if (Raycast(direction)) return;

        isInMove = true;
        var position = (Vector2) transform.position + direction;
        transform.DOMove(position, 0.5f).OnComplete(() => isInMove = false);
    }

    private bool Raycast(Vector2 direction) {
        var hit = Physics2D.Raycast(transform.position, direction, 1f, _raycastMask);
        return hit.collider != null;
    }

    private GameObject RaycastFromCamera() {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f);
        return hit.collider ? hit.collider.gameObject : null;
    }
}
