using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

public static class QueryableExtensions
{

    public static IQueryable<T> IncludeAll<T>(this IQueryable<T> query) where T : class
    {
        // Получаем тип сущности
        var entityType = typeof(T);

        // Получаем все свойства сущности
        var properties = entityType.GetProperties();

        // Перебираем все свойства
        foreach (var property in properties)
        {
            // Проверяем, является ли свойство навигационным
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                // Включаем навигационное свойство в запрос
                query = query.Include(property.Name);
            }
        }

        return query;
    }
}