#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace Tea
{
   public class JSONScriptableObjectExample : MonoBehaviour
   {
      /// <summary>
      /// 文件路径
      /// </summary>
      const string filePath = "path/to/skill.json";

      const string ScriptableObjectPath = "path/to/skill.json";

      /// <summary>
      /// 保存技能数据
      /// </summary>
      /// <param name="data"></param>
      private static void SaveSkillData(AllSkillData data)
      {
         string path = AssetDatabase.GetAssetPath(Selection.activeObject);
         JsonUtilityHelper.SaveToJsonFile(data, path + "/skill.json");
      }
      /// <summary>
      /// 加载技能数据
      /// </summary>
      /// <returns></returns>
      private static AllSkillData LoadSkillData()
      {
#if UNITY_EDITOR
         string path = AssetDatabase.GetAssetPath(Selection.activeObject);
         return JsonUtilityHelper.LoadFromJsonFile<AllSkillData>(path);

#else
         return JsonUtilityHelper.LoadFromJsonFile<SkillDataList>(filePath);
#endif
      }

#if UNITY_EDITOR

      const string editorFilePath = "Assets/Resources/Skill";

      /// <summary>
      /// 转换为Json
      /// </summary>
      [MenuItem("TeaAdd/Skill/从Resources Skill中的文件 转换为JSON字符串")]
      private static void ConvertToJson()
      {
         string folderPath = "Assets/Resources/Skill"; // 替换为你的文件夹路径
         string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });
         if (assetGuids.Length > 0)
         {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            if (scriptableObject != null)
            {
               // 处理获取到的第一个ScriptableObject
               Debug.Log(scriptableObject.name);

               // 从JSON文件加载SkillData
               AllSkillData skillData = (AllSkillData)scriptableObject;

               if (skillData != null)
               {
                  JsonUtilityHelper.SaveToJsonFile(skillData, "Assets/Resources/Skill/skill.json");
               }
            }
         }
         else
         {
            Debug.LogWarning($"文件夹{folderPath}中没有ScriptableObjects");
         }
      }

      /// <summary>
      /// 转换为 可视化 对象
      /// </summary>
      [MenuItem("TeaAdd/Skill/将JSON导入ScriptableObject")]
      private static void ImportJsonToScriptableObject()
      {
         var jsPath = editorFilePath + "/skill.json";
         if (File.Exists(jsPath))
         {
            // 从JSON文件读取内容
            string json = File.ReadAllText(jsPath);

            // 将JSON字符串反序列化为ScriptableObject
            ScriptableObject scriptableObject = JsonConvert.DeserializeObject<AllSkillData>(json) as AllSkillData;

            // 处理ScriptableObject
            if (scriptableObject != null)
            {
               Debug.Log("从JSON导入的ScriptableObject:" + scriptableObject.name);
               var assetPath = editorFilePath + "/NewSkillData.asset";

               AssetDatabase.CreateAsset((AllSkillData)scriptableObject, assetPath);
               AssetDatabase.SaveAssets();

               EditorUtility.FocusProjectWindow();
               Selection.activeObject = scriptableObject;
            }
            else
            {
               Debug.LogWarning("从JSON中导入ScriptableObject失败");
            }
         }
         else
         {
            Debug.LogWarning("JSON文件找不到:" + jsPath);
         }
      }

      /// <summary>
      /// 在Unity编辑器中，右键单击Assets菜单，在下拉菜单中选择Create SkillData菜单项
      /// </summary>
      [MenuItem("TeaAdd/Skill/创建新的技能列表")]
      public static void CreateSkillData()
      {
         AllSkillData skillData = AllSkillData.CreateNewSkillData();

         // 实现方法的内容
         string path = AssetDatabase.GetAssetPath(Selection.activeObject);
         string assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/NewSkillData.asset");
         //string assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/NewSkillData.asset");
         AssetDatabase.CreateAsset(skillData, assetPath);
         AssetDatabase.SaveAssets();

         EditorUtility.FocusProjectWindow();
         Selection.activeObject = skillData;
      }
#endif
   }
}