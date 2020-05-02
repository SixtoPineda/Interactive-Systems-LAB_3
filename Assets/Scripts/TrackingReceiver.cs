using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using System.Linq;
using System.Diagnostics;

public class TrackingReceiver : MonoBehaviour
{
    //GameObjects to be controlled with Posenet
    public GameObject nose; //Nariz
    public GameObject wristR; //Mano derecha
    public GameObject wristL; //Mano izq.
    public GameObject background;

    //OSC Variables
    private OSCReceiver _receiver;
    private const string _oscAddress = "/pose/0";
    private player_control player_c;

    //Dictionary to store pose data
    public Dictionary<string, Vector3> pose = new Dictionary<string, Vector3>();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Set up OSC receiver
        StartOSCReceiver();

        //Initialize pose
        StartPose();
    }

    void StartOSCReceiver() {
        // Creating a receiver.
        _receiver = gameObject.AddComponent<OSCReceiver>();

        // Set local port.
        _receiver.LocalPort = 9876;

        // Bind "MessageReceived" method to special address.
        _receiver.Bind(_oscAddress, MessageReceived);
    }

    void StartPose() {//colocamos todos los objetos en 0
        pose.Add("nose", Vector3.zero);
        pose.Add("rightWrist", Vector3.zero);
        pose.Add("leftWrist", Vector3.zero);
    }
    

    // Update is called once per frame
    void Update()
    {
        nose.transform.position = pose["nose"];
        wristR.transform.position = pose["rightWrist"];
        wristL.transform.position = pose["leftWrist"];
        float XposL = wristL.transform.position.x;
        float YposL = wristL.transform.position.y;

        float XposR = wristR.transform.position.x;
        float YposR = wristR.transform.position.y;

        float r = ((XposL + XposR) / 2);
        float g = ((YposL + YposR) / 2);
        float b = (XposL + YposR) / 2;
        if (r <= 0 && g<=0 && b<=0)
        {
            background.GetComponent<Renderer>().material.color = new Color(0.49f,0.22f,0.88f);
        }
        else
        {
            background.GetComponent<Renderer>().material.color = new Color(r, g, b); //cambiamos el color del fondo según el movimiento de nuestras manos
        }

        
    }

    protected void MessageReceived(OSCMessage message)
    {
        List<OSCValue> list = message.Values;
        //UnityEngine.Debug.Log(list.Count);

        for(int i=0;i<list.Count; i+=3)
        {
            string key = "";
            Vector2 position = Vector3.zero; 

            OSCValue val0 = list.ElementAt(i);
            if (val0.Type == OSCValueType.String) key = val0.StringValue;
            OSCValue val1 = list.ElementAt(i+1);
            if (val1.Type == OSCValueType.Float) position.x = val1.FloatValue-250;
            OSCValue val2 = list.ElementAt(i+2);
            if (val2.Type == OSCValueType.Float) position.y = -(val2.FloatValue-250);

            if (pose.ContainsKey(key)) {
                pose[key] = position; 
            }
        }

    }

}
