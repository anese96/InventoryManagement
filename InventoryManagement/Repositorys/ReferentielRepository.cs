using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesRepositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    /// <summary>
    /// Identifies which referentiel table to target.
    /// </summary>
    public enum ReferentielType { Categorie, Unite, Nature, Marque }

    /// <summary>
    /// Generic repository for all simple BaseEntity referentiels (Catégorie, Unité, Nature, Marque).
    /// One instance per tab — the caller supplies <see cref="ReferentielType"/> to choose the table.
    /// </summary>
    public class ReferentielRepository : IRepository<ReferentielDto>
    {
        private readonly AppDbContext    _db;
        private readonly AddEntityBD     _audit;
        private readonly ReferentielType _type;

        public ReferentielRepository(AppDbContext db, AddEntityBD audit, ReferentielType type)
        {
            _db    = db;
            _audit = audit;
            _type  = type;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private DbSet<T> Set<T>() where T : class => _db.Set<T>();

        private async Task<BaseEntity> FindAsync(int id)
        {
            return _type switch
            {
                ReferentielType.Categorie => await _db.Categories.FindAsync(id),
                ReferentielType.Unite     => await _db.Units     .FindAsync(id),
                ReferentielType.Nature    => await _db.Natures   .FindAsync(id),
                ReferentielType.Marque    => await _db.Marques   .FindAsync(id),
                _ => throw new NotSupportedException()
            };
        }

        private BaseEntity CreateEntity(ReferentielDto dto)
        {
            return _type switch
            {
                ReferentielType.Categorie => new Category { Name = dto.Name },
                ReferentielType.Unite     => new Unit     { Name = dto.Name },
                ReferentielType.Nature    => new Nature   { Name = dto.Name },
                ReferentielType.Marque    => new Marque   { Name = dto.Name },
                _ => throw new NotSupportedException()
            };
        }

        private async Task<List<BaseEntity>> GetAllEntitiesAsync()
        {
            return _type switch
            {
                ReferentielType.Categorie => (await _db.Categories.ToListAsync()).Cast<BaseEntity>().ToList(),
                ReferentielType.Unite     => (await _db.Units     .ToListAsync()).Cast<BaseEntity>().ToList(),
                ReferentielType.Nature    => (await _db.Natures   .ToListAsync()).Cast<BaseEntity>().ToList(),
                ReferentielType.Marque    => (await _db.Marques   .ToListAsync()).Cast<BaseEntity>().ToList(),
                _ => throw new NotSupportedException()
            };
        }

        private static ReferentielDto ToDto(BaseEntity e)
            => new ReferentielDto { Id = e.Id, Name = ((BaseEntity)e).Name };

        // ── IRepository<ReferentielDto> ───────────────────────────────────────

        public async Task Insert(ReferentielDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Le nom est obligatoire.");

            var entity = CreateEntity(dto);
            _db.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Update(ReferentielDto dto, int id)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Le nom est obligatoire.");

            var entity = await FindAsync(id)
                ?? throw new Exception("Enregistrement introuvable.");

            entity.Name = dto.Name;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await FindAsync(id)
                ?? throw new Exception("Enregistrement introuvable.");

            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<ReferentielDto> GetById(int id)
        {
            var entity = await FindAsync(id)
                ?? throw new Exception("Enregistrement introuvable.");
            return ToDto(entity);
        }

        public async Task<List<ReferentielDto>> GetAll()
        {
            var entities = await GetAllEntitiesAsync();
            return entities.Select(ToDto).OrderBy(d => d.Name).ToList();
        }
    }
}
