using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class BoxBorderEntry : Entry
    {
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create("IsValid", typeof(bool), typeof(BoxBorderEntry), true);
        public static readonly BindableProperty NormalBorderColorProperty =
            BindableProperty.Create("NormalBorderColor", typeof(Color), typeof(BoxBorderEntry), Color.Default);
        public static readonly BindableProperty ErrorBorderColorProperty =
            BindableProperty.Create("ErrorBorderColor", typeof(Color), typeof(BoxBorderEntry), Color.Default);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        public Color NormalBorderColor
        {
            get => (Color)GetValue(NormalBorderColorProperty);
            set => SetValue(NormalBorderColorProperty, value);
        }

        public Color ErrorBorderColor
        {
            get => (Color)GetValue(ErrorBorderColorProperty);
            set => SetValue(ErrorBorderColorProperty, value);
        }

        public BoxBorderEntry()
        {
        }
    }
}
