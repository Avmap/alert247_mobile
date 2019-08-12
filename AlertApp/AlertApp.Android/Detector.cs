using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlertApp.Droid
{
    public class Detector
    {
        //static Detector()
        //{
        //    Java.Lang.JavaSystem.LoadLibrary("detector");
        //}
        public readonly static int INTERVAL_MS = 20;
        public readonly static int DURATION_S = 10;
        public readonly static int N = DURATION_S * 1000 / INTERVAL_MS;
        public readonly static int OFFSET_X = N * 0;
        public readonly static int OFFSET_Y = N * 1;
        public readonly static int OFFSET_Z = N * 2;
        public readonly static int OFFSET_X_LPF = N * 3;
        public readonly static int OFFSET_Y_LPF = N * 4;
        public readonly static int OFFSET_Z_LPF = N * 5;
        public readonly static int OFFSET_X_HPF = N * 6;
        public readonly static int OFFSET_Y_HPF = N * 7;
        public readonly static int OFFSET_Z_HPF = N * 8;
        public readonly static int OFFSET_X_D = N * 9;
        public readonly static int OFFSET_Y_D = N * 10;
        public readonly static int OFFSET_Z_D = N * 11;
        public readonly static int OFFSET_SV_TOT = N * 12;
        public readonly static int OFFSET_SV_D = N * 13;
        public readonly static int OFFSET_SV_MAXMIN = N * 14;
        public readonly static int OFFSET_Z_2 = N * 15;
        public readonly static int OFFSET_FALLING = N * 16;
        public readonly static int OFFSET_IMPACT = N * 17;
        public readonly static int OFFSET_LYING = N * 18;
        public readonly static int SIZE = N * 19;
        public readonly static double FALLING_WAIST_SV_TOT = 0.6;
        public readonly static double IMPACT_WAIST_SV_TOT = 2.0;
        public readonly static double IMPACT_WAIST_SV_D = 1.7;
        public readonly static double IMPACT_WAIST_SV_MAXMIN = 2.0;
        public readonly static double IMPACT_WAIST_Z_2 = 1.5;

        [DllImport("detector", EntryPoint = "openSerialPort")]
        public static extern void initiate(Context context);

        public extern static void acquire();

        public extern static double[] buffer();

        public extern static int position();

        public extern static void release();

        public static void call(Context context)
        {
           // Alarm.call(context);
        }
    }
}
