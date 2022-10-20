using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GoSceneBtn : MonoBehaviour
{
    [SerializeField] int sceneId;

    private void Start()
    {
        if(sceneId < 0 || sceneId > SceneManager.sceneCount)
        {
            sceneId = 0;
        }

        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneId);
        });
    }
}
