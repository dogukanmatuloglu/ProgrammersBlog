using AutoMapper;
using ProgrammersBlog.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Concrete
{
    public class ManagerBase
    {
        protected IUnitOfWork UnitOfWork { get; }
        protected IMapper Mapper { get; }

        public ManagerBase( IUnitOfWork unitOfWork,IMapper mapper)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }
    }
}
