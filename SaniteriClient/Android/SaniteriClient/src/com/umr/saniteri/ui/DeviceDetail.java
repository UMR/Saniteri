package com.umr.saniteri.ui;

import org.json.JSONObject;

import com.umr.saniteri.connection.RestClient;
import com.umr.saniteri.connection.RestClient.RequestMethod;
import com.umr.saniteri.entity.CanLiveStatus;
import com.umr.saniteri.ui.R.color;

import android.app.Activity;
import android.app.TabActivity;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.text.TextUtils;
import android.util.AttributeSet;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.Window;
import android.view.View.OnClickListener;
import android.view.ViewGroup.LayoutParams;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TabHost;
import android.widget.TabWidget;
import android.widget.TextView;
import android.widget.Toast;

public class DeviceDetail extends TabActivity {
	public static final String tag = "DeviceDetail";
	Button btnOpenLid, btnCloseLid, btnOpenDoor;
	TextView lblDeviceDetailHeader, lblPowerInDeviceDetail,
			lblCommStatusInDeviceDetail, lblWeightInDeviceDetail,
			lblBagStatusInDeviceDetail, lblLidOpenInDeviceDetail,
			lblDoorOpenInDeviceDetail, lblNeedServiceInDeviceDetail,
			lblFaultInDeviceDetail;

