using Microsoft.AspNetCore.Mvc.Rendering;
using Pay1193.Entity;
using System.ComponentModel.DataAnnotations;
namespace Pay1193.Models
{
    public class PaymentRecordCreateViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Full Name")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime DatePay { get; set; }
        [Display(Name = "Pay Month")]
        public string MonthPay { get; set; }
        [Display(Name = "Tax Year")]
        public int TaxYearId { get; set; }
        public TaxYear? TaxYear { get; set; }
        public string TaxCode { get; set; } = "1258L";
        [Display(Name = "Hourly Rate")]
        public decimal HourslyRate { get; set; }
        [Display(Name = "Hourly Worked")]
        public decimal HourlyWorked { get; set; }
        [Display(Name = "Contractual Hours")]
        public decimal ContractualHours { get; set; }
        public decimal OvertimeEarning { get; set; }
        public decimal Tax { get; set; }
        public decimal NIC { get; set; }
        public decimal? UnionFee { get; set; }
        public Nullable<decimal> SLC { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetPayment { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public List<SelectListItem> TaxYearList { get; set; }
    }
}
