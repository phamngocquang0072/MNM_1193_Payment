﻿using Pay1193.Entity;
using Pay1193.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Services.Implement
{
    public class PayService : IPayService
    {
        private decimal overTimeHours;
        private decimal contractualEarnings;
        private readonly ApplicationDbContext _context;
        public PayService(ApplicationDbContext context)
        {
            _context = context;
        }
        public decimal ContractualEarning(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            if(hoursWorked < contractualHours)
            {
                contractualEarnings = hoursWorked * hourlyRate;

            }
            else
            {
                contractualEarnings = contractualHours * hourlyRate;
            }
            return contractualEarnings;
        }

        public async Task CreateAsync(PaymentRecord paymentRecord)
        {
            await _context.PaymentRecords.AddAsync(paymentRecord);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PaymentRecord> GetAll() => _context.PaymentRecords.OrderBy(p => p.Id).ToList();

        public IEnumerable<TaxYear> GetAllTaxYear() => _context.TaxYears.OrderBy(t => t.Id).ToList();

        public PaymentRecord GetById(int id) => _context.PaymentRecords.Where(p => p.Id == id).FirstOrDefault();

        public TaxYear GetTaxYearById(int id) => _context.TaxYears.Where(t => t.Id == id).FirstOrDefault();

        public decimal NetPay(decimal totalEarnings, decimal totalDeduction)
        {
            return totalEarnings - totalDeduction;
        }

        public decimal OvertimeEarnings(decimal overtimeRate, decimal overTimeHours)
        {
            return overTimeHours * overtimeRate;
        }
        //update 22/11
        public decimal OverTimeHours(decimal hoursWorked, decimal contractualHours)
        {
            if(hoursWorked <= contractualHours)
            {
                overTimeHours = 0.00m;
            }
            else
            {
                overTimeHours = hoursWorked - contractualHours;
            }
            return overTimeHours;
        }

        public decimal OvertimeRate(decimal hourlyRate)
        {
            return hourlyRate * 1.5m;
        }

        public decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal unionFees)
        {
            return tax + nic + studentLoanRepayment + unionFees;
        }
        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings) => overtimeEarnings + contractualEarnings;
    }
}
