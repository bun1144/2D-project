
using UnityEngine;
public class DontDestroyBGM : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<DontDestroyBGM>().Length > 1)
        { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
    }
}
