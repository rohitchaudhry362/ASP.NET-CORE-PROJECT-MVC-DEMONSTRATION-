using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace RCPatients.Models
{
    [ModelMetadataTypeAttribute(typeof(PatientTreatmentMetaData))]
    public partial class PatientTreatment
    {
    }
    public class PatientTreatmentMetaData
    {
        public int PatientTreatmentId { get; set; }
        public int TreatmentId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy hh:mm}")]
        public DateTime DatePrescribed { get; set; }
        public string Comments { get; set; }
        public int PatientDiagnosisId { get; set; }
    }
}
