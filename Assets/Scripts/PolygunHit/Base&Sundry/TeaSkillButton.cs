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
      SkillData cacheData;
      [Space(10)]
      [SerializeField] private Image skillImage;
      [SerializeField] private string description;
      string skillName;
      public void InputSkillData(SkillData data)
      {
         cacheData = data;
         ButtonText.text = data.skillName;
         description = data.description;
         skillName = data.skillClassName;
         skillImage.sprite = data.imageName.GetSkillImage();
      }
      protected override void OnClick()
      {
         SkillAndBuffManager.I.AddSkill(cacheData);
      }
   }
}
