using System.Collections;
using UnityEngine;
using Game.Play.Level;

public class SwitchToLoading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        GameScene.Load(SceneIndex.Game);
    }
}
