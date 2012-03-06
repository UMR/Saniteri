package com.umr.saniteri.ui;

import java.net.SocketException;
import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONObject;

import com.umr.saniteri.connection.RestClient;
import com.umr.saniteri.connection.RestClient.RequestMethod;
import com.umr.saniteri.entity.DeviceListItem;
import com.umr.saniteri.lib.DeviceListAdapter;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.NetworkInfo.State;
import android.os.Bundle;
import android.os.Handler;
import android.preference.PreferenceManager;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.Toast;
import android.widget.AdapterView.OnItemClickListener;

public class DeviceList extends Activity {
	ListView lvDeviceList;
	SharedPreferences sharedpreference;
	String ipAddressForWebService;
	RestClient restClient;
	DeviceListAdapter deviceListAdapter;
	ArrayList<DeviceListItem> deviceList;
	String responseString;
	State wifiState;
	public static final String tag = "DeviceList";
	ArrayList<Integer> listOfUnitNumbers;
	Handler exceptionHandler;
	Runnable exceptionMsgRunnable;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		super.onCreate(savedInstanceState);
		setContentView(R.layout.devicelist);
		initializeUIcontrols();
		registerEvents();
		loadData();
	}

	private void loadData() {
		// TODO Auto-generated method stub
		ConnectivityManager conMan = (ConnectivityManager) getSystemService(CONNECTIVITY_SERVICE);

		wifiState = conMan.getNetworkInfo(ConnectivityManager.TYPE_WIFI)
				.getState();

		try {
			// if (wifiState == NetworkInfo.State.CONNECTED
			// || wifiState == NetworkInfo.State.CONNECTING) {

			restClient = new RestClient("http://" + ipAddressForWebService
					+ getString(R.string.url_GetAllInventoryInfo));
			restClient.Execute(RequestMethod.GET);
			String response = restClient.getResponse();

			JSONArray responseArray = new JSONArray(response);

			// listOfUnitNumbers = new ArrayList<Integer>();

			for (int i = 0; i < responseArray.length(); ++i) {
				DeviceListItem deviceListItem = new DeviceListItem();

				JSONObject deviceItemJSONobject = responseArray
						.getJSONObject(i);

				deviceListItem.setUnitId(deviceItemJSONobject.getInt("CanId"));
				deviceListItem.setUnitLocation(deviceItemJSONobject
						.getString("Room"));
				Log.d(tag, deviceItemJSONobject.getString("Room"));

				deviceList.add(deviceListItem);

			}

			if (deviceList.size() != 0) {
				deviceListAdapter = new DeviceListAdapter(DeviceList.this,
						deviceList);

				lvDeviceList.setAdapter(deviceListAdapter);
				lvDeviceList.setDivider(null);
				lvDeviceList.setDividerHeight(0);
			}

			//					
			// for (int i = 0; i < responseArray.length(); ++i) {
			//						
			// DeviceListItem deviceListItem= new DeviceListItem();
			//						
			//						
			//						
			// int unitNumber = responseArray.getInt(i);
			//						
			// deviceListItem.setUnitId(unitNumber);
			// deviceListItem.setUnitLocation("");
			// listOfUnitNumbers.add(unitNumber);
			// deviceList.add(deviceListItem);
			//						
			// }

			// if (listOfUnitNumbers.size() != 0) {
			//
			// deviceListAdapter = new DeviceListAdapter(DeviceList.this,
			// deviceList);
			//
			// lvDeviceList.setAdapter(deviceListAdapter);
			//
			// }
			// } else {
			// exceptionMsgRunnable = new Runnable() {
			//			
			// @Override
			// public void run() {
			// // TODO Auto-generated method stub
			// Toast.makeText(DeviceList.this,
			// "Check Wifi connectivity in your device.",
			// Toast.LENGTH_SHORT).show();
			// }
			//			
			// };
			// exceptionHandler.post(exceptionMsgRunnable);
			// }

		} catch (SocketException e) {

			exceptionMsgRunnable = new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub

					Toast
							.makeText(
									DeviceList.this,
									"Please check internet connectivity.Restart the application.",
									Toast.LENGTH_SHORT).show();

				}

			};
			exceptionHandler.post(exceptionMsgRunnable);

		}

		catch (Exception e) {
			exceptionMsgRunnable = new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub
					Toast.makeText(DeviceList.this,
							"Problem in loading.Restart the application.",
							Toast.LENGTH_SHORT).show();

				}

			};
			exceptionHandler.post(exceptionMsgRunnable);

		}

	}

	private void registerEvents() {
		// TODO Auto-generated method stub

		lvDeviceList.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> arg0, View arg1, int arg2,
					long arg3) {
				// TODO Auto-generated method stub

				Intent deviceDetailIntent = new Intent(DeviceList.this,
						DeviceDetail.class);
				deviceDetailIntent.putExtra("DeviceID", deviceList.get(arg2)
						.getUnitId());

				startActivity(deviceDetailIntent);

			}

		});

	}

	private void initializeUIcontrols() {
		// TODO Auto-generated method stub
		lvDeviceList = (ListView) findViewById(R.id.lvDeviceList);
		sharedpreference = PreferenceManager.getDefaultSharedPreferences(this);
		ipAddressForWebService = sharedpreference.getString(
				"IpAddressForWebService", "172.16.205.56");
		deviceList = new ArrayList<DeviceListItem>();
		exceptionHandler = new Handler();

	}

}
