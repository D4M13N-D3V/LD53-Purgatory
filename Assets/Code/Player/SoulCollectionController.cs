using Purgatory;
using Purgatory.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Purgatory.Player
{
    public class SoulCollectionController : MonoBehaviour
    {
        [SerializeField]
        private int _soulCount = 0;
        [SerializeField]
        private float _collectionRadius = 10f;

        private List<DockSoul> _autoCollectionTargets = new List<DockSoul>();

        public void AddSoul(int amount)
        {
            _soulCount += amount;
        }

        public void RemoveSoul(int amount)
        {
            if (_soulCount > amount)
                _soulCount -= amount;
            else
                _soulCount = 0;
        }

        private IEnumerator AttackCoroutine()
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _collectionRadius);
            var targets = hitColliders.Where(x=> x.GetComponent<DockSoul>()!=null).Select(x => x.GetComponent<DockSoul>()).ToList();
            _autoCollectionTargets.AddRange(targets);
            StartCoroutine(AttackCoroutine());
            yield return new WaitForSeconds(1f);
        }
        private void Start()
        {
            StartCoroutine(AttackCoroutine());
        }

        private void Update()
        {
            foreach(var target in _autoCollectionTargets)
            {

                target.transform.rotation = Quaternion.Slerp(target.transform.rotation, Quaternion.LookRotation(transform.position - target.transform.position), 5f * Time.deltaTime);


                //move towards the player
                target.transform.position += target.transform.forward * Time.deltaTime * 5f;

            }
        }

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(GetComponent<Transform>().position - Vector3.down, Vector3.up, _collectionRadius);
        }
    }
}