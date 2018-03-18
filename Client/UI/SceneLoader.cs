using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Client.Scripts.UI
{
    public class SceneLoader : MonoBehaviour {

        private bool _loadScene;
        private Text _loadingText;

        private void Awake()
        {
            _loadingText = GetComponentInChildren<Text>();
        }

        private void Update() {
            // If the new scene has started loading...
            if (_loadScene)
            {
                // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
                _loadingText.color = new Color(_loadingText.color.r, _loadingText.color.g, _loadingText.color.b, Mathf.PingPong(Time.time, 0.5f));
            }
        }

        public void LoadScene(string sceneName, float minDelay)
        {
            gameObject.SetActive(true);
            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            _loadScene = true;

            // ...change the instruction text to read "Loading..."
            _loadingText.text = "Loading " + sceneName + "...";

            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene(sceneName, minDelay));

        }

        // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
        private static IEnumerator LoadNewScene(string sceneName, float minDelay)
        {
            yield return new WaitForSeconds(minDelay);

            // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
            var async = SceneManager.LoadSceneAsync(sceneName);

            // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
            while (!async.isDone) {
                yield return null;
            }
        }
    }
}