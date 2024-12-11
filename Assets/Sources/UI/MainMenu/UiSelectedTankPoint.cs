using Assets.Sources.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu
{
    public class UiSelectedTankPoint : SelectedTankPoint
    {
        private const string Layer = "UI";

        [SerializeField] private float _scale;

        public void Show() =>
            SelectedTank.gameObject.SetActive(true);

        public void Hide() =>
            SelectedTank.gameObject.SetActive(false);

        protected override async UniTask ChangeSelectedTank(uint level)
        {
            await base.ChangeSelectedTank(level);

            foreach(Transform transform in SelectedTank.GetComponentsInChildren<Transform>())
                transform.gameObject.layer = LayerMask.NameToLayer(Layer);

            SelectedTank.transform.localScale = Vector3.one * _scale;
        }

        protected override Quaternion GetRotation()
        {
            return TankPoint.rotation;
        }

        protected override async UniTask OnStart()
        {
            await base.OnStart();
            Hide();
        }
    }
}
