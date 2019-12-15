﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Tumble : MonoBehaviour
{
    public Vector3 rotation;
    PlayerClass player;
    SaveGameManager saveMan;
    HudManager hud;
    HandleSfx sfx;
    Animator anim;
    bool isCollected = false;
    public bool isMoon = false;

    private void Awake()
    {
        hud = GameObject.FindObjectOfType<HudManager>();
        player = FindObjectOfType<PlayerClass>();
        sfx = GetComponent<HandleSfx>();
        saveMan = FindObjectOfType<SaveGameManager>();
        anim = GetComponentInParent<Animator>();
    }
    private void Start()
    {
        if (saveMan)
            if (saveMan.ReturnListOfCollectedShards().Contains(gameObject.transform.parent.gameObject.name + SceneManager.GetActiveScene().name)) { isCollected = true; }
    }
    // Update is called once per frame
    void Update()
    {
        if (isCollected) StartCoroutine(Collected());
        transform.Rotate(rotation * Time.deltaTime);
    }
    IEnumerator Collected()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<SphereCollider>().enabled = false;
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider c)//Pool them later
    {
        if (c.gameObject.layer == 14 && anim.GetCurrentAnimatorStateInfo(0).IsName("Meteor_Spin_Animation"))
        {
            if (!isMoon)
            {
                sfx.PlayOneShotByName("Collect");
                hud.ShardsUp();
                player.SetShards(1);
                isCollected = true;
                if (saveMan)
                {
                    saveMan.AddToListOfCollectedShards(gameObject.transform.parent.gameObject.name + SceneManager.GetActiveScene().name);
                    saveMan.SaveCollectedShardsID();
                }
            }
            else
            {
                sfx.PlayOneShotByName("Collect");
                hud.MoonsUp();
            }
            isCollected = true;

            if (saveMan)
            {
                saveMan.AddToListOfCollectedShards(gameObject.transform.parent.gameObject.name + SceneManager.GetActiveScene().name);
                saveMan.SaveCollectedShardsID();
            }
        }
    }
}
