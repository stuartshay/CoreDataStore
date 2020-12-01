using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using CoreDataStore.Data.Extensions;
using CoreDataStore.Data.Filters;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Models;
using Navigator.Common.Helpers;

namespace CoreDataStore.Service.Services
{
    public class LandmarkService : ILandmarkService
    {
        private readonly ILandmarkRepository _landmarkRepository;

        private readonly IMapper _mapper;

        public LandmarkService(ILandmarkRepository landmarkRepository, IMapper mapper)
        {
            _landmarkRepository = landmarkRepository ?? throw new ArgumentNullException(nameof(landmarkRepository));
            _mapper = mapper;
        }

        public List<string> GetLandmarkStreets(string lpcNumber)
        {
            Guard.ThrowIfNullOrWhitespace(lpcNumber, "LPC Number");

            var predicate = PredicateBuilder.True<Landmark>();
            predicate = predicate.And(x => x.LP_NUMBER == lpcNumber);

            var results = _landmarkRepository.FindBy(predicate).Select(x => x.PLUTO_ADDR)
                          .Select(x => new
                          {
                              x = !string.IsNullOrWhiteSpace(x) && x.Any(char.IsDigit)
                             ? Regex.Replace(x, @"^[\d-]*\s*", "", RegexOptions.Multiline)
                             : x,
                          }).Distinct().ToList();

            var list = new List<string>();
            foreach (var i in results)
            {
                list.Add(i.x);
            }

            return list.OrderBy(x => x).ToList();
        }

        public async Task<List<string>> GetLandmarkStreetsAsync(string lpcNumber)
        {
            Guard.ThrowIfNullOrWhitespace(lpcNumber, "LPC Number");

            var predicate = PredicateBuilder.True<Landmark>();
            predicate = predicate.And(x => x.LP_NUMBER == lpcNumber);

            var results = await _landmarkRepository.FindByAsync(predicate);
            var items = results.Select(x => x.PLUTO_ADDR)
                .Select(x => new
                {
                    x = !string.IsNullOrWhiteSpace(x) && x.Any(char.IsDigit)
                        ? Regex.Replace(x, @"^[\d-]*\s*", "", RegexOptions.Multiline)
                        : x,
                }).Distinct().ToList();

            var list = new List<string>();
            foreach (var i in items)
            {
                list.Add(i.x);
            }

            return list.OrderBy(x => x).ToList();
        }

        public PagedResultModel<LandmarkModel> GetLandmarks(LandmarkRequest request)
        {
            var predicate = PredicateBuilder.True<Landmark>();

            if (!string.IsNullOrEmpty(request.LpcNumber))
                predicate = predicate.And(x => x.LP_NUMBER == request.LpcNumber);

            var sortModel = new SortModel
            {
                SortColumn = !string.IsNullOrEmpty(request.SortColumn) ? request.SortColumn : null,
                SortOrder = !string.IsNullOrEmpty(request.SortOrder) ? request.SortOrder : null
            };

            var sortingList = new List<SortModel>();
            sortingList.Add(sortModel);

            int totalCount = _landmarkRepository.FindBy(predicate).Count();

            var results = _landmarkRepository
                .GetPage(predicate, request.PageSize * (request.Page - 1), request.PageSize, sortingList);

            var modelData = _mapper.Map<IEnumerable<Landmark>, IEnumerable<LandmarkModel>>(results).ToList();
            return new PagedResultModel<LandmarkModel>
            {
                Total = totalCount,
                Page = request.Page,
                Limit = request.PageSize,
                Results = modelData,
            };
        }
    }
}
