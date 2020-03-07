﻿using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Factories;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DNI.Core.UnitTests")]
namespace DNI.Core.Services.Providers
{

    internal sealed class DefaultCacheProvider : ICacheProvider
    {
        private readonly IIs _is;
        private readonly ICacheProviderFactory _cacheProviderFactory;

        public DefaultCacheProvider(IIs @is, ICacheProviderFactory cacheProviderFactory)
        {
            _is = @is;
            _cacheProviderFactory = cacheProviderFactory;
        }

        public async Task<T> Get<T>(CacheType cacheType, string cacheKeyName, CancellationToken cancellationToken = default)
        {
            var cacheService = GetCacheService(cacheType);

            return await cacheService.Get<T>(cacheKeyName, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetOrSet<T>(CacheType cacheType, string cacheKeyName, 
            Func<CancellationToken, Task<IEnumerable<T>>> getValue, 
            Func<T, object> selectId, Func<CancellationToken, Task<object>> getMaxValue, bool append = false, 
            CancellationToken cancellationToken = default)
        {
            var value = await Get<IEnumerable<T>>(cacheType, cacheKeyName, cancellationToken);

            if(value == null || !value.Any())
                return await Set(cacheType, cacheKeyName, async (cancellationToken) => await getValue(cancellationToken));

            var currentValue = await getMaxValue(cancellationToken);
            var lastValue = value.LastOrDefault();

            var identifierValue = selectId(lastValue);

            var isOutdated = false;

            var currentValType = _is
                .TryDetermineType(currentValue, out var currentVal);
            var idType = _is
                .TryDetermineType(identifierValue, out var idVal);

            if (currentValType != OfType.String
                && idType != OfType.String)
                isOutdated = currentVal > idVal;

            if (isOutdated)
                return await Set(cacheType, cacheKeyName, getValue, cancellationToken);

            return value;
        }

        public async Task<T> GetOrSet<T>(CacheType cacheType, string cacheKeyName, 
            Func<CancellationToken, Task<T>> getValue, bool append = false, 
            CancellationToken cancellationToken = default)
        {
            var value = await Get<T>(cacheType, cacheKeyName, cancellationToken);

            return await Set(cacheType, cacheKeyName, 
                    async(cancellationToken) => await getValue(cancellationToken), cancellationToken);
        }

        public async Task Set<T>(CacheType cacheType, string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            if(value == null)
                return;

            var cacheService = GetCacheService(cacheType);
            await cacheService.Set(cacheKeyName, value, cancellationToken);

        }

        public async Task<T> Set<T>(CacheType cacheType, string cacheKeyName, 
            Func<T> getValue, CancellationToken cancellationToken = default)
        {
            var value = getValue();

            await Set(cacheType, cacheKeyName, value, cancellationToken);
            return value;
        }

        public async Task<T> Set<T>(CacheType cacheType, string cacheKeyName, 
            Func<CancellationToken, Task<T>> getValue, 
            CancellationToken cancellationToken = default)
        {
            var value = await getValue(cancellationToken);

            await Set(cacheType, cacheKeyName, value, cancellationToken);

            return value;
        }

        private ICacheService GetCacheService(CacheType cacheType)
        {
            return _cacheProviderFactory.GetCache(cacheType);
        }
    }
}