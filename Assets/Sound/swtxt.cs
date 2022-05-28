using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class swtxt : MonoBehaviour
{
    [SerializeField] string fileName ="uuu";
    [SerializeField] Text txtbox;
    string [] txtarray;
    string filepath;
    // Start is called before the first frame update
    void Start()
    {
        filepath = Application.dataPath +"/"+fileName;
    }


    public void ReadFromtheFile(){
        txtarray = File.ReadAllLines(filepath);
        foreach(string line in txtarray){
        print(line);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
