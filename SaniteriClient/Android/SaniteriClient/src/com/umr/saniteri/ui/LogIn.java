package com.umr.saniteri.ui;

import org.json.JSONException;
import org.json.JSONObject;
import com.umr.saniteri.connection.RestClient;
import com.umr.saniteri.connection.RestClient.RequestMethod;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.preference.PreferenceManager;
import android.sax.StartElementListener;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.Window;
import android.view.View.OnClickListener;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.Toast;

public class LogIn extends Activity {
	EditText txtUserId, txtPassword;
	String userId, password;
	Button btnLogIn;
	RestClient restClient;
	SharedPreferences sharedpreference;
	String ipAddressForWebService;
	private static String tag = "LogIn";
	static Handler exceptionHandler;
	LinearLayout layoutLogIn;
	InputMethodManager imm;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		super.onCreate(savedInstanceState);
		setContentView(R.layout.login);
		initializeUIcontrols();
		registerEvents();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.menu, menu);
		return super.onCreateOptionsMenu(menu);
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		switch (item.getItemId()) {
		case R.id.menuitem_Settings:
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

		layoutLogIn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				imm = (InputMethodManager) getSystemService(INPUT_METHOD_SERVICE);
				imm.hideSoftInputFromWindow(v.getWindowToken(), 0);
			}

		});

		btnLogIn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				userId = txtUserId.getText().toString();
				password = txtPassword.getText().toString();

				if (LogIn(userId, password)) {
					Intent deviceListIntent = new Intent(LogIn.this,
							DeviceList.class);
					startActivity(deviceListIntent);
				}

			}

			private boolean LogIn(String userId, String password) {
				// TODO Auto-generated method stub
				restClient = new RestClient("http://" + ipAddressForWebService
						+ getString(R.string.url_LogIn));
				Log.d(tag, "http://" + ipAddressForWebService
						+ getString(R.string.url_LogIn));
				restClient.AddParam("userID", userId);
				restClient.AddParam("password", password);

				try {
					restClient.Execute(RequestMethod.GET);

					String logInResponseString = restClient.getResponse();

					return true;
					//					
					// if((logInResponseString.compareToIgnoreCase("true")==0))
					// {
					// Log.d(tag,logInResponseString+" true ");
					// return true;
					// }
					// else
					// {
					// return false;
					// }

				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

				return false;
			}
		}

		);
	}

	private void initializeUIcontrols() {
		// TODO Auto-generated method stub

		txtUserId = (EditText) findViewById(R.id.txtUserId);
		txtPassword = (EditText) findViewById(R.id.txtPassword);
		btnLogIn = (Button) findViewById(R.id.btnLogIn);
		sharedpreference = PreferenceManager.getDefaultSharedPreferences(this);
		ipAddressForWebService = sharedpreference.getString(
				"IpAddressForWebService", "172.16.205.56");
		layoutLogIn = (LinearLayout) findViewById(R.id.layoutLogIn);
		exceptionHandler = new Handler();

	}

	public static class LogInTask extends AsyncTask<Void, Void, Void> {
		ProgressDialog dialog;
		RestClient restClient;
		String userID, password;
		Context context;
		Activity activity;

		public LogInTask(String userID, String password, Context context,
				Activity activity) {
			this.userID = userID;
			this.password = password;
			this.context = context;
			this.activity = activity;
			dialog = new ProgressDialog(context);

		}

		@Override
		protected void onPreExecute() {
			// TODO Auto-generated method stub
			super.onPreExecute();
			dialog.setMessage("Logging In...");
			dialog.show();

		}

		@Override
		protected Void doInBackground(Void... params) {
			// TODO Auto-generated method stub
			restClient = new RestClient("");
			restClient.AddHeader("Content-Type", "application/json");
			restClient.AddParam("userId", userID);
			restClient.AddParam("password", password);
			try {
				restClient.Execute(RequestMethod.POST);

			} catch (Exception exception) {
				dialog.dismiss();
				Runnable exceptionMsgRunnable = new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						Toast
								.makeText(context, "Try again.",
										Toast.LENGTH_LONG);
					}
				};
				exceptionHandler.post(exceptionMsgRunnable);
			}
			return null;
		}

		@Override
		protected void onPostExecute(Void result) {
			// TODO Auto-generated method stub
			super.onPostExecute(result);
			Log.d(tag, restClient.getResponseCode() + "");

			Intent deviceListIntent = new Intent(context, DeviceList.class);
			context.startActivity(deviceListIntent);

			// try {
			// if (restClient.getResponseCode() == 200) {
			// JSONObject loginJson = new JSONObject(restClient
			// .getResponse());
			//				
			// if(loginJson.toString()=="true")
			// {
			//						
			//							
			// }
			//				 		
			//
			//						
			//
			// } else if (restClient.getResponseCode() == 400) {
			// if (dialog.isShowing()) {
			// dialog.dismiss();
			// }
			// Toast.makeText(context, "Log In failed.Try again.",
			// Toast.LENGTH_LONG).show();
			// } else if (restClient.getResponseCode() == 409) {
			// if (dialog.isShowing()) {
			// dialog.dismiss();
			// }
			// Toast.makeText(context, "Request already taken.",
			// Toast.LENGTH_LONG).show();
			// } else {
			// if (dialog.isShowing()) {
			// dialog.dismiss();
			// }
			// Toast.makeText(context, "Log In failed.Try again.",
			// Toast.LENGTH_LONG).show();
			// }
			// } catch (Exception e) {
			// if (dialog.isShowing()) {
			// dialog.dismiss();
			// }
			// e.printStackTrace();
			// }

		}

	}
}
