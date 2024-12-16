using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.MainMenu;
using Assets.Sources.Services.InputService;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class UiSelectedTankPoint : SelectedTankPoint
    {
        private const string Layer = "UI";

        [SerializeField] private float _scale;
        [SerializeField] private float _rotationSensivity;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private RectTransform _rotationArea;
        [SerializeField] private Transform _tankParentPoint;

        private IInputService _inputService;
        private MainMenuCamera _camera;

        private Vector2 _lastHandlePosition;
        private bool _isRotatedToTarget;
        private Quaternion _startRotation;
        private bool _startHandledRotation;
        private bool _isHided;
        private Quaternion _targetRotation;
        private Coroutine _rotater;

        [Inject]
        private void Construct(IInputService inputService, MainMenuCamera camera)
        {
            _inputService = inputService;
            _camera = camera;

            _inputService.HandlePressed += OnHandlePressed;
            _inputService.HandleMoved += OnHandleMoved;
            _inputService.HandleMoveCompleted += OnHandleMoveCompleted;

            _isRotatedToTarget = false;
            _startHandledRotation = false;
        }

        private void OnDestroy()
        {
            _inputService.HandlePressed -= OnHandlePressed;
            _inputService.HandleMoved -= OnHandleMoved;
            _inputService.HandleMoveCompleted -= OnHandleMoveCompleted;
        }

        public void Show()
        {
            SelectedTank.gameObject.SetActive(true);
            _isHided = false;
        }

        public void Hide()
        {
            SelectedTank.gameObject.SetActive(false);
            _isHided = true;
        }

        public void SetTargetRotation(int angle)
        {
            _targetRotation = _startRotation * Quaternion.AngleAxis(angle, Vector3.up);

            RotateToTarget();
        }

        public void ResetTargetRotation()
        {
            _targetRotation = _startRotation;

            RotateToTarget();
        }

        protected override async UniTask ChangeSelectedTank(uint level, bool needToAnimate)
        {
            await base.ChangeSelectedTank(level, needToAnimate);

            foreach (Transform transform in SelectedTank.GetComponentsInChildren<Transform>())
                transform.gameObject.layer = LayerMask.NameToLayer(Layer);

            SelectedTank.transform.localScale = Vector3.one * _scale;

            if (_isHided)
                Hide();
        }

        protected override Quaternion GetRotation() =>
            TankPoint.transform.rotation * base.GetRotation();

        protected override async UniTask OnStart()
        {
            _startRotation = TankPoint.transform.rotation;
            ResetTargetRotation();
            await base.OnStart();
            Hide();
        }

        protected override Transform GetParent() =>
            _tankParentPoint.transform;

        protected override async UniTask<GameObject> CreateTank(
            TankData tankData,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            ITankFactory tankFactory)
        {
            Tank tank = await tankFactory.CreateTank(tankData.Level, position, rotation, parent, tankData.SkinType, tankData.DecalType, true);

            return tank.gameObject;
        }

        private void OnHandlePressed(Vector2 handlePosition)
        {
            _startHandledRotation = RectTransformUtility.RectangleContainsScreenPoint(_rotationArea, handlePosition, _camera.UiCamera);
            _lastHandlePosition = handlePosition;
            _isRotatedToTarget = false;
        }

        private void OnHandleMoved(Vector2 handlePosition)
        {
            if (_startHandledRotation == false)
                return;

            float input = handlePosition.x - _lastHandlePosition.x;

            _lastHandlePosition = handlePosition;

            float rotation = input * _rotationSensivity;


            TankPoint.transform.rotation = Quaternion.RotateTowards(TankPoint.transform.rotation, TankPoint.transform.rotation * Quaternion.Euler(0, 180, 0), rotation);
        }

        private void OnHandleMoveCompleted() =>
            RotateToTarget();

        private void RotateToTarget()
        {
            if (_rotater != null)
                StopCoroutine(_rotater);

            _rotater = StartCoroutine(Rotater(_targetRotation));
        }

        private IEnumerator Rotater(Quaternion targetRotation)
        {
            _isRotatedToTarget = true;

            while (_isRotatedToTarget && TankPoint.transform.rotation != targetRotation)
            {
                TankPoint.transform.rotation = Quaternion.RotateTowards(TankPoint.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

                yield return null;
            }

            _isRotatedToTarget = false;
        }
    }
}
