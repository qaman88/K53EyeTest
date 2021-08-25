package za.co.expertwaves.k53eyetest.unityk53javaplugin;

import android.os.Build;
import android.os.VibrationEffect;
import android.os.Vibrator;

public class NativeVibration {
  private static final String TAG = "[NativeVibration]";
  private static Vibrator vibrator;

  /**
   * The function to vibrate the device default amplitude and provided wavelength.
   * @param wavelength - duration to vibrate the device.
   */
  public static void Vibrate(long wavelength) {
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
      vibrator.vibrate(VibrationEffect.createOneShot(wavelength, VibrationEffect.DEFAULT_AMPLITUDE));
    } else {
      vibrator.vibrate(wavelength);
    }
  }

  /**
   * The function to vibrate the device provided amplitude and wavelength.
   * @param wavelength duration in ms to vibrate the device.
   * @param amplitude amplitude of wavelength of vibration (0 - 255).
   */
  public static void Vibrate(long wavelength, int amplitude) {
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
      vibrator.vibrate(VibrationEffect.createOneShot(wavelength, amplitude));
    } else {
      vibrator.vibrate(wavelength);
    }
  }

  /***
   * The function to vibrate the device using waveforms with amplitudes. Option to repeat is included.
   * The maximum amplitude value is 255.
   * @param wavelengths array of wavelength patterns,
   * @param amplitudes array of wavelengths amplitudes, maximum amplitude value is 255.
   * @param repeat - number of playbacks, it should be -1 for a single shot.
   */
  public static void Vibrate(long[] wavelengths, int[] amplitudes, int repeat) {
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
      VibrationEffect effect = VibrationEffect.createWaveform(wavelengths, amplitudes, repeat);
      vibrator.vibrate(effect);
    }
    else{
      vibrator.vibrate(wavelengths, repeat);
    }
  }
}
