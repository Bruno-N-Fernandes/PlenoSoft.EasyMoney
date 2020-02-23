using System;
using System.Collections.Generic;
using System.Linq;

namespace PlenoSoft.InvestFacil.Domain.Utils
{
	public static class DistinctExtension
	{
		public static IEnumerable<TSource> Distinct<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector)
		{
			return source.GroupBy(selector).Select(g => g.FirstOrDefault());
		}
	}
}