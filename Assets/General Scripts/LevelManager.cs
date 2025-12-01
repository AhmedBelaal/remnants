using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject CurrentCheckpoint;
    public Animator anim;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        anim = FindObjectOfType<PlayerControl>().GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RespawnPlayer()
    {
        if (anim != null)
        {
            anim.SetTrigger("Death");
        }
        FindObjectOfType<PlayerControl>().transform.position = CurrentCheckpoint.transform.position;
        anim.Play("Idle_King");
    }
}