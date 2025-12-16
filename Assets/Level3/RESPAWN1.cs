using UnityEngine;
using System.Collections;

public class RESPAWN1 : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject flashEffect;
    public float slowMotionFactor = 0.2f;
    public float slowMotionDuration = 0.3f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        if (!collision.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(TeleportSequence(collision.transform));
    }

    private IEnumerator TeleportSequence(Transform player)
    {
        Instantiate(flashEffect, player.position, Quaternion.identity);

        Time.timeScale = slowMotionFactor;
        yield return new WaitForSecondsRealtime(slowMotionDuration);

        player.position = respawnPoint.position;

        Time.timeScale = 1f;

        triggered = false;
    }
}
