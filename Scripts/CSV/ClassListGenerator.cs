using System.IO;
using UnityEditor;
using UnityEngine;

public static class ClassListGenerator
{
    public static void GenerateScriptableObjectFile(string className, string outputPath)
    {
        string soClassContent = GenerateScriptableObjectContent(className);
        string fullPath = Path.Combine(outputPath, className + "List.cs");

        // 写入类文件
        File.WriteAllText(fullPath, soClassContent);
        Debug.Log("ScriptableObject file generated at: " + fullPath);

        // 刷新 Unity 资源
        AssetDatabase.Refresh();
    }

    private static string GenerateScriptableObjectContent(string className)
    {
        string template = "using System.Collections.Generic;\n" +
                          "using UnityEngine;\n\n" +
                          "[CreateAssetMenu(fileName = \"{0}List\", menuName = \"ScriptableObjects/{0}List\", order = 1)]\n" +
                          "public class {0}List : ScriptableObject\n" +
                          "{{\n" +
                          "    public List<{0}> dataList = new List<{0}>();\n" +
                          "}}\n";

        return string.Format(template, className);
    }
}