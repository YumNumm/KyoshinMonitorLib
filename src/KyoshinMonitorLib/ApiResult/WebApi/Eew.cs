﻿using System.Runtime.Serialization;

namespace KyoshinMonitorLib.ApiResult.WebApi
{
	public class Eew
	{
		[DataMember(Name = "result")]
		public Result Result { get; set; }
		[DataMember(Name = "report_time")]
		public string ReportTime { get; set; }
		[DataMember(Name = "region_code")]
		public string RegionCode { get; set; }
		[DataMember(Name = "request_time")]
		public string RequestTime { get; set; }
		[DataMember(Name = "region_name")]
		public string RegionName { get; set; }

	    [DataMember(Name = "longitude")] public string _longitudeRaw;

	    public float? Longitude
        {
            get => this._longitudeRaw != null && float.TryParse(this._longitudeRaw, out var lon) ? lon : null as float?;
            set => this._longitudeRaw = value?.ToString();
        }

        [DataMember(Name = "is_cancel")]
		public bool? IsCancel { get; set; }
		[DataMember(Name = "depth")]
		public string Depth { get; set; }
		[DataMember(Name = "calcintensity")]
		public string Calcintensity { get; set; }
		[DataMember(Name = "is_final")]
		public bool? IsFinal { get; set; }
		[DataMember(Name = "isTraining")]
		public bool? IsTraining { get; set; }

	    [DataMember(Name = "latitude")] public string _latitudeRaw;
	    public float? Latitude
	    {
	        get => this._latitudeRaw != null && float.TryParse(this._latitudeRaw, out var lat) ? lat : null as float?;
	        set => this._latitudeRaw = value?.ToString();
	    }

        [DataMember(Name = "origin_time")]
		public string OriginTime { get; set; }
		[DataMember(Name = "security")]
		public Security Security { get; set; }
		[DataMember(Name = "magunitude")]
		public string Magunitude { get; set; }
		[DataMember(Name = "report_num")]
		public string ReportNum { get; set; }
		[DataMember(Name = "request_hypo_type")]
		public string RequestHypoType { get; set; }
		[DataMember(Name = "report_id")]
		public string ReportId { get; set; }
		[DataMember(Name = "alertflg")]
		public string AlertFlag { get; set; }
	}
}
