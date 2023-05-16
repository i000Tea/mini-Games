using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Tea.PolygonHit
{
   /// <summary>
   /// 获取技能按钮
   /// </summary>
   public class TeaSkillButton : TeaSomeButton
   {
      [SerializeField]
      int skillSerialNumber;
      public void SetMySkill(int input)
      {
         skillSerialNumber = input;
      }
      protected override void OnClick()
      {
         SkillManager.inst.AddSkill(skillSerialNumber);
      }
   }
}
