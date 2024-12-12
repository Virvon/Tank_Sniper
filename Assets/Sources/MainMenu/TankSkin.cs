using UnityEngine;

namespace Assets.Sources.MainMenu
{
    public class TankSkin : MonoBehaviour
    {
        [SerializeField] private SkinPart[] _parts;

        public void SetMaterial(Material material)
        {
            foreach (SkinPart part in _parts)
                part.SetMaterial(material);
        }
    }
}
