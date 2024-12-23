using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Animation
{
    public class EnemyAnimation : MonoBehaviour
    {
        public event Action BulletNeedetToCreate;

        public void CreateBullet() =>
            BulletNeedetToCreate?.Invoke();
    }
}
