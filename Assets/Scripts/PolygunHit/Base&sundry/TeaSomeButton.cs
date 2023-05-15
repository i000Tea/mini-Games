using UnityEngine;
using UnityEngine.UI;
namespace Tea.PolygonHit
{
   /// <summary>
   /// 按钮信息
   /// </summary>
   public class TeaSomeButton : ButtonControlBase
   {
      [SerializeField]
      private ButtonType m_ButtonType;
      [SerializeField]
      private string showButtonName;

      Text ButtonText => transform.GetChild(0).GetComponent<Text>();

      //[SerializeField]
      //[Range(0.1f, 2f)]
      //private float TextScaleSize = 1;
      //[SerializeField]
      //[Range(0f, 300f)]
      //private float TextSize = 100;

      private void OnValidate()
      {
         if (showButtonName == null || showButtonName == "" || showButtonName == "新按钮_")
         {
            ButtonText.text = "新按钮";
         }
         else
         {
            ButtonText.text = showButtonName;
         }
      }

      protected override void OnClick()
      {
         TouthButton();
      }
      private void TouthButton()
      {
         EventControl.InvokeButton(m_ButtonType);
      }
   }
}
