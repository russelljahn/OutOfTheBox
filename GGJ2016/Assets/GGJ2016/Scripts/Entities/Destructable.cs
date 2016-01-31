﻿using System;
using System.Linq;
using Assets.OutOfTheBox.Scripts.Extensions;
using Sense.Extensions;
using Sense.Injection;
using Sense.PropertyAttributes;
using UnityEngine;

namespace Assets.GGJ2016.Scripts.Entities
{
    public class Destructable : InjectableBehaviour, IInteractable
    {
        [SerializeField] private float _maxHealth = 100.0f;
        [SerializeField, Readonly] private float _health; 
        [SerializeField] private float _points = 10.0f;
        [SerializeField] private bool _breakByForce = true;
        [SerializeField] private float _impactVelocityToBreak = 4.5f;

        [SerializeField] private Rigidbody2D _rigidbody2D;

        public float MaxHealth
        {
            get { return _maxHealth; }
        }

        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                if (_health <= 0.0f && value <= 0.0f)
                {
                    return;
                }
                if (_health >= MaxHealth && value >= MaxHealth)
                {
                    return;
                }
                _health = Mathf.Clamp(value, 0f, MaxHealth);
                if (_health.IsApproximatelyZero())
                {
                    Destroyed.SafelyInvoke(this);
                }
            }
        }

        public event Action<Destructable> Destroyed;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        protected override void OnPostInject()
        {
            _health = _maxHealth;
            Destroyed += OnDestroyed;

            if (_breakByForce && _rigidbody2D.IsNull())
            {
                Debug.Log(gameObject.name + " needs a Rigidbody2D to break by force!");
            }
        }

        private void OnDestroyed(Destructable destructable)
        {
            this.InvokeAtEndOfFrame(() =>
            {
                Destroy(gameObject);
            });
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var impactVelocity = _rigidbody2D.GetPointVelocity(collision.contacts.First().point);
            Debug.Log(impactVelocity);
            if (_breakByForce && impactVelocity.magnitude >= _impactVelocityToBreak)
            {
                Destroyed.SafelyInvoke(this);
            }

        }
    }
}
