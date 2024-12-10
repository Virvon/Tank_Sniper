using Assets.Sources.Services.InputService;
using System;
using UnityEngine;

namespace Assets.Sources.MainMenu
{
    public class DeskHandler : IDisposable
    {
        private const float MaxRaycastDistance = 100;
        private const float ReplacedTankBuildingHeight = 2;

        private readonly IInputService _inputService;
        private readonly Camera _camera;

        private DeskCell _currentTankParentCell;
        private Tank _tank;
        private Vector2 _lastHandlePosition;
        private DeskCell _lastMarkedCell;

        public DeskHandler(IInputService inputService, Camera camera)
        {
            _inputService = inputService;

            _camera = camera;

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

        private void OnHandlePressed(Vector2 handlePosition)
        {
            if(CheckDeskCellIntersection(handlePosition, out DeskCell deskCell))
            {
                _tank = deskCell.GetTank();
                _currentTankParentCell = deskCell;
            }
        }

        private void OnHandleMoved(Vector2 handlePosition)
        {
            if (_tank != null)
            {
                Ray ray = GetRay(handlePosition);
                Plane plane = new Plane(Vector3.up, new Vector3(0, ReplacedTankBuildingHeight, 0));

                if (plane.Raycast(ray, out float distanceToPlane))
                {
                    Vector3 worldPosition = ray.GetPoint(distanceToPlane);

                    _tank.transform.position = new Vector3(worldPosition.x, ReplacedTankBuildingHeight, worldPosition.z);
                }

                if(CheckDeskCellIntersection(handlePosition, out DeskCell deskCell))
                {
                    if(deskCell != _lastMarkedCell)
                    {
                        _lastMarkedCell?.HideMark();
                        _lastMarkedCell = deskCell;
                        _lastMarkedCell.Mark(_tank.Level);
                    }
                }
                else if(_lastMarkedCell != null)
                {
                    _lastMarkedCell.HideMark();
                    _lastMarkedCell = null;
                }

                _lastHandlePosition = handlePosition;
            }
        }

        private async void OnHandleMoveCompleted()
        {
            if (_tank == null)
                return;

            if (CheckDeskCellIntersection(_lastHandlePosition, out DeskCell deskCell) && deskCell.CanPlace(_tank.Level))
            {
                if(deskCell.IsEmpty)
                {
                    deskCell.PlaceTank(_tank);
                }
                else
                {
                    _tank.Destroy();
                    await deskCell.UpgradeTank();
                }
            }
            else
            {
                _currentTankParentCell.PlaceTank(_tank);
            }

            if(_lastMarkedCell != null)
            {
                _lastMarkedCell.HideMark();
                _lastMarkedCell = null;
            }

            _tank = null;
        }

        private bool CheckDeskCellIntersection(Vector2 handlePosition, out DeskCell deskCell)
        {
            deskCell = null;

            return Physics.Raycast(
                GetRay(handlePosition),
                out RaycastHit hitInfo,
                MaxRaycastDistance) && hitInfo.transform.TryGetComponent(out deskCell);
        }

        private Ray GetRay(Vector2 handlePosition) =>
            _camera.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));
    }
}
