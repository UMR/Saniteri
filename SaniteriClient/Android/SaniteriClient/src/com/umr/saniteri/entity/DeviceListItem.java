package com.umr.saniteri.entity;

public class DeviceListItem {

	private Integer unitId;
	private String UnitLocation;

	public void setUnitLocation(String unitLocation) {
		UnitLocation = unitLocation;
	}

	public String getUnitLocation() {
		return UnitLocation;
	}

	public void setUnitId(Integer unitId) {
		this.unitId = unitId;
	}

	public Integer getUnitId() {
		return unitId;
	}

}
