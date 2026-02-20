using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ShareButton : MonoBehaviour
{
    private string shareMessage;

    public void ShareScore()
    {
        shareMessage = "I just scored " + ScoreManager.Singleton.GetPoints().ToString() + " points in Mole Tapper's " +
                        GameModeManager.Singleton.GetRecentlyPlayedMode().modeName + " mode! Try to beat my score!";

        StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
        ss.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
        ss.Apply();

        string filePath = Path.Combine( Application.temporaryCachePath, "shared img.png" );
        File.WriteAllBytes( filePath, ss.EncodeToPNG() );

        // To avoid memory leaks
        Destroy( ss );

        new NativeShare().AddFile( filePath )
            .SetSubject( "Mole Tapper" ).SetText(shareMessage)
            .SetCallback( (result, share) => Debug.Log(shareMessage) )
            .Share();
    }
}
