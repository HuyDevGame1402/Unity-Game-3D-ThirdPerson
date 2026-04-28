using TMPro;
using UnityEngine;

public class ObjectivesComplete : MonoBehaviour
{
    [Header("Objectives to Complete")]
    public TextMeshProUGUI objective1;
    public TextMeshProUGUI objective2;
    public TextMeshProUGUI objective3;
    public TextMeshProUGUI objective4;

    public static ObjectivesComplete occurrence;

    private void Awake()
    {
        occurrence = this;
    }

    public void GetObjectivesDone(bool obj1, bool obj2, bool obj3, bool obj4)
    {
        if (obj1 == true)
        {
            objective1.text = "Objective 1: Completed";
            objective1.color = Color.green;
        }
        else
        {
            objective1.text = "Objective 1: Find The Rifle";
            objective1.color = Color.white;
        }
        if (obj2 == true)
        {
            objective1.text = "Objective 2: Completed";
            objective1.color = Color.green;
        }
        else
        {
            objective1.text = "Objective 2: Find Villagers";
            objective1.color = Color.white;
        }
        if (obj3 == true)
        {
            objective1.text = "Objective 3: Completed";
            objective1.color = Color.green;
        }
        else
        {
            objective1.text = "Objective 3: Find Vehicle";
            objective1.color = Color.white;
        }
        if (obj4 == true)
        {
            objective1.text = "Objective 4: Completed";
            objective1.color = Color.green;
        }
        else
        {
            objective1.text = "Objective 4: Take all of the villagers into vehicle";
            objective1.color = Color.white;
        }
    }
}
