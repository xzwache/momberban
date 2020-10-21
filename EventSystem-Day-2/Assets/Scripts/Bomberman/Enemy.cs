using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    private float _duration = 5.0f;
    
    void Start() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left);
        if (hit.collider != null) {
            float distance = Vector2.Distance((Vector2) this.transform.position,
                (Vector2) hit.collider.gameObject.transform.position);

            var seq = DOTween.Sequence();
            seq.Append(transform.DOLocalMoveX(distance, _duration / distance).OnComplete(() => {
                hit = Physics2D.Raycast(transform.position, Vector2.right);
            }));
            seq.Append(transform.DOLocalMoveX(distance, _duration / distance));

            seq.SetLoops(-1);
        }
    }
}
