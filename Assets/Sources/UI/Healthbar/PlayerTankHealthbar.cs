﻿using Assets.Sources.Gameplay;
using Assets.Sources.Utils;
using MPUIKIT;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class PlayerTankHealthbar : MonoBehaviour
    {
        [SerializeField] private MPImage _barFill;
        [SerializeField] private MPImage _heartFill;
        [SerializeField, Range(0, 1)] private float _fillAmount;
        [SerializeField, Range(0, 1)] private float _barFillPercent;

        //private PlayerTank _playerTank;

        [Inject]
        private void Construct()
        {
            //_playerTank = playerTank;

            OnHealthChanged();

            //_playerTank.HealthChanged += OnHealthChanged;
        }

        private void OnValidate() =>
            ChangeHealthbar();

        //private void OnDestroy() =>
        //    _playerTank.HealthChanged -= OnHealthChanged;

        private void ChangeHealthbar()
        {
            _barFill.fillAmount = _fillAmount.Remap(1 - _barFillPercent, 1, 0, 1);
            _heartFill.fillAmount = _fillAmount.Remap(0, 1 - _barFillPercent, 0, 1);
        }

        private void OnHealthChanged()
        {
            //_fillAmount = (float)_playerTank.Health / _playerTank.MaxHealth;

            ChangeHealthbar();
        }
    }
}
