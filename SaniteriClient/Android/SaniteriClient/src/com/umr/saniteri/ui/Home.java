package com.umr.saniteri.ui;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.HashMap;
import java.util.Timer;
import java.util.TimerTask;
import java.util.UUID;
import org.json.JSONArray;
import org.json.JSONObject;

import com.umr.saniteri.connection.RestClient;
import com.umr.saniteri.connection.RestClient.RequestMethod;
import com.umr.saniteri.lib.CanListDataAdapter;
import com.umr.saniteri.ui.R;

import android.app.AlertDialog;
import android.app.TabActivity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.media.AudioFormat;
import android.media.AudioManager;
import android.media.AudioTrack;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TabHost;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.TabHost.OnTabChangeListener;

public class Home extends TabActivity {
	ListView lvUnitNumbers;
	ArrayList<String> listOfUnitNumbers;
	RestClient restClient;
	CanListDataAdapter lvUnitNumberAdapter;
	Button btnOpen, btnClose;
	UUID id;
	Date dateTime;
	public static final String tag = "Tabs";
	String selectedUnitNumber;
	TextView lblCanId, lblCanIpAddress, lblRoomNo, lblFloorNo;
	TextView lblCanIdinCanStatus, lblCanStatus;
	HashMap<String, String> canConfigData;
	HashMap<String, String> canStatusData;
	SharedPreferences sharedpreference;
	String ipAddressForWebService;
	HashMap<String, String> listOfUnitNumbersFromId;
	SimpleDateFormat dateFormatter;
	Timer timer;
	AudioTrack audioTrack;
	final int duration = 1; // seconds
	final int sampleRate = 8000;
	final int numSamples = duration * sampleRate;
	double sample[] = new double[numSamples];
	double freqOfTone = 400; // hz
	int beepInterval = 250;
	int beepDevider = 4;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);
		initiliazeControls();
		loadInitialData();
		registerEvents();
		TabHost tabHost = getTabHost();

		TabHost.TabSpec specHome = tabHost.newTabSpec("HomeTab");
		specHome.setIndicator("Home");
		specHome.setContent(R.id.LayoutHome);

		TabHost.TabSpec specCanStatus = tabHost.newTabSpec("CanStatus");
		specCanStatus.setIndicator("Can Status");
		specCanStatus.setContent(R.id.LayoutCanStatus);

		TabHost.TabSpec specCanConfiguration = tabHost
				.newTabSpec("CanConfiguration");
		specCanConfiguration.setIndicator("Can Configuration");
		specCanConfiguration.setContent(R.id.LayoutCanConfig);

		tabHost.addTab(specHome);
		tabHost.addTab(specCanStatus);
		tabHost.addTab(specCanConfiguration);

		tabHost.setCurrentTab(0);

		tabHost.setOnTabChangedListener(new OnTabChangeListener() {

			@Override
			public void onTabChanged(String tabId) {
				// TODO Auto-generated method stub

			}
		});

	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.menu, menu);
		return super.onCreateOptionsMenu(menu);
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		switch (item.getItemId()) {
		case R.id.menuitem_IpAddressForWebService:
			Intent intent = new Intent(this, WebServicePreference.class);
			startActivity(intent);
			break;
		default:
			break;
		}
		return false;
	}

	private void registerEvents() {
		// TODO Auto-generated method stub

		btnOpen.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				id = UUID.randomUUID();
				restClient = new RestClient("http://" + ipAddressForWebService
						+ getString(R.string.url_InsertCanCommand));
				restClient.AddHeader("Content-Type", "application/json");
				restClient.AddParam("canId", selectedUnitNumber);
				restClient.AddParam("canLidStatus", "1");
				try {
					restClient.Execute(RequestMethod.POST);

					if (restClient.getResponseCode() == 200) {
						Toast.makeText(Home.this, "Request accepted",
								Toast.LENGTH_LONG).show();
					} else if (restClient.getResponseCode() == 400) {
						Toast.makeText(Home.this, "Request failed",
								Toast.LENGTH_LONG).show();
					} else if (restClient.getResponseCode() == 409) {
						Toast.makeText(Home.this, "Request already taken.",
								Toast.LENGTH_LONG).show();
					}
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		});

		btnClose.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				id = UUID.randomUUID();
				restClient = new RestClient("http://" + ipAddressForWebService
						+ getString(R.string.url_InsertCanCommand));
				restClient.AddHeader("Content-Type", "application/json");
				restClient.AddParam("canId", selectedUnitNumber);
				restClient.AddParam("canLidStatus", "0");

				try {
					restClient.Execute(RequestMethod.POST);
					Log.d(tag, restClient.getResponse());
					if (restClient.getResponseCode() == 200) {
						Toast.makeText(Home.this, "Request accepted",
								Toast.LENGTH_LONG).show();
					} else if (restClient.getResponseCode() == 400) {
						Toast.makeText(Home.this, "Request failed",
								Toast.LENGTH_LONG).show();
					} else if (restClient.getResponseCode() == 409) {
						Toast.makeText(Home.this, "Request already taken.",
								Toast.LENGTH_LONG).show();
					}
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		});

		lvUnitNumbers.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> arg0, View arg1, int arg2,
					long arg3) {
				// TODO Auto-generated method stub
				// selectedUnitNumber = lvUnitNumbers.getItemAtPosition(arg2)
				// .toString();
				try {
					selectedUnitNumber = listOfUnitNumbers.get(arg2);
					Log.d(tag, "Selected Unit Number in onitemclick"
							+ selectedUnitNumber);

					canConfigData = getCanConfigurationData(selectedUnitNumber);

					lblCanId.setText(canConfigData.get("CanId"));
					lblCanIpAddress.setText(canConfigData.get("CanIpAddress"));
					lblFloorNo.setText(canConfigData.get("FloorNo"));
					lblRoomNo.setText(canConfigData.get("RoomNo"));

					canStatusData = getCanStatus(selectedUnitNumber);

					lblCanIdinCanStatus.setText(canStatusData.get("CanId"));
					lblCanStatus.setText(canStatusData.get("CanStatus"));

					if (canStatusData.get("CanStatusType").compareTo(getString(R.string.CanStatusType_Full))==0) {
						timer = new Timer();
						timer.scheduleAtFixedRate(new TimerTask() {
							@Override
							public void run() {
								// TODO Auto-generated method stub
								playSound();
							}
						}, 0, 500);

						showAlert(Home.this, timer);
					}

					arg1.getFocusables(arg2);
					arg1.setSelected(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}

		});

		/*
		 * lvUnitNumbers.setOnItemSelectedListener(new OnItemSelectedListener()
		 * {
		 * 
		 * @Override public void onItemSelected(AdapterView<?> arg0, View arg1,
		 * int arg2, long arg3) { // TODO Auto-generated method stub
		 * selectedUnitNumber = lvUnitNumbers.getItemAtPosition(arg2)
		 * .toString(); Log.d(tag, "Selected Unit Number in onitemclick" +
		 * selectedUnitNumber);
		 * 
		 * canConfigData = getCanConfigurationData(selectedUnitNumber);
		 * 
		 * lblCanId.setText(canConfigData.get("CanId"));
		 * lblCanIpAddress.setText(canConfigData.get("CanIpAddress"));
		 * lblFloorNo.setText(canConfigData.get("FloorNo"));
		 * lblRoomNo.setText(canConfigData.get("RoomNo"));
		 * arg0.setBackgroundColor(R.color.light_orange_color);
		 * 
		 * }
		 * 
		 * @Override public void onNothingSelected(AdapterView<?> arg0) { //
		 * TODO Auto-generated method stub
		 * 
		 * } });
		 */

	}

	private void loadInitialData() {
		// TODO Auto-generated method stub
		restClient = new RestClient("http://" + ipAddressForWebService
				+ getString(R.string.url_GetAllCanId));
		Log.d(tag, "http://" + ipAddressForWebService
				+ getString(R.string.url_GetAllCanId));

		try {
			restClient.Execute(RequestMethod.GET);
			String response = restClient.getResponse();

			JSONArray responseArray = new JSONArray(response);

			for (int i = 0; i < responseArray.length(); ++i) {
				String unitNumber = responseArray.get(i).toString();
				listOfUnitNumbers.add(unitNumber);
				Log.d("listOfunitNumbers", listOfUnitNumbers.get(i));
			}

			for (int i = 0; i < listOfUnitNumbers.size(); ++i) {
				listOfUnitNumbersFromId.put(listOfUnitNumbers.get(i), "Unit "
						+ i);
			}

			// lvUnitNumberAdapter = new CanListDataAdapter(this,
			// listOfUnitNumbers);

			ArrayList<String> a = new ArrayList<String>();
			a.addAll(listOfUnitNumbersFromId.values());

			lvUnitNumberAdapter = new CanListDataAdapter(this, a);

			lvUnitNumbers.setAdapter(lvUnitNumberAdapter);

			// selectedUnitNumber =
			// lvUnitNumbers.getItemAtPosition(0).toString();
			selectedUnitNumber = listOfUnitNumbers.get(0);

			canConfigData = getCanConfigurationData(selectedUnitNumber);

			lblCanId.setText(canConfigData.get("CanId"));
			lblCanIpAddress.setText(canConfigData.get("CanIpAddress"));
			lblFloorNo.setText(canConfigData.get("FloorNo"));
			lblRoomNo.setText(canConfigData.get("RoomNo"));

			canStatusData = getCanStatus(selectedUnitNumber);

			lblCanIdinCanStatus.setText(canStatusData.get("CanId"));

			lblCanStatus.setText(canStatusData.get("CanStatus"));

			if (canStatusData.get("CanStatusType").compareTo(getString(R.string.CanStatusType_Full))==0) {
				timer = new Timer();
				timer.scheduleAtFixedRate(new TimerTask() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						playSound();
					}
				}, 0, 500);

				showAlert(Home.this, timer);
			}

		} catch (Exception e) {
			// TODO: hanedle exception
			e.printStackTrace();
		}

	}

	private void initiliazeControls() {
		// TODO Auto-generated method stub
		lvUnitNumbers = (ListView) findViewById(R.id.lvUnitNumbers);
		listOfUnitNumbers = new ArrayList<String>();
		listOfUnitNumbersFromId = new HashMap<String, String>();
		btnOpen = (Button) findViewById(R.id.btnOpen);
		btnClose = (Button) findViewById(R.id.btnClose);
		lblCanIdinCanStatus = (TextView) findViewById(R.id.lblCanIdinCanStatus);
		lblCanStatus = (TextView) findViewById(R.id.lblCanStatus);
		lblCanId = (TextView) findViewById(R.id.lblCanId);
		lblCanIpAddress = (TextView) findViewById(R.id.lblCanIpAddress);
		lblFloorNo = (TextView) findViewById(R.id.lblFloorNo);
		lblRoomNo = (TextView) findViewById(R.id.lblRoomNo);
		sharedpreference = PreferenceManager.getDefaultSharedPreferences(this);
		ipAddressForWebService = sharedpreference.getString(
				"IpAddressForWebService", "172.16.205.56");
		dateFormatter = new SimpleDateFormat("MM/dd/yyyy");
		timer = new Timer();
	}

	private HashMap<String, String> getCanConfigurationData(String canId) {
		restClient = new RestClient("http://" + ipAddressForWebService
				+ getString(R.string.url_GetInventoryInfo) + canId);
		try {
			HashMap<String, String> canConfig = new HashMap<String, String>();
			restClient.Execute(RequestMethod.GET);
			String canConfigResponse = null;
			if (restClient.getResponse() != "") {
				canConfigResponse = restClient.getResponse();
			}
			if (canConfigResponse != null) {
				JSONObject canConfigJsonObject = new JSONObject(
						canConfigResponse);

				if (canConfigJsonObject.getString("CanId") != "null") {
					canConfig.put("CanId", canConfigJsonObject
							.getString("CanId"));
				} else {
					canConfig.put("CanId", "N/A");
				}

				if (canConfigJsonObject.getString("IpAddress") != "null") {
					canConfig.put("CanIpAddress", canConfigJsonObject
							.getString("IpAddress"));
				} else {
					canConfig.put("CanIpAddress", "N/A");
				}

				if (canConfigJsonObject.getString("Floor") != "null") {
					canConfig.put("FloorNo", canConfigJsonObject
							.getString("Floor"));
				} else {
					canConfig.put("FloorNo", "N/A");
				}

				if (canConfigJsonObject.getString("Room") != "null")

				{
					canConfig.put("RoomNo", canConfigJsonObject
							.getString("Room"));
				} else {
					canConfig.put("RoomNo", "N/A");
				}
			} else {
				canConfig.put("CanId", "N/A");
				canConfig.put("CanIpAddress", "N/A");
				canConfig.put("FloorNo", "N/A");
				canConfig.put("RoomNo", "N/A");
			}

			return canConfig;

		}

		catch (Exception exception) {
			exception.printStackTrace();
			return null;
		}
	}

	private void playSound() {

		try {
			Log.d(tag, "Ok");
			byte generatedSnd[] = new byte[2 * numSamples];

			for (int i = 0; i < numSamples; ++i) {
				sample[i] = Math.sin(2 * Math.PI * i
						/ (sampleRate / freqOfTone));
			}

			// convert to 16 bit pcm sound array
			// assumes the sample buffer is normalised.

			int idx = 0;

			for (final double dVal : sample) {
				// scale to maximum amplitude
				final short val = (short) ((dVal * 52767));
				// in 16 bit wav PCM, first byte is the low
				// order byte
				generatedSnd[idx++] = (byte) (val & 0x00ff);
				generatedSnd[idx++] = (byte) ((val & 0xff00) >>> 8);
			}

			if (audioTrack != null) {
				audioTrack.release();
				audioTrack = null;
			}
			audioTrack = new AudioTrack(AudioManager.STREAM_MUSIC, sampleRate,
					AudioFormat.CHANNEL_CONFIGURATION_MONO,
					AudioFormat.ENCODING_PCM_16BIT, numSamples,
					AudioTrack.MODE_STATIC);
			audioTrack.write(generatedSnd, 0,
					(int) (generatedSnd.length / beepDevider));
			audioTrack.play();

		} catch (Exception exception) {
			exception.printStackTrace();
		}

	}

	private void showAlert(Context context, final Timer timer) {
		try {
			AlertDialog.Builder alertdialogBuilder = new AlertDialog.Builder(
					context);
			alertdialogBuilder.setMessage("The Can is full").setCancelable(
					false).setPositiveButton("OK",
					new DialogInterface.OnClickListener() {
						public void onClick(DialogInterface dialog, int id) {
							timer.cancel();
						}
					}).setNegativeButton("Cancel",
					new DialogInterface.OnClickListener() {
						public void onClick(DialogInterface dialog, int id) {

							timer.cancel();
							dialog.cancel();
						}
					});
			AlertDialog alert = alertdialogBuilder.create();
			alert.setTitle("The Can is full");

			alert.show();
		} catch (Exception exception) {
			exception.printStackTrace();
		}

	}

	private HashMap<String, String> getCanStatus(String canId) {
		dateTime = new Date();
		HashMap<String, String> canStatus = new HashMap<String, String>();
		restClient = new RestClient("http://" + ipAddressForWebService
				+ getString(R.string.url_GetCanStatus)
				+ getString(R.string.url_GetCanStatus_CanId) + canId + "&"
				+ getString(R.string.url_GetCanStatus_EventTime)
				+ dateFormatter.format(dateTime));

		try {
			restClient.Execute(RequestMethod.GET);
			Log.d(tag, "http://" + ipAddressForWebService
					+ getString(R.string.url_GetCanStatus)
					+ getString(R.string.url_GetCanStatus_CanId) + canId + "&"
					+ getString(R.string.url_GetCanStatus_EventTime)
					+ dateFormatter.format(dateTime));
			String canStatusResponse = null;
			if (restClient.getResponse() != "") {
				canStatusResponse = restClient.getResponse();
			}

			if (canStatusResponse != null) {
				JSONObject canStatusJsonObject = new JSONObject(
						canStatusResponse);

				if (canStatusJsonObject.getString("CanId") != "null") {
					canStatus.put("CanId", canStatusJsonObject
							.getString("CanId"));
				} else {
					canStatus.put("CanId", "N/A");
				}
				if(canStatusJsonObject.get("StatusType")!=null)
				{
					canStatus.put("CanStatusType",canStatusJsonObject.get("StatusType").toString());				
					
				}
				else
				{
					canStatus.put("CanStatusType","N/A");
				}

				if (canStatusJsonObject.getString("StatusDescription") != "null") {
					canStatus.put("CanStatus", canStatusJsonObject
							.getString("StatusDescription"));
				} else {
					canStatus.put("CanStatus", "N/A");
				}
			}
			else {
				canStatus.put("CanId", "N/A");
				canStatus.put("CanStatus", "N/A");
				canStatus.put("CanStatusType","N/A");
			}
			return canStatus;
		} catch (Exception exception) {
			exception.printStackTrace();
			return null;
		}
	}
}
