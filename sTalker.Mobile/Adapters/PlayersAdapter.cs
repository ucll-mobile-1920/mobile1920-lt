﻿using Android.Content;
using Android.Views;
using Android.Widget;
using sTalker.Shared.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace sTalker.Adapters
{
    public class PlayersAdapter : BaseAdapter<Player>
    {
        public List<Player> list;
        private Context context;
        private ConcurrentDictionary<string, Player> dictionary;
        public PlayersAdapter(Context context, List<Player> list)
        {
            this.list = list;
            this.context = context;
        }

        public override Player this[int position]
        {
            get
            {
                return list[position];
            }
        }
        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(context).Inflate(Resource.Layout.playerRow, null, false);
                }
                TextView txtName = row.FindViewById<TextView>(Resource.Id.Name);
                txtName.Text = list[position].Name;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return row;
        }
    }
}
