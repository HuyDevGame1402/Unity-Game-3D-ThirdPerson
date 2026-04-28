using UnityEngine;

public class Objectives2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectivesComplete.occurrence.GetObjectivesDone(true, true, true, false);
            Destroy(gameObject, 1f);
        }
    }
}
