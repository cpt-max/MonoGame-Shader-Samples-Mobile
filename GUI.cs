using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace ShaderSample
{
    class ButtonState
    {
        public Button Button;
        public bool IsPressed;
        public Action<Button> OnPress;
    }

    public enum Click
    {
        Single,
        Continuous,
    }

    public class GUI : FrameLayout
    {
        LinearLayout linearLayout;
        LinearLayout rowLayout;
        List<ButtonState> buttonStates = new();

        public GUI(Context context)
            : base(context)
        {
            linearLayout = new LinearLayout(context);
            linearLayout.Orientation = Orientation.Vertical;
            linearLayout.SetGravity(GravityFlags.Bottom);
            linearLayout.SetPadding(40, 0, 0, 20);
            linearLayout.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.MatchParent);

            AddView(linearLayout);
        }

        public Button AddButton(string name, Click press, Action<Button> onPress)
        {
            var back = new GradientDrawable();
            back.SetColor(Color.Black);
            back.SetCornerRadius(5);
            back.SetStroke(3, Color.White);
            
            var backPress = new GradientDrawable();
            backPress.SetColor(Color.White);
            backPress.SetCornerRadius(5);
            backPress.SetStroke(3, Color.White);

            var buttonBack = new StateListDrawable();
            buttonBack.SetExitFadeDuration(400);
            buttonBack.SetAlpha(75);
            buttonBack.AddState(new int[] { Android.Resource.Attribute.StatePressed }, backPress);
            buttonBack.AddState(new int[] { }, back);

            var b = new Android.Widget.Button(Context);
            b.Text = name;
            b.SetTextColor(Color.White);
            b.SetBackgroundDrawable(buttonBack);
            b.Tag = buttonStates.Count;

            if (press == ShaderSample.Click.Continuous)
                b.Touch += OnTouchButton;
            else
                b.Click += delegate { onPress(b); };

            buttonStates.Add(new ButtonState
            {
                Button = b,
                OnPress = onPress
            });

            if (rowLayout != null)
                rowLayout.AddView(b);
            else
                linearLayout.AddView(b);

            return b;
        }

        public void BeginRow() {
            rowLayout = new LinearLayout(Context);
            linearLayout.AddView(rowLayout);
        }

        public void EndRow()
        {
            rowLayout = null;
        }

        public void Update()
        {
            foreach (var b in buttonStates)
            {
                if (b.IsPressed)
                    b.OnPress(b.Button);
            }
        }

        private void OnTouchButton(object sender, TouchEventArgs e)
        {
            var button = sender as Button;
            int index = (int)button.Tag;
            var state = buttonStates[index];

            if(e.Event.Action == MotionEventActions.Down) 
                state.IsPressed = true;
            if (e.Event.Action == MotionEventActions.Up)
                state.IsPressed = false;

            e.Handled = false;
        }
    }
}
