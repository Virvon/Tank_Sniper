using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class DestructionedMaterialsRenderer : MonoBehaviour
    {
        private const string ColorValue = "_BaseColor";

        [SerializeField] private MeshRenderer[] _renderers;

        private DestructionConfig _destructionConfig;

        [Inject]
        private void Construct(IStaticDataService staticDataService) =>
            _destructionConfig = staticDataService.DestructionConfig;

        public void Render()
        {
            foreach (MeshRenderer meshRenderer in _renderers)
            {
                foreach (Material material in meshRenderer.materials)
                    material.SetColor(ColorValue, material.GetColor(ColorValue) * _destructionConfig.DestructionColor);
            }
        }
    }
}
