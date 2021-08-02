using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOB_Simple_project.Adapter
{
    public class IfsViewHolder : RecyclerView.ViewHolder
    {
        public TextView _txtCandidateName, _txtCandidateId;
        public ImageView _imgUser;

        public IfsViewHolder(View itemView) : base(itemView)
        {
            _txtCandidateId = itemView.FindViewById<TextView>(Resource.Id.txtCandidateId);
            _txtCandidateName = itemView.FindViewById<TextView>(Resource.Id.txtCandidateName);
            _imgUser = itemView.FindViewById<ImageView>(Resource.Id.imgUser);
        }
    }
}