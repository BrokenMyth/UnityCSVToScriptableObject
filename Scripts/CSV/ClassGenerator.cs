using System.IO;
using UnityEditor;
using UnityEngine;

public static class ClassGenerator
{
    // 生成 C# 类文件
    public static void GenerateClassFile(string className, string[] headers, string outputPath)
    {
        string classContent = GenerateClassContent(className, headers);
        string fullPath = Path.Combine(outputPath, className + ".cs");

        // 写入类文件
        File.WriteAllText(fullPath, classContent);
        Debug.Log("Class file generated at: " + fullPath);

        // 刷新Unity资源
        AssetDatabase.Refresh();
    }

    // 根据 CSV 的头部信息动态生成类内容
    private static string GenerateClassContent(string className, string[] v)
    {
        string[] headers = v[0].Split(',');
        string[] rows = v[1].Split(',');
        string classTemplate = "using System;\n" +
                               "[System.Serializable]\n" +
                               "public class {0}\n" +
                               "{{\n{1}\n}}";

        string fieldTemplate = "    public {0} {1};";

        string fields = "";
        int i = 0;
        foreach (var header in headers)
        {
            if (header=="")continue;
            string fieldType = GuessFieldType(rows[i++]); // 猜测字段类型
            fields += string.Format(fieldTemplate, fieldType, header) + "\n";
        }

        return string.Format(classTemplate, className, fields);
    }

    // 猜测 CSV 字段的数据类型，可以根据更复杂的情况扩展
    private static string GuessFieldType(string header)
    {
        float result;
        int i;
        // 默认返回 string 类型，可以根据字段名或其他规则扩展
        if (float.TryParse(header, out result))
        {
            return "float";
        }else if (int.TryParse(header, out i))
        {
            return "int";
        }
        return "string";
    }
}