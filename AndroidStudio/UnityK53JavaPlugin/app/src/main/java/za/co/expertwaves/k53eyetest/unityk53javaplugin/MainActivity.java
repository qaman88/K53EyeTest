package za.co.expertwaves.k53eyetest.unityk53javaplugin;

import android.content.Context;
import android.os.Build;
import android.os.Bundle;
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
    setContentView(R.layout.activity_main);
    vibrator = (Vibrator) this.getSystemService(Context.VIBRATOR_SERVICE);
  }

  public void OnButtonClick(View view) {
    Log.i("$$$ MainActivity", "Button Clicked.");
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
      // wave test
      long[] waves = new long[]{100, 100, 100, 100, 100};
      int[] amplitudes = new int[]{50, 0, 200, 0, 50};
      VibrationEffect effect = VibrationEffect.createWaveform(waves, amplitudes, -1);
      vibrator.vibrate(effect);
    } else {
      vibrator.vibrate(100);
    }
  }
}