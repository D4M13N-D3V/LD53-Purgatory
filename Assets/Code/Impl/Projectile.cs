using Purgatory.Interfaces;
using System.Collections;
using UnityEngine;

namespace Purgatory.Impl
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField]
        private int _damage = 1;
        [SerializeField]
        private float _speed = 1.5f;

        public int Damage => _damage;
        public float Speed => _speed;

        public virtual void Impact()
        {
            Debug.Log($"Projectile {gameObject.name} impacted");
            Destroy(this.gameObject);
        }

        public virtual void Launch()
        {
            Debug.Log($"Projectile {gameObject.name} launched");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position += transform.forward * Time.deltaTime * _speed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ITargetable target = collision.gameObject.GetComponent<Targetable>();
            if(target==null)
                target = collision.gameObject.GetComponent<TargetableAttacker>();
            target.Damage(Damage);
            Impact();
        }
    }
}
