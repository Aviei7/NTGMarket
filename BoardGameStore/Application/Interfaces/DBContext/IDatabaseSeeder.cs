using Domain.Models.FiltersModel;
using Domain.Models.ProductsModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.DBContext
{
    public interface IDatabaseSeeder
    {
        void Seed();
        void EnsureSystemUsers();
    }
}
