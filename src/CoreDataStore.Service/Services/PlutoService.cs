using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Models;
using Navigator.Common.Helpers;

namespace CoreDataStore.Service.Services
{
    public class PlutoService : IPlutoService
    {
        private readonly IPlutoRepository _plutoRepository;

        private readonly IMapper _mapper;
        public PlutoService(IPlutoRepository plutoRepository, IMapper mapper)
        {
            _plutoRepository = plutoRepository ?? throw new ArgumentNullException(nameof(plutoRepository));
            _mapper = mapper;
        }

        public List<PlutoModel> GetPluto(string lpcNumber)
        {
            Guard.ThrowIfNullOrWhitespace(lpcNumber, "LPC Number");

            var results = _plutoRepository.GetPluto(lpcNumber);
            return _mapper.Map<IEnumerable<Pluto>, IEnumerable<PlutoModel>>(results).ToList();
        }

        public async Task<List<PlutoModel>> GetPlutoAsync(string lpcNumber)
        {
            Guard.ThrowIfNullOrWhitespace(lpcNumber, "LPC Number");

            var results = await _plutoRepository.GetPlutoAsync(lpcNumber);
            return _mapper.Map<IEnumerable<Pluto>, IEnumerable<PlutoModel>>(results).ToList();
        }

        public async Task<PlutoModel> GetPlutoAsync(long bbl)
        {
            Guard.ThrowIfZeroOrLess(bbl, "BBL Number");
            var result = await _plutoRepository.FindByAsync(x => x.BBL == bbl).ConfigureAwait(false);

            return _mapper.Map<Pluto, PlutoModel>(result.FirstOrDefault());
        }

        public async Task<List<PlutoModel>> GetPlutoListAsync(long[] bbl)
        {
            Guard.ThrowIfZeroOrLess(bbl.Length, "BBL Number");
            var result = await _plutoRepository.FindByAsync(x => bbl.Contains(x.BBL));  

            return _mapper.Map<List<Pluto>, List<PlutoModel>>(result.ToList());
        }
    }
}
