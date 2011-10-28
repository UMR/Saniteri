package com.umr.saniteri.lib;

import java.util.ArrayList;
import com.umr.saniteri.ui.R;
import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class CanListDataAdapter extends BaseAdapter {

	private final Activity activity;
	private ArrayList<String> canIdList = new ArrayList<String>();
	private static LayoutInflater inflater = null;
	public Context context;

	public CanListDataAdapter(Activity activity, ArrayList<String> canIdList) {
		this.activity = activity;
		this.canIdList = canIdList;
		inflater = (LayoutInflater) activity
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		context = activity.getApplicationContext();

	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return canIdList.size();
	}

	@Override
	public Object getItem(int position) {
		// TODO Auto-generated method stub
		return canIdList.get(position);
	}

	@Override
	public long getItemId(int position) {
		// TODO Auto-generated method stub
		return position;
	}

	public static class ViewHolder {
		public TextView lblUnitNumberinList;
	}

	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		// TODO Auto-generated method stub

		View vi = convertView;
		final ViewHolder holder;

		if (convertView == null) {
			vi = inflater.inflate(R.layout.canlistitem, null);
			holder = new ViewHolder();
			holder.lblUnitNumberinList = (TextView) vi
					.findViewById(R.id.lblUnitNumberInList);
			vi.setTag(holder);
		} else
			holder = (ViewHolder) vi.getTag();

		String unitNumber = canIdList.get(position).toString();

		holder.lblUnitNumberinList.setText(unitNumber);

		return vi;
	}

}
