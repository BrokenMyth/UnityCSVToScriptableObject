using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectAssetGenerator
{
    // 读取每行数据，生成 ScriptableObject 实例，并保存为资产
    public static void GenerateAssetsFromCSV(string csvFilePath, string className,string classListName, string assetFolder)
    {
        // 确保生成资产的路径存在
        if (!Directory.Exists(assetFolder))
        {
            Directory.CreateDirectory(assetFolder);
        }
        // 读取 CSV 文件的内容
        string[] lines = File.ReadAllLines(csvFilePath);
        if (lines.Length <= 1)
        {
            Debug.LogError("CSV 文件没有数据。");
            return;
        }
        // CSV 文件的头部
        string[] headers = lines[0].Split(',');
        // 创建 ClassList 实例
        ScriptableObject ScList = ScriptableObject.CreateInstance(classListName);
        if (ScList == null)
        {
            Debug.LogError("类文件不存在");
           return;
        }
        //填充 ClassList 实例
        FillListWithClassName(ScList, className, lines);
        // 保存为资产
        string assetPath = Path.Combine(assetFolder, $"{className}.asset");
        AssetDatabase.CreateAsset(ScList, assetPath);
        // 刷新 Unity 资源
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObject 资产生成完成。");
    }
    // 通过类名和字段名填充 List<Class> 的方法
    public static void FillListWithClassName(object obj, string className,string[] lines)
    {
        // 获取 obj 的类型
        Type objType = obj.GetType();

        // 获取 List 字段信息
        FieldInfo[] fields = objType.GetFields();
        if (fields.Length > 1)
        {
            Debug.LogError("异常的映射关系");
            return;
        }
        FieldInfo listField = fields[0];
        if (listField != null && listField.FieldType.IsGenericType)
        {
            // 获取 List 实例
            object listInstance = listField.GetValue(obj);
            if (listInstance != null)
            {
                // 根据类名获取 SubClass 的类型
                Type listElementType = Type.GetType(className);
                if (listElementType == null)
                {
                    Console.WriteLine($"类名 {className} 无效，未找到该类型");
                    return;
                } 
                // CSV 文件的头部
                string[] headers = lines[0].Split(',');
                // 遍历每一行，生成 ScriptableObject
                for (int i = 1; i < lines.Length; i++)
                {
                     // 创建 SubClass 实例
                    object subClassInstance = Activator.CreateInstance(listElementType);
                    string[] values = lines[i].Split(',');
                   
                    // 通过反射设置每个属性的值
                    for (int j = 0; j < headers.Length; j++)
                    {
                        var field = listElementType.GetField(headers[j]);
                        if (field != null)
                        {
                            var convertedValue = Convert.ChangeType(values[j], field.FieldType);
                            field.SetValue(subClassInstance, convertedValue);
                        }
                    }
                    // 通过反射获取 List 的 Add 方法
                    MethodInfo addMethod = listField.FieldType.GetMethod("Add");

                    // 调用 Add 方法将填充好的 SubClass 实例添加到 List 中
                    addMethod.Invoke(listInstance, new object[] { subClassInstance });
                }
            }
            else
            {
                Console.WriteLine("List 实例未找到或为 null");
            }
        }
        else
        {
            Console.WriteLine($"该字段不是一个泛型列表");
        }
    }
}