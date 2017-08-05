using System.Collections;
using Launcher;
using UnityEngine;

namespace MP.TSGame.Players
{
    public class DestroyMouseClick : MonoBehaviour
    {
        private void Start()
        {
            var anim = GetComponent<Animator>();
            anim.Play("Take 001");
            float delay = GameControl.PlayerState.AutoClickerActive ? 0.5f : 1.666f / 2;
            StartCoroutine(DestroyAnimation(delay));
        }

        private IEnumerator DestroyAnimation(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
