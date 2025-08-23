using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Boss"; // ตั้งชื่อซีนปลายทาง

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ให้ Player มี Tag = Player
        {
            Debug.Log("Player reached exit → Loading " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
