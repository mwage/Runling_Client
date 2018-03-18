using System.Collections;
using UnityEngine;

namespace Client.Scripts.PlayerInput
{
    public class DestroyMouseClick : MonoBehaviour
    {
        private void Start()
        {
            var anim = GetComponent<Animator>();
            anim.Play("Take 001");
            StartCoroutine(DestroyAnimation(0.5f));
        }

        private IEnumerator DestroyAnimation(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
