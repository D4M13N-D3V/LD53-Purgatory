using Cinemachine;
using Purgatory.Interfaces;
using System.Collections;
using UnityEngine;
using Purgatory.Player;

namespace Purgatory.Impl
{
    public class Obstacle : MonoBehaviour, IObstacle
    {
        [SerializeField]
        private int _damage = 1;

        public int Damage => _damage;

        public virtual void Impact()
        {
            Debug.Log("Obstacle impacted player.");
            Destroy(gameObject);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {

            ITargetable target = collision.gameObject.GetComponent<Targetable>();
            if (target == null)
                target = collision.gameObject.GetComponent<TargetableAttacker>();
            CameraController.instance.CameraShake(GetComponent<CinemachineImpulseSource>());
            target.Damage(Damage);
            Impact();
        }
    }
}