using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour {

    [SerializeField] private float _radius = 5f;

    public void Badadum() {
        Destroy(this);

        var colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (var collider in colliders) {
            if (collider.attachedRigidbody == null) continue;

            var direction = collider.transform.position - transform.position;
            var distance = direction.magnitude;
            var k = distance / _radius;
            
            collider.attachedRigidbody.AddForce(k * 40f * direction.normalized, ForceMode.Impulse);
        }
    }
}
