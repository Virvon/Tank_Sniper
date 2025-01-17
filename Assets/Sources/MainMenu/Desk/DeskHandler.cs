﻿using Assets.Sources.Services.InputService;
using Assets.Sources.Tanks;
using System;
using UnityEngine;

namespace Assets.Sources.MainMenu.Desk
{
    public class DeskHandler : IDisposable
    {
        private const float MaxRaycastDistance = 100;
        private const float ReplacedTankBuildingHeight = 2;

        private readonly IInputService _inputService;
        private readonly MainMenuCamera _camera;

        private DeskCell _currentTankParentCell;
        private DeskTankWrapper _tankWrapper;
        private Vector2 _lastHandlePosition;
        private DeskCell _lastMarkedCell;

        private bool _isActive;

        public DeskHandler(IInputService inputService, MainMenuCamera camera)
        {
            _inputService = inputService;

            _camera = camera;

            _isActive = true;

            _inputService.HandlePressed += OnHandlePressed;
            _inputService.HandleMoved += OnHandleMoved;
            _inputService.HandleMoveCompleted += OnHandleMoveCompleted;
        }

        public void Dispose()
        {
            _inputService.HandlePressed -= OnHandlePressed;
            _inputService.HandleMoved -= OnHandleMoved;
            _inputService.HandleMoveCompleted -= OnHandleMoveCompleted;
        }

        public void SetActive(bool isActive) =>
            _isActive = isActive;

        private void OnHandlePressed(Vector2 handlePosition)
        {
            if (CheckDeskCellIntersection(handlePosition, out DeskCell deskCell) && _isActive)
            {
                _tankWrapper = deskCell.GetTankWrapper();
                _currentTankParentCell = deskCell;
            }
        }

        private void OnHandleMoved(Vector2 handlePosition)
        {
            if (_tankWrapper != null)
            {
                Ray ray = _camera.GetRay(handlePosition);
                Plane plane = new Plane(Vector3.up, new Vector3(0, ReplacedTankBuildingHeight, 0));

                if (plane.Raycast(ray, out float distanceToPlane))
                {
                    Vector3 worldPosition = ray.GetPoint(distanceToPlane);

                    _tankWrapper.transform.position = new Vector3(worldPosition.x, ReplacedTankBuildingHeight, worldPosition.z);
                }

                if (CheckDeskCellIntersection(handlePosition, out DeskCell deskCell))
                {
                    if (deskCell != _lastMarkedCell)
                    {
                        _lastMarkedCell?.HideMark();
                        _lastMarkedCell = deskCell;
                        _lastMarkedCell.Mark(_tankWrapper.TankLevel);
                    }
                }
                else if (_lastMarkedCell != null)
                {
                    _lastMarkedCell.HideMark();
                    _lastMarkedCell = null;
                }

                _lastHandlePosition = handlePosition;
            }
        }

        private async void OnHandleMoveCompleted()
        {
            if (_tankWrapper == null)
                return;

            if (CheckDeskCellIntersection(_lastHandlePosition, out DeskCell deskCell) && deskCell.CanPlace(_tankWrapper.TankLevel))
            {
                if (deskCell.IsEmpty)
                {
                    deskCell.PlaceTank(_tankWrapper);
                }
                else
                {
                    _tankWrapper.Destroy();
                    await deskCell.UpgradeTank();
                }
            }
            else
            {
                _currentTankParentCell.PlaceTank(_tankWrapper);
            }

            if (_lastMarkedCell != null)
            {
                _lastMarkedCell.HideMark();
                _lastMarkedCell = null;
            }

            _tankWrapper = null;
        }

        private bool CheckDeskCellIntersection(Vector2 handlePosition, out DeskCell deskCell)
        {
            deskCell = null;

            return Physics.Raycast(
                _camera.GetRay(handlePosition),
                out RaycastHit hitInfo,
                MaxRaycastDistance) && hitInfo.transform.TryGetComponent(out deskCell);
        }
    }
}
