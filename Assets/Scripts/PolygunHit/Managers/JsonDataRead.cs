using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using WeChatWASM;
using UnityEngine.Networking;
using Tea.PolygonHit;

namespace Tea
{
   public class JsonDataRead : Singleton<JsonDataRead>
   {
      [SerializeField]
      public string DATA_CDN_File;
      public string jsonFile;
      public string skillJsonFile = "StreamingAssets/Skill/skill.json";

      const string jsonFilePath = "Assets/StreamingAssets/Skill";

      /// <summary>
      /// 缓存json字符
      /// </summary>
      string cacheJsonData;

      protected override void Awake()
      {
         StartCoroutine(ReadSkillData());
      }
      IEnumerator ReadSkillData()
      {
         yield return new WaitForFixedUpdate();
         string filePath = Path.Combine(Application.streamingAssetsPath, "Skill/skill.json");
#if UNITY_EDITOR
         var jsPath = jsonFilePath + "/skill.json";
         if (File.Exists(jsPath))
         {
            // 从JSON文件读取内容
            cacheJsonData = File.ReadAllText(jsPath);
         }
         else
         {
            Debug.LogWarning("JSON文件找不到:" + jsPath);
         }

#elif UNITY_WEBGL
         var endFile = DATA_CDN_File + skillJsonFile;
         // 处理 StreamingAssets 文件夹中的本地文件路径
         if (endFile.StartsWith("file://"))
         {
            endFile = Path.Combine(Application.streamingAssetsPath, endFile.Substring("file://".Length));
         }

         using (UnityWebRequest webRequest = UnityWebRequest.Get(endFile))
         {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
               string json = webRequest.downloadHandler.text;
               Debug.Log("下载的 JSON 文件内容：" + json);
               cacheJsonData = json;
            }
            else
            {
               Debug.LogError($"下载地址{endFile} 无法下载 JSON 文件。错误：{webRequest.error}");
            }
         }
#elif UNITY_ANDROID && !UNITY_EDITOR
         // 在Android平台上，使用UnityWebRequest来读取StreamingAssets文件夹中的内容
         Coroutine coroutine = StartCoroutine(ReadAndroidJSON(filePath));
#else
         // 在其他平台上，直接使用File类来读取StreamingAssets文件夹中的内容
         ReadJSON(filePath);
#endif
         try
         {
            var data = ConvertingToData();
            cacheJsonData = default;
            if (!data)
            {
               Debug.LogWarning("数据读取异常");
            }
            else
            {
               SkillAndBuffManager.I.SetSkillData(data);
            }
         }
         catch (System.Exception)
         {
            throw;
         }

      }

      private void ReadJSON(string filePath)
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
      private IEnumerator ReadAndroidJSON(string filePath)
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
      public AllSkillData ConvertingToData()
      {
         // 将JSON字符串反序列化为ScriptableObject
         AllSkillData getSkillData = ScriptableObject.CreateInstance<AllSkillData>();
         // 将JSON字符串反序列化为ScriptableObject
         //AllSkillData getSkillData = JsonConvert.DeserializeObject<AllSkillData>(cacheJsonData) as AllSkillData;
         //AllSkillData getSkillData = JsonConvert.DeserializeObject<AllSkillData>(cacheJsonData) as AllSkillData;

         if (cacheJsonData == null || cacheJsonData == default)
         {
            Debug.LogWarning("缓存字符串为空");
         }
         JsonConvert.PopulateObject(cacheJsonData, getSkillData);

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
   }
}
