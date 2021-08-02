using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using IFS_Mobile_Challenge.Model;
using Com.Bumptech.Glide;
using AndroidX.RecyclerView.Widget;

namespace MOB_Simple_project.Adapter
{
    public class IfsAdapter : RecyclerView.Adapter
    {
        Context _context;
        List<Candidates> _candidates;
        string _technologyId;
        int _years;

        public IfsAdapter(Context context, List<Candidates> candidates, string technologyId)
        {
            _context = context;
            _candidates = candidates;
            _technologyId = technologyId;
        }
        public override int ItemCount => _candidates.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            IfsViewHolder ifsViewHolder = holder as IfsViewHolder;
            Glide.With(_context).Load(_candidates[position].image)
                .Into(ifsViewHolder._imgUser);
            
            Experience[] ex = _candidates[position].experience;
            foreach(Experience e in ex)
            {
                if(e.technologyId == _technologyId)
                {
                    _years = e.yearsOfExperience;
                }
            }
            ifsViewHolder._txtCandidateName.Text = _candidates[position].fullName + "( " + _years + " years of experence )";
            ifsViewHolder._txtCandidateId.Text = _candidates[position].candidateId;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View candidateView = LayoutInflater.From(_context).Inflate(Resource.Layout.swipe_item_layout, parent, false);
            return new IfsViewHolder(candidateView);
        }
    }
}