	TabWidget tabWidget;
	TabHost tabHost;
	RestClient restClient;
	SharedPreferences sharedpreference;
	String ipAddressForWebService;
	int deviceID;
	boolean CanLidOpen = false, CanDoorOpen = false;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		super.onCreate(savedInstanceState);
		setContentView(R.layout.devicedetail);
		initializeUIcontrols();
		registerEvents();
		loadData();

	}

	private void initializeUIcontrols() {
		// TODO Auto-generated method stub

		btnOpenLid = (Button) findViewById(R.id.btnOpenLid);
		btnCloseLid = (Button) findViewById(R.id.btnCloseLid);
		btnOpenDoor = (Button) findViewById(R.id.btnOpenDoor);
		lblDeviceDetailHeader = (TextView) findViewById(R.id.lblDeviceDetailHeader);
		lblPowerInDeviceDetail = (TextView) findViewById(R.id.lblPowerInDeviceDetail);
		lblCommStatusInDeviceDetail = (TextView) findViewById(R.id.lblCommStatusInDeviceDetail);
		lblWeightInDeviceDetail = (TextView) findViewById(R.id.lblWeightInDeviceDetail);
		lblBagStatusInDeviceDetail = (TextView) findViewById(R.id.lblBagStatusInDeviceDetail);
		lblLidOpenInDeviceDetail = (TextView) findViewById(R.id.lblLidOpenInDeviceDetail);
		lblDoorOpenInDeviceDetail = (TextView) findViewById(R.id.lblDoorOpenInDeviceDetail);
		lblNeedServiceInDeviceDetail = (TextView) findViewById(R.id.lblNeedServiceInDeviceDetail);
		lblFaultInDeviceDetail = (TextView) findViewById(R.id.lblFaultInDeviceDetail);
		sharedpreference = PreferenceManager.getDefaultSharedPreferences(this);
		ipAddressForWebService = sharedpreference.getString(
				"IpAddressForWebService", "172.16.205.56");
		tabHost = getTabHost();
		tabWidget = tabHost.getTabWidget();

		TextView canstatusView = new TextView(this);

		LayoutParams params = new LinearLayout.LayoutParams(200, 45, 1.0f);

		canstatusView.setLayoutParams(params);
		canstatusView.setBackgroundResource(R.drawable.tab_selector);
		canstatusView.setText("Status");
		canstatusView.setTextColor(Color.BLACK);
		canstatusView.setGravity(Gravity.CENTER);

		TabHost.TabSpec specCanStatus = tabHost.newTabSpec("CanStatusTab");
		// specCanStatus.setIndicator("Status",getResources().getDrawable(R.drawable.tab_selector));
		specCanStatus.setIndicator(canstatusView);
		specCanStatus.setContent(R.id.layoutCanStatus);

		TextView canLogView = new TextView(this);
		LayoutParams canLogparams = new LinearLayout.LayoutParams(200, 45, 1.0f);
		canLogView.setLayoutParams(canLogparams);
		canLogView.setBackgroundResource(R.drawable.tab_selector);
		canLogView.setText("Log");
		canLogView.setTextColor(Color.BLACK);
		canLogView.setGravity(Gravity.CENTER);

		TabHost.TabSpec specCanLog = tabHost.newTabSpec("CanLogTab");
		// specCanLog.setIndicator("Log",getResources().getDrawable(R.drawable.tab_selector));
		specCanLog.setIndicator(canLogView);
		specCanLog.setContent(R.id.layoutCanLog);

		TextView canActivityView = new TextView(this);
		LayoutParams canActivityParams = new LinearLayout.LayoutParams(200, 45,
				1.0f);
		canActivityView.setLayoutParams(canActivityParams);
		canActivityView.setBackgroundResource(R.drawable.tab_selector);
		canActivityView.setText("Activity");
		canActivityView.setTextColor(Color.BLACK);
		canActivityView.setGravity(Gravity.CENTER);

		TabHost.TabSpec specCanActivity = tabHost.newTabSpec("CanActivityTab");
		// specCanActivity.setIndicator("Activity",getResources().getDrawable(R.drawable.tab_selector));
		specCanActivity.setIndicator(canActivityView);
		specCanActivity.setContent(R.id.layoutCanActivity);

		tabHost.addTab(specCanStatus);
		tabHost.addTab(specCanLog);
		tabHost.addTab(specCanActivity);

		tabHost.setCurrentTab(0);

	}

	private void registerEvents() {
		// TODO Auto-generated method stub
		btnOpenDoor.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				restClient = new RestClient("http://" + ipAddressForWebService
						+ getString(R.string.url_InsertCanCommand));
				restClient.AddHeader("Content-Type", "application/json");
				restClient.AddParam("canId", String.valueOf(deviceID));
				restClient.AddParam("commandId", "13");
				// restClient.AddParam("canLidStatus", "0");
				try {
					restClient.Execute(RequestMethod.POST);

					if (restClient.getResponseCode() == 200) {
						Toast.makeText(DeviceDetail.this, "Request accepted",
								Toast.LENGTH_SHORT).show();
					} else if (restClient.getResponseCode() == 400) {
						Toast.makeText(DeviceDetail.this, "Request failed",
								Toast.LENGTH_SHORT).show();
					} else if (restClient.getResponseCode() == 409) {
						Toast.makeText(DeviceDetail.this,
								"Request already taken.", Toast.LENGTH_SHORT)
								.show();
					}
				} catch (Exception e) {
					// TODO Auto-generated catch block
					Toast.makeText(DeviceDetail.this, "Request not accepted.",
							Toast.LENGTH_LONG).show();
				}

			}

		});

		btnOpenLid.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				restClient = new RestClient("http://" + ipAddressForWebService
						+ getString(R.string.url_InsertCanCommand));
				restClient.AddHeader("Content-Type", "application/json");
				restClient.AddParam("canId", String.valueOf(deviceID));
				restClient.AddParam("commandId", "11");
				// restClient.AddParam("canLidStatus", "1");
				try {
					restClient.Execute(RequestMethod.POST);

					if (restClient.getResponseCode() == 200) {
						Toast.makeText(DeviceDetail.this, "Request accepted",
								Toast.LENGTH_SHORT).show();
					} else if (restClient.getResponseCode() == 400) {
						Toast.makeText(DeviceDetail.this, "Request failed",
								Toast.LENGTH_SHORT).show();
					} else if (restClient.getResponseCode() == 409) {
						Toast.makeText(DeviceDetail.this,
								"Request already taken.", Toast.LENGTH_SHORT)
								.show();
					}
				} catch (Exception e) {
					// TODO Auto-generated catch block
					Toast.makeText(DeviceDetail.this, "Request not accepted.",
							Toast.LENGTH_LONG).show();
				}
			}

		});

		btnCloseLid.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				restClient = new RestClient("http://" + ipAddressForWebService
						+ getString(R.string.url_InsertCanCommand));
				restClient.AddHeader("Content-Type", "application/json");
				restClient.AddParam("canId", String.valueOf(deviceID));
				restClient.AddParam("commandId", "12");
				// restClient.AddParam("canLidStatus", "0");

				try {
					restClient.Execute(RequestMethod.POST);
					Log.d(tag, restClient.getResponse());
					if (restClient.getResponseCode() == 200) {
						Toast.makeText(DeviceDetail.this, "Request accepted",
								Toast.LENGTH_SHORT).show();
					} else if (restClient.getResponseCode() == 400) {
						Toast.makeText(DeviceDetail.this, "Request failed",
								Toast.LENGTH_SHORT).show();
					} else if (restClient.getResponseCode() == 409) {
						Toast.makeText(DeviceDetail.this,
								"Request already taken.", Toast.LENGTH_SHORT)
								.show();
					}
				} catch (Exception e) {
					// TODO Auto-generated catch block
					Toast.makeText(DeviceDetail.this, "Request not accepted.",
							Toast.LENGTH_LONG).show();
				}

			}

		});

	}

	private void loadData() {
		// TODO Auto-generated method stub

		deviceID = getIntent().getExtras().getInt("DeviceID");
		lblDeviceDetailHeader.setText("Device Detail for Unit " + deviceID);
		loadCanData();

	}

	private void loadCanData() {
		// TODO Auto-generated method stub
		restClient = new RestClient("http://" + ipAddressForWebService
				+ getString(R.string.url_GetLiveCanStatus));

		restClient.AddParam("CanId", String.valueOf(deviceID));
		try {
			restClient.Execute(RequestMethod.GET);

			String canLiveStatusResponseString = restClient.getResponse();

			JSONObject canLiveStatusJSONobject = new JSONObject(
					canLiveStatusResponseString);

			if (canLiveStatusJSONobject.getString("BagInfo") != "null") {
				lblBagStatusInDeviceDetail.setText(canLiveStatusJSONobject
						.getString("BagInfo"));
			} else {
				lblBagStatusInDeviceDetail.setText("N/A");
			}

			if (!canLiveStatusJSONobject.isNull("CommunicationStatus")) {
				if (canLiveStatusJSONobject.getInt("CommunicationStatus") == 1) {
					lblCommStatusInDeviceDetail.setText("Active");
				} else {
					lblCommStatusInDeviceDetail.setText("Inactive");
				}
			} else {
				lblCommStatusInDeviceDetail.setText("N/A");

			}

			if (!canLiveStatusJSONobject.isNull("PowerStatus")) {
				if (canLiveStatusJSONobject.getInt("PowerStatus") == 1) {
					lblPowerInDeviceDetail.setText("On");
				} else {
					lblPowerInDeviceDetail.setText("Off");
				}
			} else {
				lblPowerInDeviceDetail.setText("N/A");
			}

			if (!canLiveStatusJSONobject.isNull("Weight")) {

				lblWeightInDeviceDetail.setText(canLiveStatusJSONobject
						.getDouble("Weight")
						+ "lbs");
			} else {
				lblWeightInDeviceDetail.setText("N/A");
			}

			if (!canLiveStatusJSONobject.isNull("LidOpen")) {
				if (canLiveStatusJSONobject.getBoolean("LidOpen")) {
					CanLidOpen = true;
					lblLidOpenInDeviceDetail.setText("Yes");
				} else {
					lblLidOpenInDeviceDetail.setText("No");
				}
			} else {
				lblLidOpenInDeviceDetail.setText("N/A");
			}

			if (!canLiveStatusJSONobject.isNull("DoorOpen")) {
				if (canLiveStatusJSONobject.getBoolean("DoorOpen")) {
					CanDoorOpen = true;
					lblDoorOpenInDeviceDetail.setText("Yes");
				} else {
					lblDoorOpenInDeviceDetail.setText("No");
				}
			} else {
				lblDoorOpenInDeviceDetail.setText("N/A");
			}

			if (!canLiveStatusJSONobject.isNull("NeedService")) {

				if (canLiveStatusJSONobject.getBoolean("NeedService")) {
					lblNeedServiceInDeviceDetail.setText("Yes");
				} else {
					lblNeedServiceInDeviceDetail.setText("No");
				}
			} else {
				lblNeedServiceInDeviceDetail.setText("N/A");
			}
			if (canLiveStatusJSONobject.getString("Fault") != "null") {
				if (canLiveStatusJSONobject.getString("Fault") != "null") {
					lblFaultInDeviceDetail.setText(canLiveStatusJSONobject
							.getString("Fault"));
				} else {
					lblFaultInDeviceDetail.setText("N/A");
				}
			} else {
				lblFaultInDeviceDetail.setText("N/A");
			}

		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

}
