using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Bomberman : MonoBehaviour {

    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private LayerMask _explosionMask;
    private bool isInMove;

    [SerializeField] private GameObject _bomb;

    private Vector2 _previousBombPosition;
    
    private void Update() {
        if (isInMove) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { MoveTo(Vector2.left); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { MoveTo(Vector2.right); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { MoveTo(Vector2.up); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { MoveTo(Vector2.down); }

        if (Input.GetKeyDown(KeyCode.Space) && isInMove == false && _previousBombPosition != (Vector2)gameObject.transform.position) {
            _previousBombPosition = transform.position;
            var bombObject = Instantiate(_bomb, transform.position, Quaternion.identity);
            var bomb = bombObject.GetComponent<Bomb>();
            if (_bomb != null) {
                bomb.Detonate(_explosionMask, () => _previousBombPosition = default);
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
}