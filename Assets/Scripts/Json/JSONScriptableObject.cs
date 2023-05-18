using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
using UnityEditor;
using WeChatWASM;
#endif

namespace Tea
{
   public class JSONScriptableObject
   {
      /// <summary>
      /// 文件路径
      /// </summary>
      const string filePath = "Skill/skill.json";
      static string cacheJsonData;
      static JSONScriptableObject instance;
      /// <summary>
      /// 加载技能数据
      /// </summary>
      /// <returns></returns>
      private static AllSkillData LoadSkillDatas()
      {
#if UNITY_EDITOR
         string path = AssetDatabase.GetAssetPath(Selection.activeObject);
         return JsonUtilityHelper.LoadFromJsonFile<AllSkillData>(path);

#else
         return JsonUtilityHelper.LoadFromJsonFile<AllSkillData>(filePath);
#endif
      }

      public static void LoadSkillData()
      {
         string filePath = Path.Combine(Application.streamingAssetsPath, "Skill/skill.json");
#if UNITY_ANDROID && !UNITY_EDITOR
        // 在Android平台上，使用UnityWebRequest来读取StreamingAssets文件夹中的内容
         Coroutine coroutine = StartCoroutine(ReadAndroidJSON(filePath));
#else
         // 在其他平台上，直接使用File类来读取StreamingAssets文件夹中的内容
         ReadJSON(filePath);
#endif
         var data = ConvertingToData();
         if (data)
         {

         }
      }


      private static void ReadJSON(string filePath)
      {
         if (File.Exists(filePath))
         {
            string jsonContent = File.ReadAllText(filePath);
            cacheJsonData = jsonContent;
         }
         else
         {
            Debug.LogError($"在路径 [{filePath}] 上加载JSON文件失败:");
         }
      }
      private static IEnumerator ReadAndroidJSON(string filePath)
      {
         // 使用UnityWebRequest来读取StreamingAssets文件夹中的内容
         using (var www = new UnityEngine.Networking.UnityWebRequest(filePath))
         {
            if (filePath.Contains("://"))
            {
               www.url = filePath;
            }
            else
            {
               www.url = "file://" + filePath;
            }

            www.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
               string jsonContent = www.downloadHandler.text;
               cacheJsonData = jsonContent;
            }
            else
            {
               Debug.LogError("在路径上加载JSON文件失败:" + filePath + "\n" + www.error);
            }
         }
      }

      /// <summary>
      /// 字符转化为数据集
      /// </summary>
      /// <returns></returns>
      public static AllSkillData ConvertingToData()
      {
         // 将JSON字符串反序列化为ScriptableObject
         AllSkillData getSkillData = JsonConvert.DeserializeObject<AllSkillData>(cacheJsonData) as AllSkillData;

         // 处理ScriptableObject
         if (getSkillData != null)
         {
            return getSkillData;
         }
         else
         {
            return null;
         }
      }

      #region UNITY_EDITOR
#if UNITY_EDITOR
      const string editorFilePath = "Assets/StreamingAssets/Skill";

      /// <summary>
      /// 转换为Json
      /// </summary>
      [MenuItem("TeaAdd/Skill/从Resources Skill中的文件 转换为JSON字符串")]
      private static void ConvertToJson()
      {
         string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { editorFilePath });
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
                  JsonUtilityHelper.SaveToJsonFile(skillData, editorFilePath + "/skill.json");
               }
            }
         }
         else
         {
            Debug.LogWarning($"文件夹{editorFilePath}中没有ScriptableObjects");
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

      #endregion
   }
}