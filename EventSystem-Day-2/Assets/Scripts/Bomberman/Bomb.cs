using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Bomb : MonoBehaviour {

    private LayerMask _explosionMask;
    private SpriteRenderer _renderer;
    [SerializeField] private float _delay;

    private GameObject _explosionImage;
    
    public void Detonate(LayerMask mask, Action callback = null) {
        _explosionMask = mask;
        
        var sequence = DOTween.Sequence();
        sequence.Append(_renderer.DOFade(0.0f, 0.2f));
        sequence.SetDelay(_delay);
        
        sequence.OnComplete(() => {
            MakeBoom();
            AnimateExplosion();
            Destroy(this.gameObject);
            
            callback?.Invoke();
        });
    }

    private void MakeBoom() {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 3.0f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("NonDistructable") == false) {
            var hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 3.0f, _explosionMask);
            hitsLeft.ToList().ForEach(h => Eliminate(hit));
            
            //Compute distance and animate part of explosion
            //AnimateExplosion();
        }
        hit = Physics2D.Raycast(transform.position, Vector2.right, 3.0f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("NonDistructable") == false) {
            var hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right, 3.0f, _explosionMask);
            hitsRight.ToList().ForEach(h => Eliminate(hit));
        }
        hit = Physics2D.Raycast(transform.position, Vector2.up, 3.0f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("NonDistructable") == false) {
            var hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up, 3.0f, _explosionMask);
            hitsUp.ToList().ForEach(h => Eliminate(hit));
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("NonDistructable") == false) {
            var hitsDown = Physics2D.RaycastAll(transform.position, Vector2.down, 3.0f, _explosionMask);
            hitsDown.ToList().ForEach(h => Eliminate(hit));
        }

        void Eliminate(RaycastHit2D h) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            Destroy(hit.collider.gameObject);
        }
    }

    private void AnimateExplosion(int count, Vector2 direction) {
        for (int i = 0; i < count; i++) {
            GameObject obj = Instantiate(_explosionImage, direction * (i + 1), Quaternion.identity);
            obj.GetComponent<Image>().DOFade(0.0f, 1.0f);
        }
    }

    private void Reset() {
        _renderer = GetComponent<SpriteRenderer>();
    }
}
