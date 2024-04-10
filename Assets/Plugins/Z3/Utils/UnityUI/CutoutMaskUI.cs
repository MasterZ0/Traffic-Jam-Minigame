using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Z3.UnityUI
{
    /// <summary>
    /// Renders an image outside of a mask boundary.
    /// </summary>
    public class CutoutMaskUI : Image
    {
        public override Material materialForRendering
        {
            get
            {
                Material material = new Material(base.materialForRendering);
                material.SetFloat("_StencilComp", (float)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}
