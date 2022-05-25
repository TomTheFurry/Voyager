using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangSystem
{
    // FIXME: Potencial path excape attack.
    // Therefore both activeLangSet and file param is not safe!
    public const string langPath = "Lang/";
    // Dictionary of the sub-file path, then the key, then the value
    public static Dictionary<string, Dictionary<string, string>> activeLang = null;
    private static string activeLangSet = "";

    public static void LoadLang(string lang)
    {
        if (lang == activeLangSet)
            return;
        Debug.Log("Loading language: " + lang);
        activeLangSet = lang;
        activeLang = new Dictionary<string, Dictionary<string, string>>();
    }

    private static string _ErrorLang(string error, string file, string key)
    {
        return error + "!" + file + "/" + key + "!";
    }

    public static string GetLang(string file, string key)
    {
        if (activeLang == null) return _ErrorLang("NOLANG", file, key);
        if (!LoadSubFile(file)) return _ErrorLang("NOFILE", file, key);
        //if (!activeLang[file].ContainsKey(key)) return _ErrorLang("NOKEY", file, key);
        return activeLang[file][key];
    }

    public static bool LoadSubFile(string file)
    {
        if (activeLang == null)
            return false;
        if (activeLang.ContainsKey(file))
            return true;
        Debug.Log("Loading lang file: " + langPath + activeLangSet + "/" + file + ".txt");
        TextAsset textAsset = Resources.Load<TextAsset>(langPath + activeLangSet + "/" + file);
        if (textAsset == null)
        {
            Debug.LogError("Lang file not found: " + file + ".txt");
            return false;
        }
        Dictionary<string, string> subFile = new Dictionary<string, string>();
        // Format:
        // `~KEY
        // VALUE
        // `~KEY
        // VALUE
        // VALUE
        // `~KEY
        // ...

        string[] lines = textAsset.text.Split('\n');
        string currentKey = "";
        string currentValue = "";
        foreach (string line in lines)
        {
            if (line.StartsWith("`~"))
            {
                if (currentKey != "")
                {
                    subFile.Add(currentKey, currentValue);
                }
                currentKey = line.Substring(2).Trim();
                currentValue = "";
            }
            else
            {
                if (currentValue != "") currentValue += "\n";
                currentValue += line;
            }
        }
        if (currentKey != "")
        {
            subFile.Add(currentKey, currentValue);
        }
        Debug.Log("Loaded lang file: " + file + ".txt with " + subFile.Count + " entries.");
        foreach (KeyValuePair<string, string> entry in subFile)
        {
            Debug.Log(entry.Key + ": " + entry.Value);
        }
        activeLang[file] = subFile;
        return true;
    }

    // turn all $lang/file/key$ into the value
    public static string parseText(string text)
    {
        while (text.Contains("$lang/"))
        {
            int start = text.IndexOf("$lang/");
            int end = text.IndexOf("$", start + 6);
            if (end == -1) {
                Debug.LogWarning("Missing '$' in the end of '$lang' tag! Ignoring...");
                break;
            }

            int fileEnd = text.IndexOf("/", start + 6);
            string file = text.Substring(start + 6, fileEnd - start - 6);
            string key = text.Substring(fileEnd + 1, end - fileEnd - 1);
            text = text.Replace("$lang/" + file + "/" + key + "$", GetLang(file, key));
        }
        return text;
    }
}
