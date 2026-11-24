using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField]private Transform RootBone;

    public void SetBone(Transform bone)
    {
        MatchAllBone(bone, RootBone);
        ApplyExplosion(RootBone, 500.0f, transform.position, 10.0f);
    }

    private void MatchAllBone(Transform bone,Transform selfbone)
    {
        foreach(Transform child in bone)
        {
            Transform childBone = selfbone.Find(child.name);
            if (childBone != null)
            {
                childBone.localPosition = child.localPosition;
                childBone.localRotation = child.localRotation;
                MatchAllBone(child, childBone);
            }
        }
    }

    private void ApplyExplosion(Transform bone,float explosionForce,Vector3 explosionPosition,float explosionRadius)
    {
        foreach (Transform child in bone)
        {

            if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
            ApplyExplosion(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
