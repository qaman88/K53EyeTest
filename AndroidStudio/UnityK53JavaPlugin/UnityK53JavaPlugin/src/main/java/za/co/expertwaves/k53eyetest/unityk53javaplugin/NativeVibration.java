package za.co.expertwaves.k53eyetest.unityk53javaplugin;

import android.app.Activity;
import android.content.Context;
import android.os.Build;
import android.os.VibrationEffect;
import android.os.Vibrator;

public class NativeVibration {
  private static final String TAG = "[NativeVibration]";
  private static Activity unityActivity;
  private static Vibrator vibrator;

  public static void SetUnityActivity(Activity activity) {
    unityActivity = activity;
    vibrator = (Vibrator) unityActivity.getSystemService(Context.VIBRATOR_SERVICE);
  }

  public static void Vibrate(long ms_duration) {
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
      vibrator.vibrate(VibrationEffect.createOneShot(ms_duration, VibrationEffect.DEFAULT_AMPLITUDE));
    } else {
      vibrator.vibrate(ms_duration);
    }
  }

  public static void Vibrate(long[] pattern, int count) {
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
      VibrationEffect effect = VibrationEffect.createWaveform(pattern, count);
      vibrator.vibrate(effect);
    }
    else{
      vibrator.vibrate(pattern, count);
    }
  }
}
