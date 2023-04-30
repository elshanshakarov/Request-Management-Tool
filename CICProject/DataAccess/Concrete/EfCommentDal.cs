﻿using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfCommentDal : EfEntityRepositoryBase<Comment, CICDbContext>, ICommentDal
    {
        public EfCommentDal(CICDbContext context) : base(context)
        {
        }
    }
}