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

namespace MOB_Simple_project
{
    class CandidateSearchListAdapter : BaseAdapter<Candidates>
    {
        private List<Candidates> _candidates;
        private Activity _context;
        private string _technologyId;
        private string _candidateId;
        public CandidateSearchListAdapter(Activity context, List<Candidates> list, string technologyId)
        {
            _candidates = list;
            _context = context;
            _technologyId = technologyId;
        }
        public override Candidates this[int position]
        {
            get
            {
                return _candidates[position];
            }
        }


        public override int Count
        {
            get
            {
                return _candidates.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            convertView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CandidateList, null);
            View row = convertView;
            Candidates atualCandidate = _candidates[position];
            Experience[] exp = atualCandidate.experience;
            TextView txtFullName = row.FindViewById<TextView>(Resource.Id.txtCandName);
            TextView txtYoE = row.FindViewById<TextView>(Resource.Id.txtYearsExperience);
            TextView txtId = row.FindViewById<TextView>(Resource.Id.txtCandidateId);
            //LinearLayout grupBtn = row.FindViewById<LinearLayout>(Resource.Id.groupButtons);
            //grupBtn.Visibility = ViewStates.Invisible;
            int years = 0;
            txtFullName.Text = atualCandidate.fullName;
            _candidateId = atualCandidate.candidateId;
            txtId.Text = atualCandidate.candidateId;
            foreach(Experience e in exp)
            {
                if(e.technologyId == _technologyId)
                {
                    years = e.yearsOfExperience;
                }
            }
            txtYoE.Text = "Years of Experience: " + years.ToString() + " years";

            return row;
        }
    }
}