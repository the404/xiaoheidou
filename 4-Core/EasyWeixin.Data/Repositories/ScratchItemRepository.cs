﻿using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ScratchItemRepository : BaseRepository<ScratchItem>, IScratchItemRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ScratchItemRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IScratchItemRepository : IBaseRepository<ScratchItem>
    {
    }
}