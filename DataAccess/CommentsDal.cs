﻿using System.Threading.Tasks;
using DataAccess.Contracts.Blog;
using System.Threading;
using System;

namespace DataAccess
{
    public class CommentsDal : ICommentsDal
    {
        private PdDbContext DbContext { get; }

        public CommentsDal(PdDbContext dbContext) => DbContext = dbContext;

        public async Task<CommentDto> Create(CommentDto comment, CancellationToken cancellationToken = default(CancellationToken))
        {
            comment.CreatedOn = DateTimeOffset.Now;
            var commentDto = await DbContext.Comments.AddAsync(comment, cancellationToken).ConfigureAwait(false);
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return commentDto.Entity;
        }
    }
}
