using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    public GameObject ragdollPrefab;
    [SerializeField] private Transform bone;

    private HealthSystem healthSystem;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.UnitDead += HSUnitDead;
    }

    void Update()
    {

    }

    private void HSUnitDead(object sender, EventArgs e)
    {
        GameObject ragdoll = Instantiate(ragdollPrefab,transform.position,transform.rotation);
        UnitRagdoll ragdollScript = ragdoll.GetComponent<UnitRagdoll>();
        ragdollScript.SetBone(bone);
    }



}
