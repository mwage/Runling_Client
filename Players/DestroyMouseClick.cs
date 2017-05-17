using System.Collections;
using UnityEngine;

namespace Players
{
    public class DestroyMouseClick : MonoBehaviour
    {
        void Start()
        {
            var anim = GetComponent<Animator>();
            anim.Play("Take 001");
            StartCoroutine(DestroyAnimation());
        }

        private IEnumerator DestroyAnimation()
        {
            yield return new WaitForSeconds(1.66f / 2);
            Destroy(gameObject);
        }
    }
}
