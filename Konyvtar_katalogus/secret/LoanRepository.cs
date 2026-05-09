using Konyvtar_katalogus.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Konyvtar_katalogus.Models;


namespace Konyvtar_katalogus.Repository
{
    public class LoanRepository : ILoanRepository
    {
        private LibraryDbContext context;
        public LoanRepository( LibraryDbContext _context)
            { 
            this.context = _context;
            }
        public IEnumerable<Models.Loan> GetLoans()
        {
            return context.Loans.ToList();
        }
        public Loan GetLoanById(int id)
        {
            return context.Loans.Find(id);
        }
        public void DeleteLoan(int id)
        {
            Models.Loan loan = context.Loans.Find(id);
            if (loan != null)
            {
                context.Loans.Remove(loan);
            }
        }
        public void UpdateLoan(Models.Loan loan)
        {
            context.Loans.Update(loan);
        }
        public void AddLoan(Models.Loan loan)
        {
            context.Loans.Add(loan);
        }
    }
}
