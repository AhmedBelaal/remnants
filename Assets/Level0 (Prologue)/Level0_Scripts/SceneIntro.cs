using System.Collections;
using UnityEngine;

public class SceneIntro : MonoBehaviour
{
    public PlayerControl playerControl; 
    public float standUpDuration = 4.0f; 

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        if (playerControl != null)
        {
            // 1. Lock Movement
            playerControl.canMove = false;
            playerControl.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            Animator anim = playerControl.GetComponent<Animator>();
            if(anim != null)
            {
                anim.SetBool("Grounded", true); 
                anim.SetFloat("Height", 0f);
                anim.SetFloat("Speed", 0f);
                
                yield return new WaitForEndOfFrame(); 

                // 2. Play StandUp
                anim.SetTrigger("StandUp");
            }

            // 3. Wait for animation
            yield return new WaitForSeconds(standUpDuration);

            // 4. Unlock
            playerControl.canMove = true;
        }
    }
}