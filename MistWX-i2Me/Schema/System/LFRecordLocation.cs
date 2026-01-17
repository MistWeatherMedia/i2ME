using System.ComponentModel.DataAnnotations;

namespace MistWX_i2Me.Schema.System;

public class LFRecordLocation
{
    [Key] 
    public string locId { get; set; } = string.Empty;
    public string locType { get; set; } = string.Empty;
    public string cityNm { get; set; } = string.Empty;
    public string stCd { get; set; } = string.Empty;
    public string? prsntNm { get; set; }
    public string? cntryCd { get; set; }
    public string? coopId { get; set; }
    public string? lat { get; set; }
    public string? @long { get; set; }
    public string? obsStn { get; set; }
    public string? secObsStn { get; set; }
    public string? tertObsStn { get; set; }
    public string? gmtDiff { get; set; }
    public string? regSat { get; set; }
    public string? cntyId { get; set; }
    public string? cntyNm { get; set; }
    public string? zoneId { get; set; }
    public string? cntyFips { get; set; }
    public string? active { get; set; }
    public string? dySTInd { get; set; }
    public string? zip2locId { get; set; }
    public string? elev { get; set; }
    public string? cliStn { get; set; }
    public string? tmZnNm { get; set; }
    public string? tmZnAbbr { get; set; }
    public string? dySTAct { get; set; }
    public string? clsRad { get; set; }
    public string? metRad { get; set; }
    public string? ultRad { get; set; }
    public string? ssRad { get; set; }
    public string? lsRad { get; set; }
    public string? siteId { get; set; }
    public string? idxId { get; set; }
    public string? primTecci { get; set; }
    public string? secTecci { get; set; }
    public string? tertTecci { get; set; }
    public string? arptId { get; set; }
    public string? mrnZoneId { get; set; }
    public string? pllnId { get; set; }
    public string? skiId { get; set; }
    public string? tideId { get; set; }
    public string? epaId { get; set; }
    public string? tPrsntNm { get; set; }
    public string? wrlsPrsntNm { get; set; }
    public string? wmoId { get; set; }

    public string? dmaCd { get; set; }
}