using UnityEngine.SceneManagement;
using UnityEngine;

public class Objective4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            ObjectivesComplete.occurrence.GetObjectivesDone(true, true, true, true);
            SceneManager.LoadScene("MainMenu");
            Destroy(gameObject, 1f);
        }
    }
}
