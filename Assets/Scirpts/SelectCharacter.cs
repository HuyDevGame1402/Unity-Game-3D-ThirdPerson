using UnityEngine.SceneManagement;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public GameObject selectCharacter;
    public GameObject mainMenu;
    public void OnCharacter1()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnCharacter2()
    {
        SceneManager.LoadScene("GameScene1");
    }
    public void OnCharacter3()
    {
        SceneManager.LoadScene("GameScene2");
    }
    public void OnBackButton()
    {
        mainMenu.SetActive(true);
        selectCharacter.SetActive(false);
    }
}
