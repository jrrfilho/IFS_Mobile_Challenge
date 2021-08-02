using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOB_Simple_project.Helper
{
    public class CustomButton
    {
        private int imageResId, textSize, pos;
        private string text, color;
        private RectF clickRegion;
        private CustomButtonClickListner listner;
        private Context context;
        private Resources recources;

        public CustomButton(Context context, string text, int textsize, int imageresid, string color, CustomButtonClickListner listner) {
            this.context = context;
            this.text = text;
            this.textSize = textsize;
            this.imageResId = imageresid;
            this.color = color;
            this.listner = listner;
            recources = context.Resources;
        }
        public bool OnClick(float x, float y)
        {
            if(clickRegion != null && clickRegion.Contains(x, y))
            {
                listner.OnClick(pos);
                return true;
            }
            return false;
        }

        public void OnDraw(Canvas c, RectF rectF, int pos)
        {
            Paint p = new Paint();
            p.Color = Color.ParseColor(color);
            c.DrawRect(rectF, p);

            p.Color = Color.White;
            p.TextSize = textSize;

            Rect r = new Rect();
            float cHeight = rectF.Height();
            float cWidth = rectF.Width();
            p.TextAlign = Paint.Align.Center;
            p.GetTextBounds(text, 0, text.Length, r);
            float x = 0, y = 0;
            if(imageResId == 0)
            {
                x = cWidth / 2f - r.Width() / 2f - r.Left;
                y = cHeight / 2f + r.Height() / 2f - r.Bottom;
                c.DrawText(text, rectF.Left + x, rectF.Top + y, p);
            }
            else
            {
                Drawable d = ContextCompat.GetDrawable(context, imageResId);
                Bitmap bitmap = DrawableToBitmap(d);
                c.DrawBitmap(bitmap, (rectF.Left + rectF.Right) / 2, (rectF.Top + rectF.Bottom) / 2, p);
            }
            clickRegion = rectF;
            this.pos = pos;

        }

        private Bitmap DrawableToBitmap(Drawable d)
        {
            if(d is BitmapDrawable)
            {
                return ((BitmapDrawable)d).Bitmap;
            }
            Bitmap bitmap = Bitmap.CreateBitmap(d.IntrinsicWidth, d.IntrinsicHeight, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            d.SetBounds(0, 0, canvas.Width, canvas.Height);
            d.Draw(canvas);
            return bitmap;
        }
    }
}