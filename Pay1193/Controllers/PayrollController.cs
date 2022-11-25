using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pay1193.Entity;
using Pay1193.Models;
using Pay1193.Services;
using Pay1193.Services.Implement;

namespace Pay1193.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IPayService _paySevices;
        private readonly IEmployee _employeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceService _nationalInsuranceService;
        private decimal overtimeHrs;
        private decimal contractualEarnings;
        private decimal overtimeEarnings;
        private decimal totalEarnings;
        private decimal tax;
        private decimal unionFee;
        private decimal studentLoan;
        private decimal nationalInsurance;
        private decimal totalDeduction;

        public PayrollController(IPayService payService, IEmployee employeeService, ITaxService taxService, INationalInsuranceService nationalInsuranceService)
        {
            _paySevices = payService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceService = nationalInsuranceService;
        }

        public IActionResult Index()
        {
            var payRecords = _paySevices.GetAll().Select(pay => new PaymentRecordIndexViewModel
            {
                Id = pay.Id,
                EmployeeId = pay.EmployeeId,
                Employee = _employeeService.GetById(pay.EmployeeId),
                DatePay = pay.DatePay,
                MonthPay = pay.MonthPay,
                TaxYearId = pay.TaxYearId,
                TaxYear = _paySevices.GetTaxYearById(pay.TaxYearId).YearOfTax,
                TotalEarnings = pay.TotalEarnings,
                TotalDeduction = pay.EarningDeduction,
                NetPayment = pay.NetPayment,
            });
            return View(payRecords);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var employData = _employeeService.GetAll();
            var taxYearData = _paySevices.GetAllTaxYear();

            var model = new PaymentRecordCreateViewModel();
            model.EmployeeList = new List<SelectListItem>();
            model.TaxYearList = new List<SelectListItem>();

            foreach (var employee in employData)
            {
                model.EmployeeList.Add(new SelectListItem { Text = employee.FullName, Value = employee.Id.ToString() });
            }

            foreach (var taxYear in taxYearData)
            {
                model.TaxYearList.Add(new SelectListItem { Text = taxYear.YearOfTax, Value = taxYear.Id.ToString() });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentRecordCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var payRecord = new PaymentRecord()
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    Employee = _employeeService.GetById(model.EmployeeId),
                    DatePay = model.DatePay,
                    MonthPay = model.MonthPay,
                    TaxYearId = model.TaxYearId,
                    TaxYear = _paySevices.GetTaxYearById(model.TaxYearId),
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourslyRate,
                    HourWorked = model.HourlyWorked,
                    ContractualHours = model.ContractualHours,
                    OvertimeHours = overtimeHrs = _paySevices.OverTimeHours(model.HourlyWorked, model.ContractualHours),
                    ContractualEarnings = contractualEarnings = _paySevices.ContractualEarning(model.ContractualHours, model.HourlyWorked, model.HourslyRate),
                    OvertimeEarnings = overtimeEarnings = _paySevices.OvertimeEarnings(_paySevices.OvertimeRate(model.HourslyRate), overtimeHrs),
                    TotalEarnings = totalEarnings = _paySevices.TotalEarnings(overtimeEarnings, contractualEarnings),
                    Tax = tax = _taxService.TaxAmount(totalEarnings),
                    UnionFee = unionFee = _employeeService.UnionFee(model.EmployeeId),
                    NiC = nationalInsurance = _nationalInsuranceService.NIContribution(totalEarnings),
                    EarningDeduction = totalDeduction = _paySevices.TotalDeduction(tax, nationalInsurance, studentLoan, unionFee),
                    NetPayment = _paySevices.NetPay(totalEarnings, totalDeduction)
                };
                await _paySevices.CreateAsync(payRecord);
                return RedirectToAction("Index");
            }

            var employData = _employeeService.GetAll();
            var taxYearData = _paySevices.GetAllTaxYear();

            model.EmployeeList = new List<SelectListItem>();
            model.TaxYearList = new List<SelectListItem>();

            foreach (var employee in employData)
            {
                model.EmployeeList.Add(new SelectListItem { Text = employee.FullName, Value = employee.Id.ToString() });
            }

            foreach (var taxYear in taxYearData)
            {
                model.TaxYearList.Add(new SelectListItem { Text = taxYear.YearOfTax, Value = taxYear.Id.ToString() });
            }

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var paymentRecord = _paySevices.GetById(id);
            if (paymentRecord == null)
            {
                return NotFound();
            }

            var model = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                Employee = _employeeService.GetById(paymentRecord.EmployeeId),
                DatePay = paymentRecord.DatePay,
                MonthPay = paymentRecord.MonthPay,
                TaxYearId = paymentRecord.TaxYearId,
                TaxYear = _paySevices.GetTaxYearById(paymentRecord.TaxYearId),
                TaxCode = paymentRecord.TaxCode,
                HourslyRate = paymentRecord.HourlyRate,
                HourlyWorked = paymentRecord.HourWorked,
                ContractualHours = paymentRecord.ContractualHours,
                OvertimeHours = paymentRecord.OvertimeHours,
                OvertimeRate = _paySevices.OvertimeRate(paymentRecord.HourlyRate),
                ContractualEarnings = paymentRecord.ContractualEarnings,
                OvertimeEarnings = paymentRecord.OvertimeEarnings,
                Tax = paymentRecord.Tax,
                Nic = paymentRecord.NiC,
                UnionFee = paymentRecord.UnionFee,
                SLC = paymentRecord.SLC,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalDeduction = paymentRecord.EarningDeduction,
                NetPayment = paymentRecord.NetPayment,
            };
            return View(model);
        }

    }
}
