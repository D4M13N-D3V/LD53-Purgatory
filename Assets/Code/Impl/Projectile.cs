using Cinemachine;
using Purgatory.Interfaces;
using Purgatory.Player;
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
        [SerializeField]
        private float _minimumArchHeight = 3.5f;
        [SerializeField]
        private float _maximummArchHeight = 5.5f;
        [SerializeField]
        private bool _horizontalCurve = false;
        [SerializeField] float _timeToLive = 0f;

        public Vector3 TargetLocation;
        public Vector3 StartingLocation;
        public Vector3 ControlLocation;

        public int Damage => _damage;
        public float Speed => _speed;

        private float _sampleTime = 0f;

        public virtual void Impact()
        {
            Debug.Log($"Projectile {gameObject.name} impacted");
            Destroy(this.gameObject);
        }

        public virtual void Launch()
        {
            Debug.Log($"Projectile {gameObject.name} launched");
        }

        internal Vector3 evaluateArc(float t)
        {
            Vector3 ac = Vector3.Lerp(StartingLocation, ControlLocation, t);
            Vector3 cb = Vector3.Lerp(ControlLocation, TargetLocation, t); ;
            return Vector3.Lerp(ac, cb, t);
        }

        private void OnDrawGizmos()
        {
            if (StartingLocation == null || TargetLocation == null)
                return;

            for (var i = 0; i < 20; i++)
            {
                Gizmos.DrawWireSphere(evaluateArc(i / 20f), 0.1f);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            StartingLocation = transform.position;
            if (_minimumArchHeight == 0f || _maximummArchHeight == 0f)
                ControlLocation = Between(StartingLocation, TargetLocation, 0.5f);
            else if (_horizontalCurve)
                ControlLocation = Between(StartingLocation, TargetLocation, 0.5f) + transform.right * Random.Range(_minimumArchHeight, _maximummArchHeight);
            else
                ControlLocation = Between(StartingLocation, TargetLocation, 0.5f) + transform.up * Random.Range(_minimumArchHeight, _maximummArchHeight);

            if (_timeToLive != 0f)
                StartCoroutine(TimeToLive());
        }

        private IEnumerator TimeToLive()
        {
            yield return new WaitForSeconds(_timeToLive);
            Destroy(gameObject);
        }

        Vector3 Between(Vector3 v1, Vector3 v2, float percentage)
        {
            return (v2 - v1) * percentage + v1;
        }


        // Update is called once per frame
        void Update()
        {
            _sampleTime += Time.deltaTime * Speed;
            transform.position = evaluateArc(_sampleTime);
            transform.forward = evaluateArc(_sampleTime) - transform.position;

            if (transform.position == TargetLocation)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ITargetable target = collision.gameObject.GetComponent<PlayerController>();

            if (target != null)
            {
                CameraController.instance.CameraShake(GetComponent<CinemachineImpulseSource>());
                PlayerController.instance?.Damage(Damage);
                Impact();
            }


            if (target==null)
               target = collision.gameObject.GetComponent<Targetable>();
            if(target==null)
               target = collision.gameObject.GetComponent<TargetableAttacker>();

            if (target != null)
            {
                CameraController.instance.CameraShake(GetComponent<CinemachineImpulseSource>());
                target?.Damage(Damage);
                Impact(); 
            }
        }
    }
}
