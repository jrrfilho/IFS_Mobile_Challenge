using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOB_Simple_project.Helper
{
    public abstract class SwipeHelper : ItemTouchHelper.SimpleCallback
    {
        int buttonWidth, swipePosition = -1;
        float swipeTheshold = 0.5f;
        Dictionary<int, List<CustomButton>> buttonBuffer;
        Queue<int> removerQueue = new Queue<int>();
        GestureDetector.SimpleOnGestureListener gestureListener;
        View.IOnTouchListener onTouchListener;
        RecyclerView recyclerView;
        List<CustomButton> buttonList;
        GestureDetector gestureDetector;

        public abstract void InstanciateCustomButtom(RecyclerView.ViewHolder viewHolder, List<CustomButton> buffer);
        public SwipeHelper(Context context, RecyclerView recyclerView, int buttonWidth) : base(0, ItemTouchHelper.Left)
        {
            this.recyclerView = recyclerView;
            this.buttonList = new List<CustomButton>();
            this.buttonBuffer = new Dictionary<int, List<CustomButton>>();
            this.buttonWidth = buttonWidth;

            gestureListener = new CustomGestureListner(this);
            onTouchListener = new CustomOnTouchListener(this);

            this.gestureDetector = new GestureDetector(context, gestureListener);
            this.recyclerView.SetOnTouchListener(onTouchListener);

            AttachSwipe();
        }

        private void AttachSwipe()
        {
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(this);
            itemTouchHelper.AttachToRecyclerView(recyclerView);
        }

        private class CustomGestureListner : GestureDetector.SimpleOnGestureListener
        {
            private SwipeHelper swipeHelper;

            public CustomGestureListner(SwipeHelper swipeHelper)
            {
                this.swipeHelper = swipeHelper;
            }

            public override bool OnSingleTapUp(MotionEvent e)
            {
                foreach (CustomButton cb in swipeHelper.buttonList)
                {
                    if (cb.OnClick(e.GetX(), e.GetY()))
                    {
                        break;
                    }
                }
                return true;
            }
        }

        private class CustomOnTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            private SwipeHelper swipeHelper;
            public CustomOnTouchListener(SwipeHelper swipeHelper)
            {
                this.swipeHelper = swipeHelper;
            }
            public bool OnTouch(View v, MotionEvent e)
            {
                if (swipeHelper.swipePosition < 0) return false;
                Android.Graphics.Point point = new Android.Graphics.Point((int)e.RawX, (int)e.RawY);

                RecyclerView.ViewHolder viewHolder = swipeHelper.recyclerView.FindViewHolderForAdapterPosition(swipeHelper.swipePosition);
                View itemSwiped = viewHolder.ItemView;
                Rect rect = new Rect();
                itemSwiped.GetGlobalVisibleRect(rect);
                if(e.Action == MotionEventActions.Down || e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Move)
                {
                    if(rect.Top < point.Y && rect.Bottom > point.Y)
                    {
                        swipeHelper.gestureDetector.OnTouchEvent(e);
                    }
                    else
                    {
                        swipeHelper.removerQueue.Enqueue(swipeHelper.swipePosition);
                        swipeHelper.swipePosition = -1;
                        swipeHelper.RecoverSwipedItem();
                    }
                }
                return false;
            }
        }

        private void RecoverSwipedItem()
        {
            while(removerQueue.Count > 0)
            {
                int pos = removerQueue.Dequeue();
                if(pos > -1)
                {
                    recyclerView.GetAdapter().NotifyItemChanged(pos);

                }
            }
        }

        //Overide
        public override bool OnMove(RecyclerView p0, RecyclerView.ViewHolder p1, RecyclerView.ViewHolder p2)
        {
            return false;
        }

        public override float GetSwipeThreshold(RecyclerView.ViewHolder viewHolder)
        {
            return swipeTheshold;
        }

        public override float GetSwipeEscapeVelocity(float defaultValue)
        {
            return 0.1f * defaultValue;
        }

        public override float GetSwipeVelocityThreshold(float defaultValue)
        {
            return 5.0f *  defaultValue;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            int pos = viewHolder.AdapterPosition;
            if(swipePosition != pos)
            {
                if (!removerQueue.Contains(swipePosition))
                    removerQueue.Enqueue(swipePosition);
                swipePosition = pos;
                if (buttonBuffer.ContainsKey(swipePosition))
                    buttonList = buttonBuffer[swipePosition];
                else
                    buttonList.Clear();
                buttonBuffer.Clear();
                swipeTheshold = 0.5f * buttonList.Count * buttonWidth;
                RecoverSwipedItem();
            }
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            int pos = viewHolder.AdapterPosition;
            float translationX = dX;
            View itemVIew = viewHolder.ItemView;
            if(pos < 0)
            {
                swipePosition = pos;
                return;
            }
            if(actionState == ItemTouchHelper.ActionStateSwipe)
            {
                
                if (dX < 0)
                {
                    List<CustomButton> buffer = new List<CustomButton>();
                    if (!buttonBuffer.ContainsKey(pos))
                    {
                        InstanciateCustomButtom(viewHolder, buffer);
                        buttonBuffer.Add(pos, buffer);
                    }
                    else
                    {
                        buffer = buttonBuffer[pos];
                    }
                    translationX = dX * buffer.Count * buttonWidth / itemVIew.Width;
                    DrawButton(c, itemVIew, buffer, pos, translationX);
                }
            }
            base.OnChildDraw(c, recyclerView, viewHolder, translationX, dY, actionState, isCurrentlyActive);
        }
        private void DrawButton(Canvas c, View itemVIew, List<CustomButton> buffer, int pos, float translationX)
        {
            float right = itemVIew.Right;
            float dButtonWidth = -1 * translationX / buffer.Count;
            foreach(CustomButton button in buffer)
            {
                float left = right - dButtonWidth;
                button.OnDraw(c, new RectF(left, itemVIew.Top, right, itemVIew.Bottom), pos);
                right = left;

            }
        }
    }
}
