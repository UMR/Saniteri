package com.umr.saniteri.ui;

import java.util.ArrayList;
import java.util.Date;
import java.util.UUID;
import org.apache.http.NameValuePair;
import com.umr.saniteri.ui.*;
import com.umr.saniteri.connection.RestClient;
import com.umr.saniteri.connection.RestClient.RequestMethod;
import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.Toast;

public class Home extends Activity {
	/** Called when the activity is first created. */
	Button btnOpen, btnClose;
	RestClient restClient;
	UUID id;
	Date dateTime;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);
		initializeUIControls();
		registerEvents();
	}

	private void registerEvents() {
		// TODO Auto-generated method stub
		btnOpen.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				id = UUID.randomUUID();
				restClient.AddParam("Id", id.toString());
				restClient.AddParam("CanId", id.toString());
				restClient.AddParam("CanLidStatus", "1");
				dateTime = new Date();
				restClient.AddParam("CommandTimeStamp", dateTime.toString());
				try {
					restClient.Execute(RequestMethod.POST);

					if (restClient.getResponseCode() == 200) {
						Toast.makeText(Home.this, "done", Toast.LENGTH_LONG);
					} else {
						Toast.makeText(Home.this, "Request Error",
								Toast.LENGTH_LONG);
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
				restClient.AddParam("Id", id.toString());
				restClient.AddParam("CanId", id.toString());
				restClient.AddParam("CanLidStatus", "0");
				dateTime = new Date();
				restClient.AddParam("CommandTimeStamp", dateTime.toString());
				try {
					restClient.Execute(RequestMethod.POST);
					if (restClient.getResponseCode() == 200) {
						Toast.makeText(Home.this, "done", Toast.LENGTH_LONG);
					} else {
						Toast.makeText(Home.this, "Request Error",
								Toast.LENGTH_LONG);
					}
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		});

	}

	private void initializeUIControls() {
		// TODO Auto-generated method stub
		btnOpen = (Button) findViewById(R.id.btnOpen);
		btnClose = (Button) findViewById(R.id.btnClose);
		restClient = new RestClient("");

	}
}