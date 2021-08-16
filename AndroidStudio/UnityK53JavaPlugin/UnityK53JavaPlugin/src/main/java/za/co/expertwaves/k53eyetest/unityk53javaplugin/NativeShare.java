package za.co.expertwaves.k53eyetest.unityk53javaplugin;

import android.app.Activity;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

public class NativeShare {
  private static final String TAG = "[UnityAndroidK53]";
  private static Activity unityActivity;

  public static void SetUnityActivity(Activity activity) {
    unityActivity = activity;
  }

  public void Toast(String msg) {
    Toast.makeText(unityActivity, msg, Toast.LENGTH_SHORT).show();
  }

  public void Share(String msg) {
    Log.i(TAG, "Plugin share method called.");
    String extra = msg;
    Intent intent = new Intent(Intent.ACTION_SEND);
    intent.setType("text/plain");
    intent.putExtra(Intent.EXTRA_TEXT, extra);
    unityActivity.startActivity(intent);
  }
}