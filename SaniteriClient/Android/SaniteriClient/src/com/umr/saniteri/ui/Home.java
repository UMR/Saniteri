package com.umr.saniteri.ui;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.UUID;
import org.apache.http.NameValuePair;
import org.json.JSONArray;

import com.umr.saniteri.ui.*;
import com.umr.saniteri.connection.RestClient;
import com.umr.saniteri.connection.RestClient.RequestMethod;
import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;
import android.widget.Toast;
import android.widget.AdapterView.OnItemSelectedListener;

public class Home extends Activity {
	/** Called when the activity is first created. */
	Button btnOpen, btnClose;
	RestClient restClient;
	UUID id;
	Date dateTime;
	public static final String tag = "Home";
	Spinner ddlUnitNumber;
	ArrayAdapter<String> ddlUnitNumberAdapter;
	ArrayList<String> listOfUnitNumbers;
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);
		initializeUIControls();
		loadInitialData();
		registerEvents();
	}

	private void loadInitialData() {
		// TODO Auto-generated method stub		
		restClient = new RestClient("http://172.16.205.56/tt/SaniteriService/GetAllCanId");
		try {
			restClient.Execute(RequestMethod.GET);
			String response = restClient.getResponse();

			JSONArray responseArray = new JSONArray(response);
			for (int i = 0; i < responseArray.length(); ++i) {
				String unitNumber = responseArray.get(i).toString();
				listOfUnitNumbers.add(unitNumber);
			}
			
			ddlUnitNumberAdapter = new ArrayAdapter<String>(this,
					android.R.layout.simple_list_item_1, listOfUnitNumbers);
			ddlUnitNumberAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
			ddlUnitNumber.setAdapter(ddlUnitNumberAdapter);

		} catch (Exception e) {
			e.printStackTrace();
		}

	}

	private void registerEvents() {
		// TODO Auto-generated method stub
		btnOpen.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				id = UUID.randomUUID();
				restClient = new RestClient(
						"http://172.16.205.56/tt/SaniteriService/InsertCanCommand");
				restClient.AddHeader("Content-Type", "application/json");
				restClient.AddParam("commandId", id.toString());
				restClient.AddParam("canId",
						"2db0f9f1-027a-45f2-bd87-4d631b413883");
				restClient.AddParam("canLidStatus", "1");
				dateTime = new Date();
				restClient.AddParam("commandTimeStamp", DateFormat
						.getInstance().format(dateTime));

				try {
					restClient.Execute(RequestMethod.POST);

					if (restClient.getResponseCode() == 201) {
						Toast.makeText(Home.this, "created", Toast.LENGTH_LONG)
								.show();
					} else if (restClient.getResponseCode() == 400) {
						Toast.makeText(Home.this, "Request failed",
								Toast.LENGTH_LONG).show();
					} else if (restClient.getResponseCode() == 409) {
						Toast.makeText(Home.this, "Command already taken.",
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
				restClient = new RestClient("http://172.16.205.56/tt/SaniteriService/InsertCanCommand");
				restClient.AddHeader("Content-Type", "application/json");
				restClient.AddParam("commandId", id.toString());
				restClient.AddParam("canId",
						"2db0f9f1-027a-45f2-bd87-4d631b413883");
				restClient.AddParam("canLidStatus", "0");
				dateTime = new Date();
				restClient.AddParam("commandTimeStamp", DateFormat
						.getInstance().format(dateTime));

				try {
					restClient.Execute(RequestMethod.POST);
					Log.d(tag, restClient.getResponse());
					if (restClient.getResponseCode() == 201) {
						Toast.makeText(Home.this, "done", Toast.LENGTH_LONG)
								.show();
					} else if (restClient.getResponseCode() == 400) {
						Toast.makeText(Home.this, "Request failed",
								Toast.LENGTH_LONG).show();
					} else if (restClient.getResponseCode() == 409) {
						Toast.makeText(Home.this, "Command already taken.",
								Toast.LENGTH_LONG).show();
					}
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		});

		ddlUnitNumber.setOnItemSelectedListener(new OnItemSelectedListener() {

			@Override
			public void onItemSelected(AdapterView<?> arg0, View arg1,
					int arg2, long arg3) {
				// TODO Auto-generated method stub

			}

			@Override
			public void onNothingSelected(AdapterView<?> arg0) {
				// TODO Auto-generated method stub

			}
		});
	}		

	private void initializeUIControls() {
		// TODO Auto-generated method stub
		btnOpen = (Button) findViewById(R.id.btnOpen);
		btnClose = (Button) findViewById(R.id.btnClose);
		ddlUnitNumber = (Spinner) findViewById(R.id.ddlUnitNumber);
		listOfUnitNumbers = new ArrayList<String>();
	

	}
}