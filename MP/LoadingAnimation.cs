using UnityEngine;

namespace MP
{
    public class LoadingAnimation : MonoBehaviour
    {
        public float Speed = 180f;
        public float Radius = 1f;
        public GameObject Particles;

        private Vector3 _offset;
        private Transform _transform;
        private Transform _particleTransform;
        private bool _isAnimating;


        private void Awake()
        {
            _particleTransform = Particles.GetComponent<Transform>();
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            if (_isAnimating)
            {
                _transform.Rotate(0f, 0f, Speed * Time.deltaTime);
                _particleTransform.localPosition = Vector3.MoveTowards(_particleTransform.localPosition, _offset, 0.5f * Time.deltaTime);
            }
        }

        public void StartLoaderAnimation()
        {
            _isAnimating = true;
            _offset = new Vector3(Radius, 0f, 0f);
            Particles.SetActive(true);
        }

        public void StopLoaderAnimation()
        {
            Particles.SetActive(false);
        }
    }
}