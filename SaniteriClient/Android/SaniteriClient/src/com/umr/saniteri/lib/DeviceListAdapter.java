package com.umr.saniteri.lib;

import java.util.ArrayList;

import com.umr.saniteri.entity.DeviceListItem;
import com.umr.saniteri.lib.CanListDataAdapter.ViewHolder;
import com.umr.saniteri.ui.R;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class DeviceListAdapter extends BaseAdapter {

	private final Activity activity;
	private ArrayList<DeviceListItem> deviceList = new ArrayList<DeviceListItem>();
	private static LayoutInflater inflater = null;
	public Context context;

	public DeviceListAdapter(Activity activity,
			ArrayList<DeviceListItem> deviceList) {
		this.activity = activity;
		this.deviceList = deviceList;
		inflater = (LayoutInflater) activity
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		context = activity.getApplicationContext();

	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return deviceList.size();
	}

	@Override
	public Object getItem(int arg0) {
		// TODO Auto-generated method stub
		return deviceList.get(arg0);
	}

	@Override
	public long getItemId(int arg0) {
		// TODO Auto-generated method stub
		return arg0;
	}

	public static class ViewHolder {
		public TextView lblUnitIdInListItem;
		public TextView lblUnitLocationInListItem;
	}

	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		// TODO Auto-generated method stub

		View vi = convertView;
		final ViewHolder holder;

		if (convertView == null) {
			vi = inflater.inflate(R.layout.devicelistitem, null);
			holder = new ViewHolder();
			holder.lblUnitIdInListItem = (TextView) vi
					.findViewById(R.id.lblUnitIdInListItem);
			holder.lblUnitLocationInListItem = (TextView) vi
					.findViewById(R.id.lblUnitLocationInListItem);
			vi.setTag(holder);
		} else
			holder = (ViewHolder) vi.getTag();

		String unitId = deviceList.get(position).getUnitId().toString();
		String unitLocation = deviceList.get(position).getUnitLocation();

		holder.lblUnitIdInListItem.setText(unitId);
		holder.lblUnitLocationInListItem.setText(unitLocation);

		return vi;
	}

}
