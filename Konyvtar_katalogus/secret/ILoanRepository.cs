using System;
using System.Collections.Generic;
using System.Text;
using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Repository
{
    public interface ILoanRepository
    {
        public void AddLoan(Loan loan);
        public void UpdateLoan(Loan loan);
        public void DeleteLoan(int id);
        public IEnumerable<Loan> GetLoans();
        public Loan GetLoanById(int id);


    }
}
