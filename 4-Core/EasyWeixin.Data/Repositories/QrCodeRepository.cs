using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class QrCodeRepository : BaseRepository<QrCode>, IQrCodeRepository
    {
        private readonly IEntityFrameworkRepositoryContext _efContext;

        public QrCodeRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this._efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IQrCodeRepository : IBaseRepository<QrCode>
    {
    }
}