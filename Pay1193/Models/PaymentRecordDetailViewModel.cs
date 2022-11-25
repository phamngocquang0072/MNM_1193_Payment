using Pay1193.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Pay1193.Models
{
    public class PaymentRecordDetailViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime DatePay { get; set; }
        [Display(Name = "Pay Month")]
        public string MonthPay { get; set; }
        [Display(Name = "Tax Year")]
        public int TaxYearId { get; set; }
        public TaxYear TaxYear { get; set; }
        [Display(Name = "Tax Code")]
        public string TaxCode { get; set; }
        [Display(Name = "Hourly Rate")]
        public decimal HourslyRate { get; set; }
        [Display(Name = "Hourly Worked")]
        public decimal HourlyWorked { get; set; }
        [Display(Name = "Contractual Hours")]
        public decimal ContractualHours { get; set; }
        [Display(Name = "Overtime Hours")]
        public decimal OvertimeHours { get; set; }
        [Display(Name = "Overtime Rate")]
        public decimal OvertimeRate { get; set; }
        [Display(Name = "Contractual Earnings")]
        public decimal ContractualEarnings { get; set; }
        [Display(Name = "Overtime Earnings")]
        public decimal OvertimeEarnings { get; set; }
        public decimal Tax { get; set; }
        public decimal Nic { get; set; }
        [Display(Name = "Union Fee")]
        public decimal? UnionFee { get; set; }
        public Nullable<decimal> SLC { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetPayment { get; set; }
    }
}
