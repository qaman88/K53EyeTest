package za.co.expertwaves.k53eyetest.unityk53javaplugin;

import android.content.Context;
import android.os.Build;
import android.os.Bundle;
import android.os.Debug;
import android.os.VibrationEffect;
import android.os.Vibrator;
import android.util.Log;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;

public class MainActivity extends AppCompatActivity {
  private static Vibrator vibrator;
  @Override
  protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    setContentView( R.layout.activity_main);
    vibrator = (Vibrator) this.getSystemService(Context.VIBRATOR_SERVICE);
  }

  public void OnButtonClick(View view) {
    Log.i("$$$ MainActivity", "Button Clicked.");
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
     // works/ vibrator.vibrate(VibrationEffect.createOneShot(100, VibrationEffect.DEFAULT_AMPLITUDE));


      // wave test
      long[] mVibratePattern = new long[]{0, 40, 80, 60, 80, 100};
      VibrationEffect effect = VibrationEffect.createWaveform(mVibratePattern, -1);
      vibrator.vibrate(effect);
    }
    else{
      // works but deprecated // vibrator.vibrate(100);
    }

  }
}