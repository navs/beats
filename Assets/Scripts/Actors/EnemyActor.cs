using UnityEngine;
using System.Collections;

namespace Actors
{
    public class EnemyActor : MonoBehaviour
    {
        public float Lifetime = 2.0f;
        public float Speed = 0.5f;

        private float elapsedTime = 0.0f;
        private bool isDying = false;

        void Start()
        {
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > Lifetime)
            {
                isDying = true;
            }

            if (!isDying)
            {
                Move(Time.deltaTime * Speed);
            }
            else
            {
                transform.localScale -= Vector3.one * Time.deltaTime;
            }

            if (elapsedTime > Lifetime + 1)
            {
                Destroy(gameObject);
            }
        }

        private void Move(float distance)
        {
            transform.localPosition += transform.forward * distance;
        }
        
    }
}
