package com.umr.saniteri.ui;

import android.os.Bundle;
import android.preference.PreferenceActivity;

public class WebServicePreference extends PreferenceActivity{

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		addPreferencesFromResource(R.xml.webservicepreference);      
	}	

}
