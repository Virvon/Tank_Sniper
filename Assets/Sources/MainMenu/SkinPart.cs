using UnityEngine;

namespace Assets.Sources.MainMenu
{
    public class SkinPart : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshrenderer;
        [SerializeField] private uint _skinedMaterialIndex;

        public void SetMaterial(Material material)
        {
            Material[] materials = _meshrenderer.materials;
            materials[_skinedMaterialIndex] = material;
            _meshrenderer.materials = materials;
        }
    }
}
