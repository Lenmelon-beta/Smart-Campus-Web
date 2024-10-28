using System;

namespace CustomAuth.Models
{
    public class IncidentReport
    {
        public int ReportId { get; set; } // Unique ID for the report
        public string SubmittedBy { get; set; } // Username of the person submitting
        public string Email { get; set; } // Email address of the submitter
        public string Title { get; set; } // Title of the incident
        public string Description { get; set; } // Description of the incident
        public DateTime SubmittedDate { get; set; } // Date the report was submitted
    }
}
