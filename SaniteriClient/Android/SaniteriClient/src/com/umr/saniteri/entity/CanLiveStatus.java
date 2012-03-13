package com.umr.saniteri.entity;

public class CanLiveStatus {
	private int canId;
	private int powerStatus;
	private boolean needService;
	private int communicationStatus;
	private boolean lidOpen;
	private boolean doorOpen;
	private String fault;
	private String bagInfo;
	private double weight;

	public void setCanId(int canId) {
		this.canId = canId;
	}

	public int getCanId() {
		return canId;
	}

	public void setPowerStatus(int powerStatus) {
		this.powerStatus = powerStatus;
	}

	public int getPowerStatus() {
		return powerStatus;
	}

	public void setNeedService(boolean needService) {
		this.needService = needService;
	}

	public boolean isNeedService() {
		return needService;
	}

	public void setCommunicationStatus(int communicationStatus) {
		this.communicationStatus = communicationStatus;
	}

	public int getCommunicationStatus() {
		return communicationStatus;
	}

	public void setLidOpen(boolean lidOpen) {
		this.lidOpen = lidOpen;
	}

	public boolean isLidOpen() {
		return lidOpen;
	}

	public void setDoorOpen(boolean doorOpen) {
		this.doorOpen = doorOpen;
	}

	public boolean isDoorOpen() {
		return doorOpen;
	}

	public void setFault(String fault) {
		this.fault = fault;
	}

	public String getFault() {
		return fault;
	}

	public void setBagInfo(String bagInfo) {
		this.bagInfo = bagInfo;
	}

	public String getBagInfo() {
		return bagInfo;
	}

	public void setWeight(double weight) {
		this.weight = weight;
	}

	public double getWeight() {
		return weight;
	}

}
