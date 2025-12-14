using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyActions : MonoBehaviour
{
    public void CreateNewCharacter()
    {
        SceneManager.LoadScene("CharacterCreation");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
