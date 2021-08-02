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
    class ExpandableListAdapter : BaseExpandableListAdapter
    {
        private Activity _context;
        private List<Candidates> _listDataHeader; // header titles

        // child data in format of header title, child title
        private Dictionary<Candidates, List<string>> _listDataChild;
        ExpandableListView expandList;
        string _technology;

        public ExpandableListAdapter(Activity context, List<Candidates> listDataHeader, Dictionary<Candidates, List<string>> listChildData, ExpandableListView mView, string Technology)
        {
            _context = context;
            _listDataHeader = listDataHeader;
            _listDataChild = listChildData;
            expandList = mView;
            _technology = Technology;
        }
        public override int GroupCount
        {
            get
            {
                return _listDataHeader.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return false;
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return _listDataChild[_listDataHeader[groupPosition]][childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            int childCount = _listDataChild[_listDataHeader[groupPosition]].Count;
            return childCount;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            string childText = (string)GetChild(groupPosition, childPosition);
            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.AceptDeclineUser, null);
            }
            Button btnAccept = (Button)convertView.FindViewById(Resource.Id.btnAcceptCandidate);
            Button btnReject = (Button)convertView.FindViewById(Resource.Id.btnRejectCandidate);
            //btnAccept.Click += MainActivity.AcceptCandidate;
            return convertView;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return new JavaObjectWrapper<Candidates>() { Obj = _listDataHeader[groupPosition] };
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            Candidates headerTitle = _listDataHeader[groupPosition];
            Experience[] YoE = headerTitle.experience;
            string yearsText = "";
            convertView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CandidateList, null);
            TextView candName = (TextView)convertView.FindViewById(Resource.Id.txtCandName);
            TextView candYrExp = (TextView)convertView.FindViewById(Resource.Id.txtYearsExperience);
            candName.Text = headerTitle.fullName;
            
            foreach(Experience ex in YoE){
                if(ex.technologyId == _technology){
                    yearsText = "Years Of Experience: " + ex.yearsOfExperience + " years";
                }
            }
            candYrExp.Text = yearsText;

            return convertView;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
        public class JavaObjectWrapper<T> : Java.Lang.Object
        {
            public T Obj { get; set; }
        }
    }
}