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
      string description;
      string skillName;
      public void InputSkillData(SkillData data)
      {
         ButtonText.text = data.skillName;
         description = data.description;
         skillName = data.skillClassName;
      }
      protected override void OnClick()
      {
         SkillManager.I.AddSkill(skillName);
      }
   }
}
