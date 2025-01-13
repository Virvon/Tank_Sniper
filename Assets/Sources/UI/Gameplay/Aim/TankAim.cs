using Assets.Sources.Gameplay.Player.Weapons;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.Aim
{
    public class TankAim : MonoBehaviour
    {
        [SerializeField] private Transform _superBulletsContainer;
        [SerializeField] private Transform _superAim;
        [SerializeField] private CanvasGroup _superAimCanvasGroup;
        [SerializeField] private float _superAimMoveSpeed;

        private PlayerTankWeapon _playerTankWeapon;
        private IUiFactory _uiFactory;

        private List<SuperBulletIcon> _icons;
        private Coroutine _superAimMover;

        [Inject]
        private async void Construct(PlayerTankWeapon playerTankWeapon, IUiFactory uiFactory)
        {
            _playerTankWeapon = playerTankWeapon;
            _uiFactory = uiFactory;

            _icons = new();
            _superAimCanvasGroup.alpha = 0;

            await Fill();

            _playerTankWeapon.BulletsCountChanged += OnBulletsCountChanged;
        }

        private async UniTask Fill()
        {
            for(int i = 0; i < _playerTankWeapon.RequireShotsNumberToSuperShot; i++)
            {
                SuperBulletIcon icon = await _uiFactory.CreateSuperBulletIcon(_superBulletsContainer);
                _icons.Add(icon);
            }
        }

        private void OnBulletsCountChanged()
        {
            foreach (SuperBulletIcon icon in _icons)
                icon.SetAcive(false);

            IEnumerable<SuperBulletIcon> activeIcons = _icons.Take((int)(_playerTankWeapon.RequireShotsNumberToSuperShot - _playerTankWeapon.ShootsNumberToSuperShot));

            foreach (SuperBulletIcon icon in activeIcons)
                icon.SetAcive(true);

            if (_playerTankWeapon.RequireShotsNumberToSuperShot == _playerTankWeapon.RequireShotsNumberToSuperShot - _playerTankWeapon.ShootsNumberToSuperShot)
            {
                _superAimCanvasGroup.alpha = 1;
                _superAimMover = StartCoroutine(SuperAimMover());
            }
            else if (_superAimMover != null)
            {
                _superAimCanvasGroup.alpha = 0;
                StopCoroutine(_superAimMover);
            }
        }

        private IEnumerator SuperAimMover()
        {
            bool isMoved = true;

            while (isMoved)
            {
                _superAim.transform.rotation = Quaternion.RotateTowards(_superAim.transform.rotation, _superAim.transform.rotation * Quaternion.Euler(0, 0, 100), _superAimMoveSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}
