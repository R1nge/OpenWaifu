using System.Runtime.InteropServices;
using UnityEngine;

public class OpenCV : MonoBehaviour
{
    //TODO: build a OpenCV library myself, import it as a plugin (QrCode detection)
    [DllImport("opencv")]
    private static extern int TestFunction();

    void Start()
    {
        print(TestFunction());
    }
}