﻿using Marmalade.TheGameOfLife.Shared;
using System;
using System.Collections.Generic;
using UnityEngine;
using Z3.ObjectPooling;
using Z3.Paths;
using Z3.Utils.ExtensionMethods;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class BlackCar : MonoBehaviour
    {
        [SerializeField] private Transform collisionFx;
        [SerializeField] private List<PathPack> paths;
        [SerializeField] private BezierCurve curve;

        [Inject]
        private TrafficJamConfig config;

        private FollowPathBySpeed runner;
        private List<ICashHandler> hittedPlayers = new();
        private bool gameOver;

        public event Action OnFinish { add => runner.OnFinish += value; remove => runner.OnFinish -= value; }

        private void Awake()
        {
            this.InjectServices();

            runner = new FollowPathBySpeed()
            {
                Transfom = transform,
                Speed = 5f
            };

            runner.OnFinish += FinishRoute;
        }

        private void OnDestroy()
        {
            runner.Dispose();
        }

        private void OnEnable()
        {
            gameOver = false;
            runner.PathPack = paths.GetRandom();

            runner.SetReference(transform.position, transform.rotation);
            runner.Reset();
            runner.Start();
        }

        private void FixedUpdate()
        {
            if (gameOver)
                return;

            runner.Update();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.rigidbody || !collision.rigidbody.TryGetComponent(out ICashHandler player))
                return;

            if (hittedPlayers.Contains(player))
                return;

            hittedPlayers.Add(player);
            player.RemoveCash(config.LossCashByCollision);
            ObjectPool.SpawnPooledObject(collisionFx, collision.contacts[0].point, transform.rotation);
        }

        private void FinishRoute()
        {
            hittedPlayers.Clear();
            this.ReturnToPool();
        }

        internal void GameOver()
        {
            gameOver = true;
        }
    }
}
