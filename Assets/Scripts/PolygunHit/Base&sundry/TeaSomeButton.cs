using UnityEngine;
using UnityEngine.SceneManagement;
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

      [SerializeField]
      bool ChangeName;
      Text ButtonText => transform.GetChild(0).GetComponent<Text>();

      //[SerializeField]
      //[Range(0.1f, 2f)]
      //private float TextScaleSize = 1;
      //[SerializeField]
      //[Range(0f, 300f)]
      //private float TextSize = 100;

      private void OnValidate()
      {
         if (ChangeName)
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
      }

      protected override void OnClick()
      {
         TouthButton();
      }
      private void TouthButton()
      {
         switch (m_ButtonType)
         {
            // 直接执行的场景切换
            case ButtonType.Exit:
               // 跳回0场景
               SceneManager.LoadScene(0);
               break;
            case ButtonType.Restart:
               // 重新开始
               SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
               break;
            //按钮事件
            default:
               if ((int)m_ButtonType > 10 && (int)m_ButtonType < 21)
               {
                  EventControl.InvokeButton(m_ButtonType);
               }
               break;
            // 状态切换
            case ButtonType.Gameing_Pause:
               EventControl.SetGameState(GameState.Pause);
               break;
            case ButtonType.Gameing_Again:
               EventControl.SetGameState(GameState.Gameing);
               break;
         }
      }
   }
}
