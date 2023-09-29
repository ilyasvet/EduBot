﻿using GoodsDbLibrary.DbInterfaces;
using GoodsDbLibrary.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodsDbLibrary.CommandHandlers
{
    public class SelectCommandHandler<T> : CommandHandlerBase<T> where T : DbModel, new()
    {
        public SelectCommandHandler() : base() {}

        public async Task<T> GetEntity(int id)
        {
            var commandText = GetCommandTextBase();
            commandText.Append($" WHERE ID = {id}");

            var entities = await ExecuteReader(commandText.ToString());
            if(entities.Count == 0)
            {
                throw new KeyNotFoundException("There is no such entity");
            }

            return entities[0];
        }

        public async Task<List<T>> GetAllEntitiesFromTable()
        {
            var commandText = GetCommandTextBase();
            return await ExecuteReader(commandText.ToString());
        }

        private async Task<List<T>> ExecuteReader(string commandText)
        {
            List<T> entities = new List<T>();
            using (var command = new Command())
            {
                using (var reader = await command.ExecuteReader(commandText))
                {
                    while (reader.Read())
                    {
                        T entity = DbEntityFactory.CreateEntity<T>(reader);
                        entities.Add(entity);
                    }
                    return entities;
                }
            }
        }

        protected override StringBuilder GetCommandTextBase()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"SELECT * FROM {_tableName}");
            return result;
        }
    }
}
