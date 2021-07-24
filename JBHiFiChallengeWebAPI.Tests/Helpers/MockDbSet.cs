﻿using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace JBHiFiChallengeWebAPI.Tests.Helpers
{
    public static class MockDbSet
    {
        public static Mock<IDbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var mock = new Mock<IDbSet<T>>();
            var queryData = data.AsQueryable();
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryData.Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryData.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            Type type = typeof(T);
            string colName = type.Name + "Id";
            var pk = type.GetProperty(colName);
            if (pk == null)
            {
                colName = type.Name + "ID";
                pk = type.GetProperty(colName);
            }
            if (pk != null)
            {
                mock.Setup(x => x.Add(It.IsAny<T>())).Returns((T x) =>
                {
                    if (pk.PropertyType == typeof(int)
                        || pk.PropertyType == typeof(Int32))
                    {
                        var max = data.Select(d => (int)pk.GetValue(d)).Max();
                        pk.SetValue(x, max + 1);
                    }
                    else if (pk.PropertyType == typeof(Guid))
                    {
                        pk.SetValue(x, Guid.NewGuid());
                    }
                    data.Add(x);
                    return x;
                });
                mock.Setup(x => x.Remove(It.IsAny<T>())).Returns((T x) =>
                {
                    data.Remove(x);
                    return x;
                });
                mock.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] id) =>
                {
                    var param = Expression.Parameter(type, "t");
                    var col = Expression.Property(param, colName);
                    var body = Expression.Equal(col, Expression.Constant(id[0]));
                    var lambda = Expression.Lambda<Func<T, bool>>(body, param);
                    return queryData.FirstOrDefault(lambda);
                });
            }

            return mock;
        }
    }
}
