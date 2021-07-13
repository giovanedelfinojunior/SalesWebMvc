using SalesWebMvc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services {
    public class SellerService {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context) {
            _context = context;
        }
        public async Task<List<Seller>> FindAllAsync() {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj) {

            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id) {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        internal object FindAll() {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(int id) {
            try {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }catch(DbUpdateException e) {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Seller obj) {
            bool hasAny = await _context.Seller.AnyAsync(X => X.Id == obj.Id);
            if(!hasAny) {
                throw new NotFoundException("Id not found");
            }
            try {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }catch(DbUpdateException) {
                throw new IntegrityException("Can't delete seller because he/she has sales");
            }
        }

        internal object FindById(int value) {
            throw new NotImplementedException();
        }
    }
}
