using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVToScriptableObjectMain
{
    static string className = "PlayerGrade";
    static string csvFilePath = "Assets/CSV/PlayerGrades.csv"; // CSV 文件路径
    static string classOutputPath = "Assets/Scripts/CSV/GeneratedClasses"; // 类文件生成路径
    static string assetFolder = "Assets/Scripts/CSV/GeneratedAssets"; // ScriptableObject 资产路径
    
    [MenuItem("Tools/Generate Class from CSV")]
    public static void GenerateClassFromCSV()
    {
        string path = EditorUtility.OpenFilePanel("Select CSV file", "", "csv");

        if (string.IsNullOrEmpty(path))
            return;
        csvFilePath = path;
        // 提取文件名
        className = Path.GetFileNameWithoutExtension(path);
        // 确保输出文件夹存在
        if (!Directory.Exists(classOutputPath))
        {
            Directory.CreateDirectory(classOutputPath);
        }
        if (!Directory.Exists(assetFolder))
        {
            Directory.CreateDirectory(assetFolder);
        }
        // 读取 CSV 文件
        string[] lines = File.ReadAllLines(csvFilePath);
        if (lines.Length == 0)
        {
            Debug.LogError("CSV 文件为空。");
            return;
        }

        //  CSV 文件的字段推导
        ClassGenerator.GenerateClassFile(className, lines, classOutputPath);
        // 生成 ScriptableObject 类文件
        ClassListGenerator.GenerateScriptableObjectFile(className, classOutputPath);
        
    }
    
    [MenuItem("Tools/Generate ScriptableObjects from CSV")]
    public static void GenerateScriptableObjectsFromCSV()
    {
        string path = EditorUtility.OpenFilePanel("Select CSV file", "", "csv");

        if (string.IsNullOrEmpty(path))
            return;
        csvFilePath = path;
        // 提取文件名
        className =Path.GetFileNameWithoutExtension(path);
        // 确保输出文件夹存在
        if (!Directory.Exists(classOutputPath))
        {
            Directory.CreateDirectory(classOutputPath);
        }
        if (!Directory.Exists(assetFolder))
        {
            Directory.CreateDirectory(assetFolder);
        }

        // 读取 CSV 文件的第一行
        string[] lines = File.ReadAllLines(csvFilePath);
        if (lines.Length == 0)
        {
            Debug.LogError("CSV 文件为空。");
            return;
        }

        // 获取 CSV 文件的头部字段
        string[] headers = lines[0].Split(',');
        // 根据 CSV 每行生成 ScriptableObject 实例并保存为资产
        ScriptableObjectAssetGenerator.GenerateAssetsFromCSV(csvFilePath, className,className+"List", assetFolder);

        Debug.Log("Class and ScriptableObject generation completed.");
    }
}