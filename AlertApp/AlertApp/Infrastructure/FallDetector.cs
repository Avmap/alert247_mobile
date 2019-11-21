using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class FallDetector
    {
        static readonly object _locker = new object();
        #region SETTINGS

        public const double LYING_AVERAGE_Z_LPF = 0.5;
        public const int INTERVAL_MS = 20;
        public const int DURATION_S = 10;
        public const int N = DURATION_S * 1000 / INTERVAL_MS;
        public const int OFFSET_X = N * 0;
        public const int OFFSET_Y = N * 1;
        public const int OFFSET_Z = N * 2;
        public const double G = 1.0;
        #endregion

        public const double ASENSOR_STANDARD_GRAVITY = 9.80665;
        public const int SIZE_BUFFER = N * 19;
        public const int FILTER_NZEROS = 2;
        public const int FILTER_NPOLES = 2;
        public const double FALLING_WAIST_SV_TOT = 0.6;
        public const double IMPACT_WAIST_SV_TOT = 2.0;
        public const double IMPACT_WAIST_SV_D = 1.7;
        public const double IMPACT_WAIST_SV_MAXMIN = 2.0;
        public const double IMPACT_WAIST_Z_2 = 1.5;
        public const int OFFSET_X_LPF = N * 3;
        public const int OFFSET_Y_LPF = N * 4;
        public const int OFFSET_Z_LPF = N * 5;
        public const int OFFSET_X_HPF = N * 6;
        public const int OFFSET_Y_HPF = N * 7;
        public const int OFFSET_Z_HPF = N * 8;

        public const int OFFSET_FALLING = N * 16;
        public const int OFFSET_IMPACT = N * 17;
        public const int OFFSET_LYING = N * 18;


        private const double FILTER_LPF_GAIN = 4.143204922e+03;
        private const double FILTER_FACTOR_0 = -0.9565436765;
        private const double FILTER_FACTOR_1 = +1.9555782403;
        private const double FILTER_HPF_GAIN = 1.022463023e+00;

        public const int OFFSET_SV_TOT = N * 12;
        public const int OFFSET_SV_D = N * 13;
        public const int OFFSET_SV_MAXMIN = N * 14;
        public const int OFFSET_Z_2 = N * 15;

        public const int OFFSET_X_D = N * 9;
        public const int OFFSET_Y_D = N * 10;
        public const int OFFSET_Z_D = N * 11;

        private const int SPAN_MAXMIN = (100 / INTERVAL_MS);



        private int SPAN_FALLING = (1000 / INTERVAL_MS);
        private int SPAN_IMPACT = (2000 / INTERVAL_MS);
        private double SPAN_AVERAGING = (400 / INTERVAL_MS);

        private StateStructure State = new StateStructure();
        private IFallDetectionListener _FallDetectionListener;

        private static int Position = 0;

        public interface IFallDetectionListener
        {
            void OnFallDetected();
        }

        public FallDetector(IFallDetectionListener fallDetectionListener)
        {
            _FallDetectionListener = fallDetectionListener;
        }

        public static int GetPosition()
        {
            lock (_locker)
            {
                return Position;
            }
        }

        private double SV(double X, double Y, double Z)
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        private int EXPIRE(int Timeout)
        {
            return (Timeout > -1) ? Timeout - 1 : -1;
        }

        private double LINEAR(long Before, double Ante, double After, double Post, long Now)
        {
            return (Ante + (Post - Ante) * (double)(Now - Before) / (double)(After - Before));
        }



        private void Fill(double[] Array, int Offset, int Length, double Value)
        {
            for (int I = Offset; I < Offset + Length; I++)
            {
                Array[I] = Value;
            }
        }

        private bool isnan(double value)
        {
            return Double.IsNaN(value);
        }

        // Low-pass Butterworth filter, 2nd order, 50 Hz sampling rate, corner frequency 0.25 Hz
        private double LPF(double Value, double[] XV, double[] YV)
        {
            XV[0] = XV[1];
            XV[1] = XV[2];
            XV[2] = Value / FILTER_LPF_GAIN;
            YV[0] = YV[1];
            YV[1] = YV[2];
            YV[2] = (XV[0] + XV[2]) + 2 * XV[1] + (FILTER_FACTOR_0 * YV[0]) + (FILTER_FACTOR_1 * YV[1]);
            return YV[2];
        }

        // High-pass Butterworth filter, 2nd order, 50 Hz sampling rate, corner frequency 0.25 Hz
        private double HPF(double Value, double[] XV, double[] YV)
        {
            XV[0] = XV[1];
            XV[1] = XV[2];
            XV[2] = Value / FILTER_HPF_GAIN;
            YV[0] = YV[1];
            YV[1] = YV[2];
            YV[2] = (XV[0] + XV[2]) - 2 * XV[1] + (FILTER_FACTOR_0 * YV[0]) + (FILTER_FACTOR_1 * YV[1]);
            return YV[2];
        }



        void InitiateBuffer(StateStructure State)
        {
            State.TimeoutImpact = -1;
            State.TimeoutFalling = -1;
            Position = 0;
        }

        void InitiateSamples(StateStructure State)
        {
            State.X = new double[SIZE_BUFFER];
            State.Y = new double[SIZE_BUFFER];
            State.Z = new double[SIZE_BUFFER];
        }

        void InitiateResampling(StateStructure State)
        {
            State.AnteX = State.AnteY = State.AnteZ = Double.NaN;
            State.AnteTime = 0;
        }

        void InitiateFiltering(StateStructure State)
        {
            //State.X_LPF = Double.NaN;
            // State.Y_LPF = Double.NaN;
            State.Z_LPF = new double[SIZE_BUFFER];

            State.X_HPF = new double[SIZE_BUFFER];
            State.Y_HPF = new double[SIZE_BUFFER];
            State.Z_HPF = new double[SIZE_BUFFER];

            State.XLPFXV = new double[FILTER_NZEROS + 1];
            State.XLPFYV = new double[FILTER_NZEROS + 1];
            State.YLPFXV = new double[FILTER_NZEROS + 1];
            State.YLPFYV = new double[FILTER_NZEROS + 1];
            State.ZLPFXV = new double[FILTER_NZEROS + 1];
            State.ZLPFYV = new double[FILTER_NZEROS + 1];

            State.XHPFXV = new double[FILTER_NZEROS + 1];
            State.XHPFYV = new double[FILTER_NZEROS + 1];
            State.YHPFXV = new double[FILTER_NZEROS + 1];
            State.YHPFYV = new double[FILTER_NZEROS + 1];
            State.ZHPFXV = new double[FILTER_NZEROS + 1];
            State.ZHPFYV = new double[FILTER_NZEROS + 1];

            Fill(State.XLPFXV, 0, FILTER_NZEROS + 1, 0);
            Fill(State.XLPFYV, 0, FILTER_NPOLES + 1, 0);
            Fill(State.YLPFXV, 0, FILTER_NZEROS + 1, 0);
            Fill(State.YLPFYV, 0, FILTER_NPOLES + 1, 0);
            Fill(State.ZLPFXV, 0, FILTER_NZEROS + 1, 0);
            Fill(State.ZLPFYV, 0, FILTER_NPOLES + 1, 0);
            Fill(State.XHPFXV, 0, FILTER_NZEROS + 1, 0);
            Fill(State.XHPFYV, 0, FILTER_NPOLES + 1, 0);
            Fill(State.YHPFXV, 0, FILTER_NZEROS + 1, 0);
            Fill(State.YHPFYV, 0, FILTER_NPOLES + 1, 0);
            Fill(State.ZHPFXV, 0, FILTER_NZEROS + 1, 0);
            Fill(State.ZHPFYV, 0, FILTER_NPOLES + 1, 0);
        }

        void InitiateDeltas(StateStructure State)
        {
            State.X_MAXMIN = new double[SIZE_BUFFER];
            State.Y_MAXMIN = new double[SIZE_BUFFER];
            State.Z_MAXMIN = new double[SIZE_BUFFER];
        }

        void InitiateSV(StateStructure State)
        {
            State.SV_TOT = new double[SIZE_BUFFER]; ;
            State.SV_D = new double[SIZE_BUFFER];
            State.SV_MAXMIN = new double[SIZE_BUFFER];
            State.Z_2 = new double[SIZE_BUFFER];
        }

        void InitiateEvents(StateStructure State)
        {
            //  State.Falling = new List<double>();
            // State.Impact = new List<double>();
            //State.Lying = new List<double>();
        }

        public void Initiate()
        {
            if (State != null)
            {
                InitiateBuffer(State);
                InitiateSamples(State);
                InitiateResampling(State);
                InitiateFiltering(State);
                InitiateDeltas(State);
                InitiateSV(State);
                InitiateEvents(State);
            }
        }

        public void Protect(long timestamp, double x, double y, double z)
        {
            lock (_locker)
            {
                Sampled(timestamp, x, y, z);
            }
        }
        void Sampled(long timestamp, double x, double y, double z)
        {
            long PostTime = timestamp / 1000000;
            double PostX = x / ASENSOR_STANDARD_GRAVITY;
            double PostY = y / ASENSOR_STANDARD_GRAVITY;
            double PostZ = z / ASENSOR_STANDARD_GRAVITY;
            Resample(PostTime, PostX, PostY, PostZ);
            State.AnteTime = PostTime;
            State.AnteX = PostX;
            State.AnteY = PostY;
            State.AnteZ = PostZ;
        }

        // Android sampling is irregular, thus the signal is (linearly) resampled at 50 Hz
        void Resample(long PostTime, double PostX, double PostY, double PostZ)
        {
            if (0 == State.AnteTime)
            {
                State.Regular = PostTime + INTERVAL_MS;
            }
            while (State.Regular < PostTime)
            {
                AddToQueue(State.X, LINEAR(State.AnteTime, State.AnteX, PostTime, PostX, State.Regular));
                AddToQueue(State.Y, LINEAR(State.AnteTime, State.AnteY, PostTime, PostY, State.Regular));
                AddToQueue(State.Z, LINEAR(State.AnteTime, State.AnteZ, PostTime, PostZ, State.Regular));
                Process();
                SetPosition();
                State.Regular += INTERVAL_MS;
            }
        }

        void Process()
        {
            State.TimeoutFalling = EXPIRE(State.TimeoutFalling);
            State.TimeoutImpact = EXPIRE(State.TimeoutImpact);


            AddToQueue(State.Z_LPF, LPF(State.Z[Position], State.ZLPFXV, State.ZLPFYV));

            AddToQueue(State.X_HPF, HPF(State.X[Position], State.XHPFXV, State.XHPFYV));
            AddToQueue(State.Y_HPF, HPF(State.Y[Position], State.YHPFXV, State.YHPFYV));
            AddToQueue(State.Z_HPF, HPF(State.Z[Position], State.ZHPFXV, State.ZHPFYV));

            AddToQueue(State.X_MAXMIN, Max(State.X) - Min(State.X));
            AddToQueue(State.Y_MAXMIN, Max(State.Y) - Min(State.Y));
            AddToQueue(State.Z_MAXMIN, Max(State.Z) - Min(State.Z));

            AddToQueue(State.SV_TOT, SV(State.X[Position], State.Y[Position], State.Z[Position]));
            double SV_TOT = State.SV_TOT[Position];

            AddToQueue(State.SV_D, SV(State.X_HPF[Position], State.Y_HPF[Position], State.Z_HPF[Position]));
            double SV_D = State.SV_D[Position];

            AddToQueue(State.SV_MAXMIN, SV(State.X_MAXMIN[Position], State.Y_MAXMIN[Position], State.Z_MAXMIN[Position]));
            AddToQueue(State.Z_2, (SV_TOT * SV_TOT - SV_D * SV_D - G * G) / (2.0 * G));

            double SV_TOT_BEFORE = Position == 0 ? State.SV_TOT[0] : State.SV_TOT[(Position - 1)];

            //  State.Falling.Add(0);
            //Debug.WriteLine($"Before: X: {SV_TOT_BEFORE}, NOW: {SV_TOT}");
            if (FALLING_WAIST_SV_TOT <= SV_TOT_BEFORE && SV_TOT < FALLING_WAIST_SV_TOT)
            {
                //Debug.WriteLine($"Start");
                State.TimeoutFalling = SPAN_FALLING;
                //State.Falling[State.Falling.Count - 1] = 1;
            }
            //State.Impact.Add(0);
            if (-1 < State.TimeoutFalling)
            {
                double SV_MAXMIN = State.SV_MAXMIN[Position];
                double Z_2 = State.Z_2[Position];
                if (IMPACT_WAIST_SV_TOT <= SV_TOT || IMPACT_WAIST_SV_D <= SV_D ||
                    IMPACT_WAIST_SV_MAXMIN <= SV_MAXMIN || IMPACT_WAIST_Z_2 <= Z_2)
                {
                    State.TimeoutImpact = SPAN_IMPACT;
                    //State.Impact[State.Impact.Count - 1] = 1;
                }
            }
            //  State.Lying.Add(0);
            if (0 == State.TimeoutImpact)
            {
                int I;
                double Sum = 0, Count = 0;
                for (I = 0; I < SPAN_AVERAGING; I++)
                {
                    double Value = State.Z_LPF[Position];
                    if (!isnan(Value))
                    {
                        Sum += Value;
                        Count += 1;
                    }
                }
                if (LYING_AVERAGE_Z_LPF < (Sum / Count))
                {
                    if (_FallDetectionListener != null)
                    {
                        Device.BeginInvokeOnMainThread(() => _FallDetectionListener.OnFallDetected());
                        //_FallDetectionListener.OnFallDetected();
                    }
                    Debug.WriteLine($"Fall!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    // State.Lying[State.Lying.Count - 1] = 1;
                }
            }
        }
        private void AddToQueue(double[] array, double Value)
        {
            array[Position] = Value;
        }
        //private void AddToQueue(Queue<Double> queue, double Value)
        //{
        //    if (queue.Count >= SIZE_BUFFER)
        //    {
        //        queue.Dequeue();
        //        queue.Enqueue(Value);
        //    }
        //    else
        //    {
        //        queue.Enqueue(Value);
        //    }
        //}
        double Min(Queue<double> Array)
        {
            var index = Array.Count() - SPAN_MAXMIN;
            if (index < 0)
            {
                index = 0;
            }
            var beforeValues = Array.Skip(index);
            return beforeValues.Min();
        }

        double Min(double[] Array)
        {
            int I;
            double Min = AT(Array, Position, N);
            for (I = 1; I < SPAN_MAXMIN; I++)
            {
                double Value = AT(Array, Position - I, N);
                if (!isnan(Value) && Value < Min)
                {
                    Min = Value;
                }
            }
            return Min;
        }

        double AT(double[] Array, int Index, int Size)
        {
            return (Array[(Index + Size) % Size]);
        }

        double Max(double[] Array)
        {
            int I;
            double Max = AT(Array, Position, N);
            for (I = 1; I < SPAN_MAXMIN; I++)
            {
                double Value = AT(Array, Position - I, N);
                if (!isnan(Value) && Max < Value)
                {
                    Max = Value;
                }
            }
            return Max;
        }

        private void SetPosition()
        {
            Position = (Position + 1) % N;
            //System.Diagnostics.Debug.WriteLine($"Position = " + Position.ToString());
        }

        public class SignalData
        {
            public double[] Buffer { get; set; }
            public int Position { get; set; }
        }

        public class StateStructure
        {
            //public double[] Buffer { get; set; }
            public int TimeoutFalling { get; set; }
            public int TimeoutImpact { get; set; }

            public double[] X;
            public double[] Y;
            public double[] Z;
            //public double X_LPF;
            //  public double Y_LPF;
            public double[] Z_LPF;
            public double[] X_HPF;
            public double[] Y_HPF;
            public double[] Z_HPF;
            public double[] X_MAXMIN;
            public double[] Y_MAXMIN;
            public double[] Z_MAXMIN;
            public double[] SV_TOT;
            public double[] SV_D;
            public double[] SV_MAXMIN;
            public double[] Z_2;
            //public List<Double> Falling;
            // public List<Double> Impact;
            //public List<Double> Lying;
            public double[] XLPFXV { get; set; }
            public double[] XLPFYV { get; set; }
            public double[] YLPFXV { get; set; }
            public double[] YLPFYV { get; set; }
            public double[] ZLPFXV { get; set; }
            public double[] ZLPFYV { get; set; }
            public double[] XHPFXV { get; set; }
            public double[] XHPFYV { get; set; }
            public double[] YHPFXV { get; set; }
            public double[] YHPFYV { get; set; }
            public double[] ZHPFXV { get; set; }
            public double[] ZHPFYV { get; set; }
            public double AnteX { get; set; }
            public double AnteY { get; set; }
            public double AnteZ { get; set; }
            public long AnteTime { get; set; }
            public long Regular { get; set; }
        }
    }
}
